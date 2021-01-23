namespace Mcpgnz.DesktopFramework
{
    using System;
    using UnityEngine;
    using Vanara.PInvoke;

    public static class Log
    {
        #region Public Methods
        public static void Warning(object message)
        {
            Debug.LogWarning(message);
        }

        public static void HResult(HRESULT hresult, object message, bool showSuccess = false)
        {
            if (hresult != HRESULT.S_OK)
            {
                Console.WriteLine($"fail: {message} : {hresult}");
                Debug.LogError($"fail: {message} : {hresult}");
            }
            else if (showSuccess)
            {
                Console.WriteLine($"success: {message}");
                Debug.Log($"success: {message}");
            }
        }

        public static void HResult(int hresult, object message)
            => HResult(new HRESULT(hresult), message);

        public static void HResult(Win32Error error, object message)
            => HResult(error.ToHRESULT(), message);
        #endregion Public Methods
    }
}