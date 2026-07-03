using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MuMu坐标计算
{
    internal sealed class MuMuKeymapSyncService : IDisposable
    {
        private FileSystemWatcher _watcher;
        private Timer _pollTimer;
        private readonly Dictionary<string, DateTime> _debounceCache = new Dictionary<string, DateTime>();
        private readonly Dictionary<string, string> _lastKnownPath = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly object _syncRoot = new object();
        private readonly SynchronizationContext _uiContext;
        private bool _isEnabled;
        private bool _disposed;
        private string _keymapDir;
        private const int DebounceMs = 600;
        private const int PollIntervalMs = 1000;
        private const int WatcherRestartDelayMs = 2000;
        private static readonly string _diagLogPath = Path.Combine(Path.GetTempPath(), "MuMu_Sync_Diag.log");

        private static void DiagLog(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
            try { File.AppendAllText(_diagLogPath, DateTime.Now.ToString("HH:mm:ss.fff") + " " + msg + Environment.NewLine); }
            catch { }
        }

        public event Action<string> KeymapChanged;

        public MuMuKeymapSyncService()
        {
            _uiContext = SynchronizationContext.Current;
            DiagLog("[MuMuKeymapSyncService] 诊断日志路径: " + _diagLogPath);
        }

        public void Start(string keymapDir)
        {
            _keymapDir = keymapDir;
            StopWatcher();
            StopPollTimer();

            if (string.IsNullOrEmpty(keymapDir) || !Directory.Exists(keymapDir))
                return;

            PrintDirectoryState("启动时");
            StartWatcher(keymapDir);
            StartPollTimer();
        }

        private void StartWatcher(string keymapDir)
        {
            try
            {
                _watcher = new FileSystemWatcher(keymapDir, "*.json")
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName,
                    IncludeSubdirectories = false,
                    InternalBufferSize = 65536
                };
                _watcher.Changed += OnFileEvent;
                _watcher.Created += OnFileEvent;
                _watcher.Deleted += OnFileEvent;
                _watcher.Renamed += OnFileEvent;
                _watcher.Error += OnWatcherError;
                _watcher.EnableRaisingEvents = true;
                DiagLog("[MuMuKeymapSyncService] Watcher已启动: " + keymapDir);
            }
            catch (Exception ex)
            {
                DiagLog("[MuMuKeymapSyncService] 启动Watcher失败: " + ex.Message);
            }
        }

        private void StopWatcher()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Changed -= OnFileEvent;
                _watcher.Created -= OnFileEvent;
                _watcher.Deleted -= OnFileEvent;
                _watcher.Renamed -= OnFileEvent;
                _watcher.Error -= OnWatcherError;
                _watcher.Dispose();
                _watcher = null;
            }
            DiagLog("[MuMuKeymapSyncService] Watcher已停止");
        }

        private void StartPollTimer()
        {
            StopPollTimer();
            _pollTimer = new Timer(OnPollTick, null, PollIntervalMs, PollIntervalMs);
        }

        private void StopPollTimer()
        {
            if (_pollTimer != null)
            {
                _pollTimer.Dispose();
                _pollTimer = null;
            }
        }

        public void Stop()
        {
            StopPollTimer();
            StopWatcher();
        }

        public void Enable()
        {
            _isEnabled = true;
            lock (_syncRoot)
            {
                _debounceCache.Clear();
            }
            if (!string.IsNullOrEmpty(_keymapDir))
            {
                PreloadLastKnownPaths();
                Start(_keymapDir);
            }
        }

        private void PreloadLastKnownPaths()
        {
            if (string.IsNullOrEmpty(_keymapDir) || !Directory.Exists(_keymapDir))
                return;
            try
            {
                var files = Directory.GetFiles(_keymapDir, "*.json", SearchOption.TopDirectoryOnly);
                lock (_syncRoot)
                {
                    foreach (string file in files)
                    {
                        string path = ReadCurrentPath(file);
                        if (path != null)
                            _lastKnownPath[file] = path;
                    }
                }
            }
            catch { }
        }

        public void Disable() => _isEnabled = false;

        public void ImmediatePoll()
        {
            if (!_isEnabled || string.IsNullOrEmpty(_keymapDir) || !Directory.Exists(_keymapDir))
                return;
            PollOnce();
        }

        private void OnFileEvent(object sender, FileSystemEventArgs e)
        {
            DiagLog("[MuMuKeymapSyncService] 文件事件: " + e.ChangeType + " => " + e.FullPath);

            if (!_isEnabled) return;
            string filePath = e.FullPath;
            if (string.IsNullOrEmpty(filePath)) return;

            lock (_syncRoot)
            {
                DateTime now = DateTime.UtcNow;
                if (_debounceCache.TryGetValue(filePath, out var last)
                    && (now - last).TotalMilliseconds < DebounceMs)
                    return;
                _debounceCache[filePath] = now;
            }

            ProcessFile(filePath);
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            DiagLog("[MuMuKeymapSyncService] Watcher错误, 2s后重启: " + e.GetException().Message);
            StopWatcher();

            Timer restartTimer = null;
            restartTimer = new Timer(_ =>
            {
                restartTimer?.Dispose();
                if (_isEnabled && !string.IsNullOrEmpty(_keymapDir))
                    StartWatcher(_keymapDir);
            }, null, WatcherRestartDelayMs, Timeout.Infinite);
        }

        private void PrintDirectoryState(string tag)
        {
            try
            {
                DiagLog("[MuMuKeymapSyncService] ====== " + tag + " 目录文件快照 ======");
                if (string.IsNullOrEmpty(_keymapDir) || !Directory.Exists(_keymapDir))
                {
                    DiagLog("  (目录不存在)");
                    return;
                }

                var topFiles = Directory.GetFiles(_keymapDir, "*", SearchOption.TopDirectoryOnly);
                DiagLog("  顶层文件(" + topFiles.Length + "个):");
                foreach (var f in topFiles)
                {
                    var fi = new FileInfo(f);
                    DiagLog(string.Format("    {0}  size={1}  lastWrite={2:HH:mm:ss.fff}",
                        fi.Name, fi.Length, fi.LastWriteTime));
                }

                var subFiles = Directory.GetFiles(_keymapDir, "*", SearchOption.AllDirectories);
                if (subFiles.Length > topFiles.Length)
                {
                    DiagLog("  所有子文件(" + subFiles.Length + "个):");
                    foreach (var f in subFiles)
                    {
                        var fi = new FileInfo(f);
                        DiagLog(string.Format("    {0}  size={1}  lastWrite={2:HH:mm:ss.fff}",
                            f.Substring(_keymapDir.Length + 1), fi.Length, fi.LastWriteTime));
                    }
                }
                DiagLog("  ====== 快照结束 ======");
            }
            catch (Exception ex)
            {
                DiagLog("[MuMuKeymapSyncService] 快照异常: " + ex.Message);
            }
        }

        private void OnPollTick(object state)
        {
            if (!_isEnabled) return;
            PollOnce();
        }

        private void PollOnce()
        {
            if (string.IsNullOrEmpty(_keymapDir) || !Directory.Exists(_keymapDir))
                return;

            try
            {
                var files = Directory.GetFiles(_keymapDir, "*.json", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (!_isEnabled) return;

                    string path = ReadCurrentPath(file);
                    if (path == null) continue;

                    lock (_syncRoot)
                    {
                        if (_lastKnownPath.TryGetValue(file, out string lastPath)
                            && string.Equals(lastPath, path, StringComparison.OrdinalIgnoreCase))
                            continue;

                        DateTime now = DateTime.UtcNow;
                        if (_debounceCache.TryGetValue(file, out var lastDebounce)
                            && (now - lastDebounce).TotalMilliseconds < DebounceMs)
                            continue;

                    }

                    DiagLog("[MuMuKeymapSyncService] 轮询检测到变更: " + file + " -> " + path);
                    TryNotifyPath(file, path);
                }

                bool watcherAlive = _watcher != null;
                if (!watcherAlive)
                    DiagLog("[MuMuKeymapSyncService] 警告: Watcher 已失效,只有轮询在工作!");
            }
            catch (Exception ex)
            {
                DiagLog("[MuMuKeymapSyncService] 轮询异常: " + ex.Message);
            }
        }

        private void ProcessFile(string filePath)
        {
            string path = ReadCurrentPath(filePath);
            if (path == null) return;
            TryNotifyPath(filePath, path);
        }

        private void TryNotifyPath(string indexFile, string path)
        {
            lock (_syncRoot)
            {
                if (_lastKnownPath.TryGetValue(indexFile, out string lastPath)
                    && string.Equals(lastPath, path, StringComparison.OrdinalIgnoreCase))
                    return;

                _lastKnownPath[indexFile] = path;
            }

            if (_uiContext != null)
                _uiContext.Post(_ => KeymapChanged?.Invoke(path), null);
        }

        public static string ReadCurrentPath(string indexFilePath)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    string content = File.ReadAllText(indexFilePath, Encoding.UTF8);
                    if (content.IndexOf("\"Current\"", StringComparison.Ordinal) < 0)
                        return null;

                    JObject json = JObject.Parse(content);
                    if (json["Current"] is JObject current && current.Count > 0)
                    {
                        string path = current.Properties().First().Value?["path"]?.Value<string>();
                        if (path == null) return null;
                        if (path.StartsWith("sanitary-", StringComparison.Ordinal))
                            return null;
                        return path.Replace('/', '\\');
                    }
                    return null;
                }
                catch (IOException)
                {
                    if (i < 4) Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    if (i < 4 && ex is Newtonsoft.Json.JsonReaderException)
                        Thread.Sleep(200);
                    else
                        return null;
                }
            }
            return null;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Stop();
            KeymapChanged = null;
        }
    }
}
