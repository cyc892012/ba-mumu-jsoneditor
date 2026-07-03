using System;
using System.IO;

namespace MuMu坐标计算
{
    internal sealed class FileMonitor : IDisposable
    {
        private FileSystemWatcher _watcher;
        private bool _disposed;
        private DateTime _lastEventTime;
        private readonly object _debounceLock = new object();
        private const int DebounceMs = 2000;

        public event Action FileChanged;

        public void Watch(string filePath)
        {
            Stop();
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;
            string dir = Path.GetDirectoryName(filePath);
            string name = Path.GetFileName(filePath);
            if (string.IsNullOrEmpty(dir)) return;
            _watcher = new FileSystemWatcher(dir, name)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                InternalBufferSize = 65536,
                EnableRaisingEvents = true
            };
            _watcher.Changed += OnFileChanged;
            _watcher.Error += OnWatcherError;
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            var ex = e.GetException();
            System.Diagnostics.Debug.WriteLine("[FileMonitor] FileSystemWatcher错误: " + ex.Message);
            LogService.Error("FileMonitor", ex, "FileSystemWatcher错误，正在重启...");
            string filePath = _watcher?.Path != null ? Path.Combine(_watcher.Path, _watcher.Filter) : null;
            Stop();
            if (filePath != null) Watch(filePath);
        }

        public void Stop()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Changed -= OnFileChanged;
                _watcher.Error -= OnWatcherError;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            lock (_debounceLock)
            {
                DateTime now = DateTime.UtcNow;
                if ((now - _lastEventTime).TotalMilliseconds < DebounceMs) return;
                _lastEventTime = now;
            }
            var handler = FileChanged;
            if (handler != null) handler();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Stop();
            FileChanged = null;
        }
    }
}