using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;

namespace MuMu坐标计算
{
    internal class TouchCollector : IDisposable
    {
        private readonly AdbClient _adb;
        private readonly TouchEventParser _parser = new TouchEventParser();
        private Thread _readThread;
        private volatile bool _running;
        private readonly object _lockObj = new object();
        private bool _disposed;

        public event Action<TouchCoordinate> CoordinateCaptured;
        public event Action<string> StatusChanged;
        public event Action<string> ErrorOccurred;
        public event Action<int, int> ResolutionDetected;

        public bool IsRunning { get { return _running; } }

        private readonly Dictionary<string, TouchCoordInfo> _uniqueCoords
            = new Dictionary<string, TouchCoordInfo>();

        public class TouchCoordInfo
        {
            public TouchCoordinate Coord;
            public int HitCount;
            public DateTime FirstSeen;
            public DateTime LastSeen;
        }

        public TouchCollector(AdbClient adb)
        {
            _adb = adb;
            _parser.TouchDetected += OnTouchDetected;
            _parser.DebugOutput += RaiseStatus;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _parser.TouchDetected -= OnTouchDetected;
            _parser.DebugOutput -= RaiseStatus;
            Stop();
        }

        public bool Start()
        {
            if (_running) return true;

            RaiseStatus("正在连接 ADB...");
            if (!_adb.Connect())
            {
                LogService.Error("TouchCollector", "ADB连接失败");
                RaiseError("ADB 连接失败，请检查端口号是否正确。");
                return false;
            }

            string sizeStr = _adb.GetScreenSize();
            if (string.IsNullOrEmpty(sizeStr))
            {
                LogService.Error("TouchCollector", "无法获取模拟器屏幕分辨率");
                RaiseError("无法获取模拟器屏幕分辨率。");
                _adb.Disconnect();
                return false;
            }

            string[] parts = sizeStr.Trim().Split('x');
            if (parts.Length != 2
                || !int.TryParse(parts[0], out int w)
                || !int.TryParse(parts[1], out int h))
            {
                LogService.Error("TouchCollector", "屏幕分辨率格式异常: " + sizeStr);
                RaiseError("屏幕分辨率格式异常：" + sizeStr);
                _adb.Disconnect();
                return false;
            }

            if (w < h)
            {
                int tmp = w;
                w = h;
                h = tmp;
            }

            _parser.SetScreenSize(w, h);
            _uniqueCoords.Clear();

            var resHandler = ResolutionDetected;
            if (resHandler != null) resHandler(w, h);

            if (_adb.GetMouseMaxValues(out int mouseMaxX, out int mouseMaxY))
            {
                _parser.SetMouseScale(mouseMaxX, mouseMaxY);
                RaiseStatus(string.Format("鼠标集成分辨率:{0}x{1}", mouseMaxX, mouseMaxY));
            }

            if (_adb.GetSensorMaxValues(out int sensorMaxX, out int sensorMaxY))
            {
                _parser.SetSensorScale(sensorMaxX, sensorMaxY);
            }

            Process process = _adb.StartGetEvent();
            if (process == null)
            {
                RaiseError("无法启动 getevent 进程。");
                _adb.Disconnect();
                return false;
            }

            try { process.Start(); }
            catch (Exception ex)
            {
                LogService.Error("TouchCollector", ex, "启动getevent进程失败");
                RaiseError("无法启动 getevent 进程：" + ex.Message);
                _adb.Disconnect();
                return false;
            }
            _running = true;
            _readThread = new Thread(() => ReadLoop(process))
            {
                IsBackground = true,
                Name = "TouchCollector"
            };
            _readThread.Start();

            RaiseStatus(string.Format("采集已启动({0}x{1})", w, h));
            return true;
        }

        public void Stop()
        {
            _running = false;
            _adb.StopGetEvent();
            _adb.Disconnect();

            Thread t;
            lock (_lockObj)
            {
                t = _readThread;
                _readThread = null;
            }
            if (t != null)
            {
                if (!t.Join(2000))
                {
                    t.Interrupt();
                    if (!t.Join(3000))
                    {
                        LogService.Warn("TouchCollector", "采集线程在3秒后仍未退出");
                    }
                }
            }

            RaiseStatus("采集已停止");
        }

        public List<TouchCoordInfo> GetUniqueCoords()
        {
            var list = new List<TouchCoordInfo>();
            lock (_uniqueCoords)
            {
                foreach (var kv in _uniqueCoords)
                    list.Add(kv.Value);
            }
            list.Sort((a, b) => b.HitCount.CompareTo(a.HitCount));
            return list;
        }

        public void ClearCoords()
        {
            _parser.ResetSeenMaxValues();
            lock (_uniqueCoords)
            {
                _uniqueCoords.Clear();
            }
        }

        private void ReadLoop(Process process)
        {
            try
            {
                using (var reader = process.StandardOutput)
                {
                    int lineCount = 0;
                    int touchLineStart = -1;
                    while (_running && !reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line == null) break;
                        lineCount++;
                        if (lineCount == 1)
                            RaiseStatus("采集已启动 - 开始接收事件数据...");
                        if (touchLineStart < 0
                            && (line.Contains("ABS_MT") || line.Contains("BTN_TOUCH")))
                            touchLineStart = lineCount;
                        _parser.ParseLine(line);
                    }
                    RaiseStatus(string.Format("采集线程结束, 共接收 {0} 行, 首个触摸行号: {1}",
                        lineCount, touchLineStart >= 0 ? touchLineStart.ToString() : "无"));
                }
            }
            catch (IOException ex)
            {
                if (_running)
                {
                    LogService.Error("TouchCollector", ex, "ADB管道IO异常");
                    RaiseError("ADB管道读取异常：" + ex.Message);
                }
            }
            catch (ThreadInterruptedException)
            {
            }
            catch (Exception ex)
            {
                LogService.Error("TouchCollector", ex, "ReadLoop采集异常");
                RaiseError("采集异常：" + ex.Message);
            }
        }

        private void OnTouchDetected(TouchCoordinate coord)
        {
            if (!coord.IsPress)
            {
                string key = string.Format(CultureInfo.InvariantCulture, "{0:F4},{1:F4}", coord.RelX, coord.RelY);
                int count;

                lock (_uniqueCoords)
                {
                    if (_uniqueCoords.ContainsKey(key))
                    {
                        _uniqueCoords[key].HitCount++;
                        _uniqueCoords[key].LastSeen = DateTime.Now;
                    }
                    else
                    {
                        _uniqueCoords[key] = new TouchCoordInfo
                        {
                            Coord = coord,
                            HitCount = 1,
                            FirstSeen = DateTime.Now,
                            LastSeen = DateTime.Now
                        };
                    }
                    count = _uniqueCoords.Count;
                }

                RaiseStatus(string.Format("[松开] x={0} y={1} rx={2:F4} ry={3:F4} 总数={4}",
                    coord.X, coord.Y, coord.RelX, coord.RelY, count));

                var handler = CoordinateCaptured;
                if (handler != null) handler(coord);
            }
            else
            {
                RaiseStatus(string.Format("[按下] x={0} y={1} rx={2:F4} ry={3:F4}",
                    coord.X, coord.Y, coord.RelX, coord.RelY));
            }
        }

        private void RaiseStatus(string msg)
        {
            var handler = StatusChanged;
            if (handler != null) handler(msg);
        }

        private void RaiseError(string msg)
        {
            var handler = ErrorOccurred;
            if (handler != null) handler(msg);
        }
    }
}
