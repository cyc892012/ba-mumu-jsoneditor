using System;
using System.Runtime.InteropServices;

namespace MuMu坐标计算
{
    public class MouseSimulator
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern bool NativeGetCursorPos(out System.Drawing.Point lpPoint);

        public static System.Drawing.Point GetCursorPos()
        {
            if (!NativeGetCursorPos(out System.Drawing.Point point))
                return new System.Drawing.Point(0, 0);
            return point;
        }

        public static void MoveMouseTo(int x, int y)
        {
            bool result = SetCursorPos(x, y);
            if (!result)
            {
                throw new InvalidOperationException("Failed to move the mouse to the specified position.");
            }
        }
    }
}