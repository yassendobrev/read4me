using System.Runtime.InteropServices;

// http://stackoverflow.com/questions/354445/restore-windowstate-from-minimized
// used in WinBehaviour: this.Restore();
// because this.WindowState = FormWindowState.Normal; doesn't work

namespace System.Windows.Forms
{
    public static class Extensions
    {
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        private const uint SW_RESTORE = 0x09;

        public static void Restore(this Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                ShowWindow(form.Handle, SW_RESTORE);
            }
        }
    }
}