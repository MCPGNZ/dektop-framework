// ReSharper disable RedundantArgumentDefaultValue
/* for some reason ReSharper tries to remove PostMessage parameters (?) */

namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using PInvoke;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Vanara.PInvoke;
    using Shell32 = Vanara.PInvoke.Shell32;
    using User32 = Vanara.PInvoke.User32;

    [Serializable]
    public class Item
    {
        #region Public Variables
        [ShowInInspector, BoxGroup("Properties")]
        public string Name => Framework.Query_Name(_ShellItem);

        [ShowInInspector, BoxGroup("Properties")]
        public string Path => Framework.Query_SystemPath(_ShellItem);

        [ShowInInspector, BoxGroup("Properties"), ReadOnly]
        public Vector2Int Position
        {
            get
            {
                /* if we do not have an index */
                if (_Index == -1)
                {
                    Log.Warning($"{nameof(Item)}: index not set");
                    return default;
                }

                var memoryPointer = Framework.VirtualAlloc();

                var hresult = (int)User32.SendMessage(
                    Framework.Desktop.Handle,
                    (uint)ComCtl32.ListViewMessage.LVM_GETITEMPOSITION,
                    _Index, memoryPointer) == 1;
                Log.HResult(hresult ? HRESULT.S_OK : HRESULT.S_FALSE, "User32.SendMessage+LVM_GETITEMPOSITION");

                var point = Framework.VirtualFree<POINT>(memoryPointer);
                return new Vector2Int(point.x, point.y);
            }
            set
            {
                /* if we do not have an index */
                if (_Index == -1)
                {
                    Log.Warning($"{nameof(Item)}: index not set");
                    return;
                }

                var position = Macros.MAKELPARAM((ushort)value.x, (ushort)value.y);

                var result = User32.PostMessage(
                    Framework.Desktop.Handle,
                    (uint)ComCtl32.ListViewMessage.LVM_SETITEMPOSITION,
                    new IntPtr(_Index), position);
                Log.HResult(result ? HRESULT.S_OK : HRESULT.S_FALSE, "User32.SendMessage+LVM_SETITEMPOSITION");
            }
        }

        [ShowInInspector, BoxGroup("Properties"), ShowIf(nameof(_ShellFolder))]
        public List<Item> Folders => Query_Folders();
        #endregion Public Variables

        #region Public Methods
        public Item(Shell32.IFolderView2 parentFolderView, Shell32.IShellItem shellItem, int index = -1)
        {
            _ParentFolderView = parentFolderView;

            _ShellItem = shellItem;
            _ShellFolder = Framework.Query_ShellFolder(_ShellItem);

            _Index = index;
            _Pidl = new Shell32.PIDL(_ShellItem.GetDisplayName(Shell32.SIGDN.SIGDN_PARENTRELATIVE));
        }
        #endregion Public Methods

        #region Private Variables
        [ShowInInspector, ReadOnly, BoxGroup("Preview")]
        internal readonly int _Index;

        internal readonly Shell32.PIDL _Pidl;
        internal readonly Shell32.IShellItem _ShellItem;
        internal readonly Shell32.IShellFolder _ShellFolder;
        internal readonly Shell32.IFolderView2 _FolderView;

        internal readonly Shell32.IFolderView2 _ParentFolderView;
        #endregion Private Variables

        #region Private Methods
        private List<Item> Query_Folders()
        {
            /* FolderEx is not a folder */
            if (_ShellFolder == null) { return null; }

            var result = new List<Item>();

            var hresult = _ShellFolder.EnumObjects(HWND.NULL, Shell32.SHCONTF.SHCONTF_FOLDERS |
                                                                              Shell32.SHCONTF.SHCONTF_NONFOLDERS |
                                                                              Shell32.SHCONTF.SHCONTF_INCLUDEHIDDEN, out var idlist);
            Log.HResult(hresult, "_ShellFolder.EnumObjects failed");

            /* todo: could be optimized */
            int index = 0;
            var items = new IntPtr[1];
            while (idlist.Next(1, items, out var _) != HRESULT.S_FALSE)
            {
                /* query IShellItem */
                /* todo: move to framework */
                hresult = Shell32.SHCreateItemWithParent(_Pidl, _ShellFolder, items[0], typeof(Shell32.IShellItem).GUID, out var item);
                Log.HResult(hresult, "Shell32.SHCreateItemWithParent()");

                var shellItem = (Shell32.IShellItem)item;

                result.Add(new Item(_FolderView, shellItem, _Index));
                index++;
            }

            return result;

        }
        #endregion Private Methods
    }

}