using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    internal sealed class GlobalKeyboardListener : IDisposable
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private readonly LowLevelKeyboardProc _proc;
        private IntPtr _hookId = IntPtr.Zero;
        private bool _disposed;

        public bool IsListening => _hookId != IntPtr.Zero;

        public event EventHandler<KeyEventArgs> KeyDownEvent;

        public GlobalKeyboardListener()
        {
            _proc = HookCallback;
        }

        public void StartListening()
        {
            if (_hookId != IntPtr.Zero) return;

            using (var curProcess = Process.GetCurrentProcess())
            {
                var moduleName = curProcess.MainModule?.ModuleName;
                if (string.IsNullOrEmpty(moduleName))
                {
                    moduleName = curProcess.ProcessName + ".exe";
                }
                var moduleHandle = GetModuleHandle(moduleName);
                if (moduleHandle == IntPtr.Zero)
                    return;
                _hookId = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, moduleHandle, 0);
            }
        }

        public void StopListening()
        {
            if (_hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookId);
                _hookId = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            StopListening();
            KeyDownEvent = null;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                try
                {
                    KeyDownEvent?.Invoke(this, new KeyEventArgs(key));
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[GlobalKeyboardListener.HookCallback] " + ex.Message); LogService.Error("GlobalKeyboardListener", ex, "HookCallback异常"); }
            }
            return CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        #region Native Methods

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        #endregion
    }
}