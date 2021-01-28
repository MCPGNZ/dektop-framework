namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public static class FrameworkEx
    {
        #region Public Metods
        public static void Initialize()
        {
            Initialize(OnUnmanagedInfo, OnUnmanagedError);
            DesktopEx.desktop_initialize();
        }

        public static Vector3 DesktopToUnityPosition(Vector2Int desktopPositio)
        {
            return new Vector2(desktopPositio.x, -desktopPositio.y);
        }
        public static Vector2Int UnityToDesktopPosition(Vector3 worldPosition)
        {
            return new Vector2Int((int)worldPosition.x, -(int)worldPosition.y);
        }
        #endregion Public Metods

        #region Import
        [DllImport("desktop-lib.dll", EntryPoint = "framework_initialize")]
        private static extern void Initialize(
            [MarshalAs(UnmanagedType.FunctionPtr)] Callbacks.Info callbackInfo,
            [MarshalAs(UnmanagedType.FunctionPtr)] Callbacks.Error callbackError);

        [DllImport("desktop-lib.dll", EntryPoint = "framework_cleanup")]
        public static extern void Cleanup();
        #endregion Import

        #region Private
        private static void OnUnmanagedInfo(string msg)
        {
            /* todo: add settings */
            // Debug.Log($"<color=#DAF7A6>Success: {msg}</color>");
        }
        private static void OnUnmanagedError(string msg)
        {
            Debug.LogError($"Error: {msg}");
        }

        internal static void SafeDelete(this DirectoryInfo info)
        {
            SafeDelete(info.FullName);
        }
        private static void SafeDelete(string rootPath)
        {
            File.SetAttributes(rootPath,
                File.GetAttributes(rootPath) & ~FileAttributes.ReadOnly);

            foreach (string directory in Directory.GetDirectories(rootPath))
            {
                SafeDelete(directory);
            }

            try
            {
                Directory.Delete(rootPath, true);
            }
            catch (IOException)
            {
                Directory.Delete(rootPath, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(rootPath, true);
            }
        }
        #endregion Private
    }
}