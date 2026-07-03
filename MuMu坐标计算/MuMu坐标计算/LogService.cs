using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace MuMu坐标计算
{
    internal static class LogService
    {
        private const int MaxLogFiles = 10;
        private const int MaxFileSize = 2 * 1024 * 1024;
        private const int FlushIntervalMs = 3000;

        private static readonly object _writeLock = new object();
        private static readonly StringBuilder _buffer = new StringBuilder();
        private static string _logDir;
        private static string _currentLogFile;
        private static int _fileSequence;
        private static Timer _flushTimer;
        private static bool _initialized;

        public enum Level
        {
            Debug,
            Info,
            Warn,
            Error
        }

        public static Level MinimumLevel { get; set; } = Level.Info;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                _logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                Directory.CreateDirectory(_logDir);

                CleanupOldLogs();

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                _fileSequence = 1;
                _currentLogFile = Path.Combine(_logDir, "log_" + timestamp + ".txt");

                _flushTimer = new Timer(_ => Flush(), null, FlushIntervalMs, FlushIntervalMs);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[LogService] 初始化失败: " + ex.Message);
                _initialized = false;
            }
        }

        public static void Shutdown()
        {
            if (!_initialized) return;
            if (_flushTimer != null)
            {
                _flushTimer.Dispose();
                _flushTimer = null;
            }
            Flush();
            _initialized = false;
        }

        public static void Debug(string source, string message)
        {
            if (Level.Debug < MinimumLevel) return;
            Write(Level.Debug, source, message);
        }

        public static void Info(string source, string message)
        {
            Write(Level.Info, source, message);
        }

        public static void Warn(string source, string message)
        {
            Write(Level.Warn, source, message);
        }

        public static void Error(string source, string message)
        {
            Write(Level.Error, source, message);
        }

        public static void Error(string source, Exception ex, string context = null)
        {
            string msg = string.IsNullOrEmpty(context)
                ? string.Format("{0}: {1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace)
                : string.Format("[{0}] {1}: {2}\n{3}", context, ex.GetType().Name, ex.Message, ex.StackTrace);
            Write(Level.Error, source, msg);
        }

        private static void Write(Level level, string source, string message)
        {
            if (!_initialized) return;

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string entry = string.Format("[{0}] [{1}] [{2}] {3}{4}",
                time, level.ToString().ToUpperInvariant(), source, message, Environment.NewLine);

            lock (_writeLock)
            {
                _buffer.Append(entry);
                if (_buffer.Length > 65536)
                {
                    FlushInternal();
                }
            }
        }

        private static void Flush()
        {
            lock (_writeLock)
            {
                FlushInternal();
            }
        }

        private static void FlushInternal()
        {
            if (_buffer.Length == 0) return;
            try
            {
                EnsureFileSize();
                File.AppendAllText(_currentLogFile, _buffer.ToString(), Encoding.UTF8);
                _buffer.Clear();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[LogService] 写入日志文件失败: " + ex.Message);
            }
        }

        private static void EnsureFileSize()
        {
            try
            {
                if (!File.Exists(_currentLogFile)) return;
                var fileInfo = new FileInfo(_currentLogFile);
                if (fileInfo.Length < MaxFileSize) return;

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                _fileSequence++;
                _currentLogFile = Path.Combine(_logDir,
                    string.Format("log_{0}_{1:D3}.txt", timestamp, _fileSequence));
            }
            catch
            {
            }
        }

        private static void CleanupOldLogs()
        {
            try
            {
                var logFiles = new List<string>(Directory.GetFiles(_logDir, "log_*.txt"));
                if (logFiles.Count < MaxLogFiles) return;

                logFiles.Sort();
                while (logFiles.Count >= MaxLogFiles)
                {
                    try
                    {
                        File.Delete(logFiles[0]);
                        logFiles.RemoveAt(0);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[LogService] 清理旧日志失败: " + ex.Message);
            }
        }
    }
}
