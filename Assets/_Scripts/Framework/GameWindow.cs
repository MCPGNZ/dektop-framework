namespace Mcpgnz.DesktopFramework.Framework
{
    using System;
    using System.Runtime.InteropServices;

    // https://answers.unity.com/questions/27490/minimizing-and-maximizing-by-script.html
    public static class GameWindow
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        public static void Minimize()
        {
            ShowWindow(GetActiveWindow(), 2);
        }
    }
}