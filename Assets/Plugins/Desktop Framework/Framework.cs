namespace Mcpgnz
{
    using System;
    using System.Runtime.InteropServices;
    using Sirenix.OdinInspector;
    using UnityEngine;

    #region Types
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct item_data
    {
        public IntPtr name;
        public int x;
        public int y;
    }
    #endregion Types

    [Serializable]
    public static class FrameworkEx
    {
        #region Api
        public static void Initialize()
        {
            Initialize(OnUnmanagedInfo, OnUnmanagedError);
            DesktopEx.Initialize();
        }
        #endregion Api

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
            Debug.Log($"<color=#DAF7A6>Success: {msg}</color>");
        }
        private static void OnUnmanagedError(string msg)
        {
            Debug.LogError($"Error: {msg}");
        }
        #endregion Private
    }

    [Serializable]
    public static class DesktopEx
    {
        #region Api
        public static ItemEx[] Items
        {
            get
            {
                /* fetch data */
                var count = GetFoldersCount();
                var data = new item_data[count];
                GetFolders(data);

                /* create representation */
                var items = new ItemEx[count];
                for (int i = 0; i < count; ++i)
                {
                    items[i] = new ItemEx(data[i]);
                }
                return items;
            }
        }
        #endregion Api

        #region Import
        [DllImport("desktop-lib.dll", EntryPoint = "desktop_initialize")]
        internal static extern void Initialize();

        [DllImport("desktop-lib.dll", EntryPoint = "desktop_folders_count")]
        private static extern int GetFoldersCount();

        [DllImport("desktop-lib.dll", EntryPoint = "desktop_folders", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetFolders([MarshalAs(UnmanagedType.LPArray)] item_data[] folders);
        #endregion Import
    }

    [Serializable]
    public sealed class ItemEx
    {
        #region Public Variables
        [ShowInInspector]
        public string Name => Marshal.PtrToStringUni(_Data.name);

        [ShowInInspector]
        public Vector2Int Position => new Vector2Int(_Data.x, _Data.y);
        #endregion Public Variables

        #region Public Methods
        public ItemEx(item_data data)
        {
            _Data = data;
        }
        #endregion Public Methods

        #region Private Variables
        private item_data _Data;
        #endregion Private Variables
    }
}