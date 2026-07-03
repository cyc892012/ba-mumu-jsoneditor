using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MuMu坐标计算
{
    internal class AdbClient : IDisposable
    {
        private string _adbPath;
        private string _deviceSerial;
        private Process _getEventProcess;
        private bool _disposed;

        public bool IsConnected { get; private set; }
        public string DeviceSerial { get { return _deviceSerial; } }

        public AdbClient(string adbPath, string deviceSerial)
        {
            _adbPath = adbPath;
            _deviceSerial = deviceSerial;
        }

        public void SetDevice(string adbPath, string deviceSerial)
        {
            _adbPath = adbPath;
            _deviceSerial = deviceSerial;
        }

        public string Execute(string arguments, int timeoutMs)
        {
            if (string.IsNullOrEmpty(_adbPath) || !File.Exists(_adbPath))
                throw new InvalidOperationException("ADB 路径无效。");

            var psi = new ProcessStartInfo
            {
                FileName = _adbPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using (var process = new Process { StartInfo = psi })
            {
                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();
                process.OutputDataReceived += (s, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };
                process.ErrorDataReceived += (s, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                if (!process.WaitForExit(timeoutMs))
                {
                    process.Kill();
                    throw new TimeoutException("ADB 命令执行超时。");
                }
                string output = outputBuilder.ToString();
                string error = errorBuilder.ToString();
                if (!string.IsNullOrEmpty(error))
                    return output + "\r\n" + error;
                return output;
            }
        }

        public bool Connect()
        {
            try
            {
                string result = Execute("connect " + _deviceSerial, 5000);
                IsConnected = result.Contains("connected to") || result.Contains("already connected");
                return IsConnected;
            }
            catch (TimeoutException ex)
            {
                LogService.Error("AdbClient", ex, "ADB连接超时");
                IsConnected = false;
                return false;
            }
            catch (Exception ex)
            {
                LogService.Error("AdbClient", ex, "ADB连接失败");
                IsConnected = false;
                return false;
            }
        }

        public void Disconnect()
        {
            try { Execute("disconnect " + _deviceSerial, 3000); }
            catch (Exception ex) { LogService.Error("AdbClient", ex, "ADB断开连接失败"); }
            IsConnected = false;
            StopGetEvent();
        }

        public string GetScreenSize()
        {
            try
            {
                string result = Execute("-s " + _deviceSerial + " shell wm size", 5000);
                int idx = result.LastIndexOf(':');
                if (idx >= 0)
                    return result.Substring(idx + 1).Trim();
                idx = result.IndexOf('\n');
                if (idx < 0)
                    return result.Trim();
                string lastLine = result.Substring(idx).Trim();
                if (lastLine.StartsWith("Override size:"))
                    return lastLine.Substring("Override size:".Length).Trim();
                return lastLine;
            }
            catch (Exception ex)
            {
                LogService.Error("AdbClient", ex, "获取屏幕分辨率失败");
                return "";
            }
        }

        public Process StartGetEvent()
        {
            if (string.IsNullOrEmpty(_adbPath) || !File.Exists(_adbPath))
                return null;

            StopGetEvent();

            var psi = new ProcessStartInfo
            {
                FileName = _adbPath,
                Arguments = "-s " + _deviceSerial + " shell getevent -lt",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            _getEventProcess = new Process { StartInfo = psi };
            return _getEventProcess;
        }

        public void StopGetEvent()
        {
            if (_getEventProcess != null)
            {
                try
                {
                    if (!_getEventProcess.HasExited)
                    {
                        _getEventProcess.Kill();
                        _getEventProcess.WaitForExit(3000);
                    }
                }
                catch { }
                finally
                {
                    _getEventProcess.Dispose();
                    _getEventProcess = null;
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            StopGetEvent();
        }

        public bool GetSensorMaxValues(out int maxX, out int maxY)
        {
            maxX = 0;
            maxY = 0;
            try
            {
                string result = Execute(
                    "-s " + _deviceSerial + " shell getevent -p /dev/input/event4",
                    5000);

                string[] lines = result.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (line.Contains("0035"))
                    {
                        int val = ExtractMaxFromParamLine(line);
                        if (val > maxX) maxX = val;
                    }
                    else if (line.Contains("0036"))
                    {
                        int val = ExtractMaxFromParamLine(line);
                        if (val > maxY) maxY = val;
                    }
                }
                return maxX > 0 && maxY > 0;
            }
            catch (Exception ex)
            {
                LogService.Error("AdbClient", ex, "获取传感器分辨率失败");
                return false;
            }
        }

        public bool GetMouseMaxValues(out int maxX, out int maxY)
        {
            maxX = 0;
            maxY = 0;
            try
            {
                string result = Execute(
                    "-s " + _deviceSerial + " shell getevent -p /dev/input/event3",
                    5000);

                string[] lines = result.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (line.Contains("0000  :") || line.Contains("0000,"))
                    {
                        int val = ExtractMaxFromParamLine(line);
                        if (val > maxX) maxX = val;
                    }
                    else if (line.Contains("0001  :") || line.Contains("0001,"))
                    {
                        int val = ExtractMaxFromParamLine(line);
                        if (val > maxY) maxY = val;
                    }
                }
                return maxX > 0 && maxY > 0;
            }
            catch (Exception ex)
            {
                LogService.Error("AdbClient", ex, "获取鼠标分辨率失败");
                return false;
            }
        }

        private static int ExtractMaxFromParamLine(string line)
        {
            int idx = line.IndexOf("max ", StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return 0;
            string rest = line.Substring(idx + 4);
            int endIdx = rest.IndexOfAny(new[] { ',', ' ', '\t' });
            if (endIdx >= 0) rest = rest.Substring(0, endIdx);
            rest = rest.Trim();
            if (int.TryParse(rest, out int val)) return val;
            return 0;
        }
    }
}