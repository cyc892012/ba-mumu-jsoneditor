using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MuMu坐标计算
{
    public class MouseSimulator
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        /// <summary>
        /// Moves the mouse cursor to the specified screen position.
        /// </summary>
        /// <param name="x">The X coordinate of the screen position.</param>
        /// <param name="y">The Y coordinate of the screen position.</param>
        public static void MoveMouseTo(int x, int y)
        {
            bool result = SetCursorPos(x, y);
            if (!result)
            {
                throw new Exception("Failed to move the mouse to the specified position.");
            }
        }
    }
}
