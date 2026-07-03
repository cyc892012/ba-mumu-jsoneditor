using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MuMu坐标计算
{
    internal struct TouchCoordinate
    {
        public double X;
        public double Y;
        public int ScreenWidth;
        public int ScreenHeight;
        public bool IsPress;

        public double RelX { get { return ScreenWidth > 1 ? X / (ScreenWidth - 1) : 0; } }
        public double RelY { get { return ScreenHeight > 1 ? Y / (ScreenHeight - 1) : 0; } }
    }

    internal class TouchEventParser
    {
        private int _sensorX = -1;
        private int _sensorY = -1;
        private int _mouseX = -1;
        private int _mouseY = -1;
        private int _mouseMaxX = 65535;
        private int _mouseMaxY = 65535;
        private int _mouseSeenMaxX;
        private int _mouseSeenMaxY;
        private bool _mouseActive;
        private int _screenWidth;
        private int _screenHeight;
        private bool _touching;
        private double _scaleX = 1.0;
        private double _scaleY = 1.0;
        private bool _useScale;
        private int _sensorMaxX;
        private int _sensorMaxY;
        private int _seenMaxX;
        private int _seenMaxY;
        private static readonly Regex _hexRegex = new Regex(@"\b([0-9a-fA-F]{4,8})\b", RegexOptions.Compiled);

        public event Action<TouchCoordinate> TouchDetected;
        public event Action<string> DebugOutput;

        public void SetScreenSize(int width, int height)
        {
            _screenWidth = width;
            _screenHeight = height;
            EmitDebug(string.Format("屏幕分辨率:{0}x{1}", width, height));
        }

        public void SetSensorScale(int sensorMaxX, int sensorMaxY)
        {
            _sensorMaxX = sensorMaxX > 0 ? sensorMaxX : 0;
            _sensorMaxY = sensorMaxY > 0 ? sensorMaxY : 0;
            _useScale = false;
            RecalcScale();
        }

        public void SetMouseScale(int mouseMaxX, int mouseMaxY)
        {
            if (mouseMaxX > 0) _mouseMaxX = mouseMaxX;
            if (mouseMaxY > 0) _mouseMaxY = mouseMaxY;
            EmitDebug(string.Format("鼠标集成分辨率:{0}x{1}", _mouseMaxX, _mouseMaxY));
        }

        private void RecalcScale() { if (_screenWidth <= 1 || _screenHeight <= 1) return; RecalcScaleInternal(); }

        private void RecalcScaleInternal()
        {
            int effectiveMaxX = Math.Max(_sensorMaxX, _seenMaxX);
            int effectiveMaxY = Math.Max(_sensorMaxY, _seenMaxY);

            if (effectiveMaxX > 0 && effectiveMaxY > 0
                && (effectiveMaxY > _screenWidth - 1 || effectiveMaxX > _screenHeight - 1))
            {
                _scaleX = (double)(_screenWidth - 1) / effectiveMaxY;
                _scaleY = (double)(_screenHeight - 1) / effectiveMaxX;
                _useScale = true;
                EmitDebug(string.Format("传感器缩放:Xmax={0} Ymax={1} scaleX={2:F6} scaleY={3:F6}",
                    effectiveMaxX, effectiveMaxY, _scaleX, _scaleY));
            }
            else if (effectiveMaxX > 0 && effectiveMaxY > 0)
            {
                _scaleX = 1.0;
                _scaleY = 1.0;
                _useScale = false;
                EmitDebug("传感器范围与屏幕一致，无需缩放");
            }
            else
            {
                _scaleX = 1.0;
                _scaleY = 1.0;
                _useScale = false;
                EmitDebug("传感器范围未知，使用1:1映射(自校准中)");
            }
        }

        public void ParseLine(string line)
        {
            try
            {
                if (string.IsNullOrEmpty(line)) return;

            if (line.Contains("ABS_X") || line.Contains("0003 0000"))
            {
                int val = ExtractLastHex(line);
                if (val >= 0) { _mouseX = val; _mouseActive = true; }
                return;
            }
            if (line.Contains("ABS_Y") || line.Contains("0003 0001"))
            {
                int val = ExtractLastHex(line);
                if (val >= 0) { _mouseY = val; _mouseActive = true; }
                return;
            }

            if (line.Contains("ABS_MT_POSITION_X"))
            {
                int val = ExtractLastHex(line);
                if (val >= 0) _sensorX = val;
            }
            else if (line.Contains("ABS_MT_POSITION_Y"))
            {
                int val = ExtractLastHex(line);
                if (val >= 0) _sensorY = val;
            }
            else if (line.Contains("BTN_TOUCH") && line.Contains("DOWN"))
            {
                _touching = true;
                EmitDebug(string.Format("[事件]DOWN sensor=({0},{1}) mouse=({2},{3})",
                    _sensorX, _sensorY, _mouseX, _mouseY));
            }
            else if (line.Contains("BTN_TOUCH") && line.Contains("UP"))
            {
                _touching = false;
                EmitDebug(string.Format("[事件]UP sensor=({0},{1}) mouse=({2},{3})",
                    _sensorX, _sensorY, _mouseX, _mouseY));
                FireTouchIfValid(false);
            }
            else if (line.Contains("SYN_REPORT") && _touching)
            {
                EmitDebug(string.Format("[事件]SYN sensor=({0},{1}) mouse=({2},{3})",
                    _sensorX, _sensorY, _mouseX, _mouseY));
                FireTouchIfValid(true);
            }
            else if (line.Contains("0003 0035"))
            {
                int val = ExtractLastHex(line);
                if (val >= 0) _sensorX = val;
            }
            else if (line.Contains("0003 0036"))
            {
                int val = ExtractLastHex(line);
                if (val >= 0) _sensorY = val;
            }
            else if (line.Contains("0001 014a") && line.TrimEnd().EndsWith("00000001"))
            {
                _touching = true;
                EmitDebug(string.Format("[事件]DOWN(hex) mouse=({0},{1})", _mouseX, _mouseY));
            }
            else if (line.Contains("0001 014a") && line.TrimEnd().EndsWith("00000000"))
            {
                _touching = false;
                EmitDebug(string.Format("[事件]UP(hex) mouse=({0},{1})", _mouseX, _mouseY));
                FireTouchIfValid(false);
            }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[TouchEventParser.ParseLine] " + ex.Message); }
        }

        private void FireTouchIfValid(bool isPress)
        {
            if (_screenWidth <= 0 || _screenHeight <= 0)
            {
                EmitDebug("[触发]跳过:分辨率无效");
                return;
            }

            bool useMouse = _mouseActive && _mouseX >= 0 && _mouseY >= 0;
            bool useSensor = _sensorX >= 0 && _sensorY >= 0;

            if (!useMouse && !useSensor)
            {
                EmitDebug("[触发]跳过:无有效坐标源");
                return;
            }

            if (_sensorX > _seenMaxX) { _seenMaxX = _sensorX; RecalcScale(); }
            if (_sensorY > _seenMaxY) { _seenMaxY = _sensorY; RecalcScale(); }

            double logicalX, logicalY;

            if (useMouse)
            {
                if (_mouseX > _mouseSeenMaxX) _mouseSeenMaxX = _mouseX;
                if (_mouseY > _mouseSeenMaxY) _mouseSeenMaxY = _mouseY;

                int effMouseMaxX = Math.Max(_mouseMaxX, _mouseSeenMaxX);
                int effMouseMaxY = Math.Max(_mouseMaxY, _mouseSeenMaxY);
                if (effMouseMaxX <= 0 || effMouseMaxY <= 0) return;

                logicalX = _mouseY * (_screenWidth - 1) / (double)effMouseMaxY;
                logicalY = (_screenHeight - 1) - _mouseX * (_screenHeight - 1) / (double)effMouseMaxX;

                EmitDebug(string.Format("[高精]mouse({0},{1})/{2},{3}→({4:F2},{5:F2})",
                    _mouseX, _mouseY, effMouseMaxX, effMouseMaxY, logicalX, logicalY));
            }
            else if (_useScale)
            {
                logicalX = _sensorY * _scaleX;
                logicalY = (_screenHeight - 1) - _sensorX * _scaleY;
            }
            else
            {
                logicalX = _sensorY;
                logicalY = _screenHeight - 1 - _sensorX;
            }

            EmitDebug(string.Format("[映射]→logical({0:F2},{1:F2})", logicalX, logicalY));

            if (logicalX < 0 || logicalY < 0 || logicalX >= _screenWidth || logicalY >= _screenHeight)
            {
                EmitDebug("[触发]跳过:映射后坐标越界");
                goto cleanup;
            }

            var coord = new TouchCoordinate
            {
                X = logicalX,
                Y = logicalY,
                ScreenWidth = _screenWidth,
                ScreenHeight = _screenHeight,
                IsPress = isPress
            };

            EmitDebug(string.Format("[结果]{0} raw=({1:F2},{2:F2}) rel=({3},{4})",
                isPress ? "按下" : "松开",
                coord.X, coord.Y,
                coord.RelX.ToString("F6"), coord.RelY.ToString("F6")));

            var handler = TouchDetected;
            if (handler != null) handler(coord);

        cleanup:
            if (!isPress)
            {
                _sensorX = -1;
                _sensorY = -1;
                _mouseActive = false;
            }
        }

        public void ResetSeenMaxValues()
        {
            _seenMaxX = 0;
            _seenMaxY = 0;
            _mouseSeenMaxX = 0;
            _mouseSeenMaxY = 0;
        }

        private int ExtractLastHex(string line)
        {
            var matches = _hexRegex.Matches(line);
            if (matches.Count > 0)
            {
                string hex = matches[matches.Count - 1].Groups[1].Value;
                if (int.TryParse(hex, NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out int value))
                    return value;
            }
            return -1;
        }

        private void EmitDebug(string msg)
        {
            var handler = DebugOutput;
            if (handler != null)
            {
                try { handler(msg); }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[TouchEventParser.ParseLine] " + ex.Message); }
            }
        }
    }
}
