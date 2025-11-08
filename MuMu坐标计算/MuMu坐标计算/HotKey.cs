using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace MuMu坐标计算
{
    internal class HotKey
    {
        [DllImportAttribute("user32.dll", EntryPoint = "RegisterHotKey")]
        public static extern bool RegisterHotKey(
        IntPtr hWnd, // 要注册热键的窗口句柄
        int id, // 热键编号
        KeyModifiers fsModifiers, // 特殊键如:Ctrl,Alt,Shift,Window
        Keys vk // 一般键如:A B C F1,F2 等
);

        [DllImportAttribute("user32.dll", EntryPoint = "UnregisterHotKey")]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd, // 注册热键的窗口句柄
            int id // 热键编号上面注册热键的编号
        );

        [Flags()]
        public enum KeyModifiers { 
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }
    }
}
