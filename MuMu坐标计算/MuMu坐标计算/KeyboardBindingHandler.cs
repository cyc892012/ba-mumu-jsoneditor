using System;
using System.Threading;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    internal sealed class KeyboardBindingHandler : IDisposable
    {
        public const int HotKeyIdSave = 100;
        public const int HotKeyIdRecall = 101;

        private readonly GlobalKeyboardListener _listenerOnce = new GlobalKeyboardListener();
        private readonly GlobalKeyboardListener _listenerContinuously = new GlobalKeyboardListener();
        private bool _disposed;

        public KeyEventArgs BindKey1 { get; private set; }
        public string BindKey1ScanCode { get; private set; }
        public KeyEventArgs BindKey2 { get; private set; }
        public KeyEventArgs BindKey3 { get; private set; }
        public Keys FindKey { get; set; }
        public Keys ResetKey { get; set; }

        public bool IsKeyCreationActive
        {
            get { return _listenerOnce.IsListening || _listenerContinuously.IsListening; }
        }

        public event EventHandler<KeyEventArgs> KeyCapturedOnce;
        public event EventHandler<KeyEventArgs> KeyCapturedContinuously;
        public event EventHandler<KeyEventArgs> BindKey1Changed;
        public event EventHandler<KeyEventArgs> BindKey2Changed;
        public event EventHandler<KeyEventArgs> BindKey3Changed;
        public event EventHandler HotKeyChanged;

        public KeyboardBindingHandler()
        {
            var syncContext = SynchronizationContext.Current;

            _listenerOnce.KeyDownEvent += (sender, key) =>
            {
                _listenerOnce.StopListening();
                if (syncContext != null)
                    syncContext.Post(_ => KeyCapturedOnce?.Invoke(this, key), null);
                else
                    KeyCapturedOnce?.Invoke(this, key);
            };

            _listenerContinuously.KeyDownEvent += (sender, key) =>
            {
                if (syncContext != null)
                    syncContext.Post(_ => KeyCapturedContinuously?.Invoke(this, key), null);
                else
                    KeyCapturedContinuously?.Invoke(this, key);
            };
        }

        public void SetListenOnceMode(bool enabled)
        {
            if (enabled)
            {
                _listenerContinuously.StopListening();
                _listenerOnce.StartListening();
            }
            else
            {
                _listenerOnce.StopListening();
            }
        }

        public void SetListenContinuouslyMode(bool enabled)
        {
            if (enabled)
            {
                _listenerOnce.StopListening();
                _listenerContinuously.StartListening();
            }
            else
            {
                _listenerContinuously.StopListening();
            }
        }

        public void StopAllListening()
        {
            _listenerOnce.StopListening();
            _listenerContinuously.StopListening();
        }

        public void RefreshHotKeyRegistration(IntPtr handle, bool coordinateCaptureEnabled)
        {
            HotKey.UnregisterHotKey(handle, HotKeyIdSave);
            HotKey.UnregisterHotKey(handle, HotKeyIdRecall);

            if (coordinateCaptureEnabled)
            {
                if (FindKey != Keys.None && FindKey != 0)
                    HotKey.RegisterHotKey(handle, HotKeyIdSave, HotKey.KeyModifiers.Ctrl, FindKey);
                if (ResetKey != Keys.None && ResetKey != 0)
                    HotKey.RegisterHotKey(handle, HotKeyIdRecall, HotKey.KeyModifiers.Ctrl, ResetKey);
            }
        }

        public KeyEventArgs ProcessBindKey1KeyDown(KeyEventArgs e)
        {
            BindKey1 = e;
            try
            {
                var scanCode = MuMuJsonEditor.GetScanCode(e.KeyCode);
                BindKey1ScanCode = scanCode >= 0 ? scanCode.ToString() : "";
            }
            catch (Exception ex)
            {
                LogService.Error("KeyboardBindingHandler", ex, "获取扫描码失败");
                BindKey1ScanCode = "";
            }
            BindKey1Changed?.Invoke(this, e);
            return e;
        }

        public KeyEventArgs ProcessBindKey2KeyDown(KeyEventArgs e)
        {
            BindKey2 = e;
            BindKey2Changed?.Invoke(this, e);
            return e;
        }

        public KeyEventArgs ProcessBindKey3KeyDown(KeyEventArgs e)
        {
            BindKey3 = e;
            BindKey3Changed?.Invoke(this, e);
            return e;
        }

        public Keys ProcessFindKeyDown(KeyEventArgs e, out bool conflict)
        {
            conflict = (ResetKey == e.KeyCode);
            if (conflict) return FindKey;
            FindKey = e.KeyCode;
            HotKeyChanged?.Invoke(this, EventArgs.Empty);
            return FindKey;
        }

        public Keys ProcessResetKeyDown(KeyEventArgs e, out bool conflict)
        {
            conflict = (FindKey == e.KeyCode);
            if (conflict) return ResetKey;
            ResetKey = e.KeyCode;
            HotKeyChanged?.Invoke(this, EventArgs.Empty);
            return ResetKey;
        }

        public static string KeyToDisplayText(Keys key)
        {
            if (key == Keys.None || key == 0) return "";
            if (key == Keys.Escape) return "Esc";
            return key.ToString().ToUpper(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string KeyEventArgsToDisplayText(KeyEventArgs e)
        {
            if (e == null) return "";
            return KeyToDisplayText(e.KeyCode);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _listenerOnce.Dispose();
            _listenerContinuously.Dispose();
            KeyCapturedOnce = null;
            KeyCapturedContinuously = null;
            BindKey1Changed = null;
            BindKey2Changed = null;
            BindKey3Changed = null;
            HotKeyChanged = null;
        }
    }
}