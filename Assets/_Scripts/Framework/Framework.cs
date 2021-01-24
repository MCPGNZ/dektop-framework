namespace Mcpgnz
{
    using System;
    using System.Runtime.InteropServices;
    using Sirenix.OdinInspector;
    using UnityEngine;

    #region Types
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct item_token
    {
        public IntPtr name; /* LPWSTR */
        public IntPtr item; /* IShellItem* */
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
            DesktopEx.desktop_initialize();
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
    public static unsafe class DesktopEx
    {
        #region Api
        public static ItemEx[] Items
        {
            get
            {
                /* fetch data */
                var count = desktop_folders_count();
                _Items = (item_token*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(item_token)) * count);
                desktop_folders(_Items);

                /* create representation */
                var items = new ItemEx[count];
                for (int i = 0; i < count; ++i)
                {
                    var item = (item_token*)IntPtr.Add((IntPtr)_Items, i * sizeof(item_token));
                    items[i] = new ItemEx(item);
                }
                return items;
            }
        }
        #endregion Api

        #region Private Variables
        internal static item_token* _Items;
        #endregion Private Variables

        #region Import
        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_initialize();

        [DllImport("desktop-lib.dll")]
        private static extern int desktop_folders_count();

        [DllImport("desktop-lib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void desktop_folders([MarshalAs(UnmanagedType.LPArray)] item_token* folders);
        #endregion Import
    }

    [Serializable]
    public sealed unsafe class ItemEx
    {
        #region Public Variables
        [ShowInInspector]
        public string Name
        {
            get => Marshal.PtrToStringUni(_Token->name);
            set
            {
                var namePtr = Marshal.StringToCoTaskMemAuto(value);
                item_rename(_Token, namePtr);
            }
        }

        [ShowInInspector]
        public Vector2Int Position
        {
            get => new Vector2Int(_Token->x, _Token->y);
            set
            {
                _Token->x = value.x;
                _Token->y = value.y;
                item_update_position(_Token);
            }
        }
        #endregion Public Variables

        #region Public Methods
        public ItemEx(item_token* token)
        {
            _Token = token;
        }
        #endregion Public Methods

        #region Import
        [DllImport("desktop-lib.dll")]
        private static extern void item_rename([MarshalAs(UnmanagedType.LPArray)] item_token* token, IntPtr name);

        [DllImport("desktop-lib.dll")]
        private static extern void item_update_position([MarshalAs(UnmanagedType.LPArray)] item_token* token);
        #endregion Import

        #region Private Variables
        private item_token* _Token;
        #endregion Private Variables
    }
}