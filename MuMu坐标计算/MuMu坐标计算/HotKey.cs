using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MuMu坐标计算
{
    internal class HotKey
    {
        [DllImportAttribute("user32.dll", EntryPoint = "RegisterHotKey", SetLastError = true)]
        public static extern bool RegisterHotKey(
        IntPtr hWnd,
        int id,
        KeyModifiers fsModifiers,
        Keys vk
);

        [DllImportAttribute("user32.dll", EntryPoint = "UnregisterHotKey", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,
            int id
        );

        [Flags]
        public enum KeyModifiers { 
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
    }
}