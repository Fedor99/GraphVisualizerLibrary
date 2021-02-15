using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GraphVisualizer
{
    public static class WindowsApi
    {
        private const int WmPaint = 0x000F;

        [DllImport("User32.dll")]
        public static extern Int64 SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static void ForcePaint(this Form form)
        {
            SendMessage(form.Handle, WmPaint, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
