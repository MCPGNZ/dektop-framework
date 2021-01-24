namespace Mcpgnz.DesktopFramework
{
    using System.Runtime.InteropServices;

    public static class Callbacks
    {
        #region Public Types
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void Info([MarshalAs(UnmanagedType.LPStr)] string messagePtr);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void Error([MarshalAs(UnmanagedType.LPStr)] string messagePtr);
        #endregion Public Types
    }

}