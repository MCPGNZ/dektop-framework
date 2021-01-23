namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Runtime.InteropServices;
    using Vanara.Extensions;
    using Vanara.PInvoke;

    /// <summary>
    /// references:
    ///     * https://www.codeproject.com/Questions/1055263/why-my-app-Hide-Show-Desktop-Icons-doesnt-work-on
    ///     * https://docs.microsoft.com/en-us/windows/win32/controls/lvm-setitemposition
    ///     * https://stackoverflow.com/questions/41423491/how-to-get-localized-name-of-known-folder/41427228
    ///     * https://docs.microsoft.com/en-us/windows/win32/api/exdisp/nn-exdisp-ishellwindows
    ///     * https://forum.unity.com/threads/using-win32-in-unity.523071/
    ///     * https://www.codeproject.com/Articles/639486/Save-and-Restore-Icon-Positions-on-Desktop
    ///     * https://docs.microsoft.com/en-us/windows/win32/winprog/windows-data-types
    ///     * https://stackoverflow.com/questions/30369537/getting-the-text-of-a-all-windows-desktop-shortcuts
    ///     * https://docs.microsoft.com/en-us/windows/win32/shell/knownfolderid
    ///     * https://www.magnumdb.com/search?q=SID_STopLevelBrowser
    ///     * https://forum.unity.com/threads/dll-reference-compiler-error.858103/
    ///     * https://docs.microsoft.com/en-us/windows/win32/stg/stgm-constants
    /// </summary>
    public sealed class Framework
    {
        #region Public Types
        public static class Desktop
        {
            #region Public Variables
            public static HWND Handle
            {
                get
                {
                    if (_Handle != default) { return _Handle; }
                    return _Handle = QueryHandle();
                }
            }
            public static Kernel32.SafeHPROCESS Process
            {
                get
                {
                    if (_Process != default) { return _Process; }
                    return _Process = Query_ProcessHandle(Handle);
                }
            }
            public static Shell32.PIDL PIDL
            {
                get
                {
                    if (_PIDL != default) { return _PIDL; }
                    return _PIDL = QueryPIDL();
                }
            }
            public static string Path
            {
                get
                {
                    if (_Path != default) { return _Path; }
                    return _Path = QueryPath();
                }
            }
            #endregion Public Variables

            #region Private Variables
            public static HWND _Handle;
            public static Kernel32.SafeHPROCESS _Process;
            public static Shell32.PIDL _PIDL;
            public static string _Path;
            #endregion Private Variables

            #region Private Methods
            private static HWND QueryHandle()
            {
                var progman = User32.FindWindow(_ProgramManagerWindowName, null);
                {
                    var error = Kernel32.GetLastError();
                    Log.HResult(error, "User32.FindWindow(_ProgramManagerWindowName)");
                }

                var desktop = User32.FindWindowEx(progman, IntPtr.Zero, _DefView, null);
                {
                    var error = Kernel32.GetLastError();
                    Log.HResult(error, "User32.FindWindowEx(progman)");
                }

                var listView = User32.FindWindowEx(desktop, IntPtr.Zero, _ListView, null);
                {
                    var error = Kernel32.GetLastError();
                    Log.HResult(error, "User32.FindWindowEx(desktop)");
                }

                return listView;
            }
            private static Shell32.PIDL QueryPIDL()
            {
                var hresult = Shell32.SHGetKnownFolderIDList(PInvoke.Shell32.KNOWNFOLDERID.FOLDERID_Desktop,
                    Shell32.KNOWN_FOLDER_FLAG.KF_FLAG_DEFAULT, HTOKEN.NULL, out var pidl);
                Log.HResult(hresult, "Shell32.SHGetKnownFolderIDList(FOLDERID_Desktop)");

                return pidl;
            }
            private static string QueryPath()
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            }
            #endregion Private Methods
        }
        #endregion Public Types

        #region Public Methods
        public static void Initialize()
        {
            var hr = Ole32.CoInitializeEx(IntPtr.Zero, Ole32.COINIT.COINIT_APARTMENTTHREADED | Ole32.COINIT.COINIT_DISABLE_OLE1DDE);
            Log.HResult(hr, "Ole32.CoInitializeEx");
        }
        public static string Query_Name(Shell32.IShellItem shellItem)
        {
            return shellItem.GetDisplayName(Shell32.SIGDN.SIGDN_NORMALDISPLAY);
        }
        public static string Query_SystemPath(Shell32.IShellItem shellItem)
        {
            var attributes = shellItem.GetAttributes(Shell32.SFGAO.SFGAO_FILESYSTEM);
            if ((attributes & Shell32.SFGAO.SFGAO_FILESYSTEM) != 0)
            {
                return shellItem.GetDisplayName(Shell32.SIGDN.SIGDN_FILESYSPATH);
            }

            return null;
        }

        public static Kernel32.SafeHPROCESS Query_ProcessHandle(HWND handle)
        {
            User32.GetWindowThreadProcessId(handle, out var result);
            return Kernel32.OpenProcess(ACCESS_MASK.GENERIC_EXECUTE |
                                        ACCESS_MASK.GENERIC_READ | ACCESS_MASK.GENERIC_WRITE, false, result);
        }

        public static Shell32.IShellFolder Query_DesktopShellFolder()
        {
            var hresult = Shell32.SHGetDesktopFolder(out var desktopFolder);
            Log.HResult(hresult, $"{nameof(Query_DesktopShellFolder)} Shell32.SHGetDesktopFolder");

            return desktopFolder;
        }
        public static Shell32.IShellItem Query_ShellItem(Shell32.PIDL pidl)
        {
            var hresult = Shell32.SHCreateItemFromIDList(pidl, typeof(Shell32.IShellItem).GUID, out var itemObj);
            Log.HResult(hresult, $"{nameof(Query_ShellItem)} SHCreateItemFromIDList failed");

            return (Shell32.IShellItem)itemObj;
        }
        public static Shell32.IShellFolder Query_ShellFolder(Shell32.IShellItem shellItem)
        {
            var attributes = shellItem.GetAttributes(Shell32.SFGAO.SFGAO_FOLDER);
            if ((attributes & Shell32.SFGAO.SFGAO_FOLDER) == 0) { return null; }

            return (Shell32.IShellFolder)shellItem.BindToHandler(null, Shell32.BHID.BHID_SFObject.Guid(),
                typeof(Shell32.IShellFolder).GUID);
        }
        public static Ole32.IStorage Query_Storage(Shell32.IShellItem shellItem)
        {
            return (Ole32.IStorage)shellItem.BindToHandler(null, Shell32.BHID.BHID_Storage.Guid(),
                typeof(Ole32.IStorage).GUID);
        }
        public static Shell32.IPersistFolder2 Query_PersistFolder2(Shell32.IShellFolder shellFolder)
        {
            var hresult = InteropExtensions.QueryInterface(shellFolder,
                typeof(Shell32.IPersistFolder2).GUID, out var persistFolderObj);
            Log.HResult(hresult, $"{nameof(Query_PersistFolder2)} QueryInterface failed");

            return (Shell32.IPersistFolder2)persistFolderObj;
        }

        public static IntPtr VirtualAlloc(int bytes = 1024)
        {
            return Kernel32.VirtualAllocEx(Desktop.Process, IntPtr.Zero, bytes,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_RESERVE | Kernel32.MEM_ALLOCATION_TYPE.MEM_COMMIT,
                Kernel32.MEM_PROTECTION.PAGE_READWRITE);
        }
        public static void VirtualFree(IntPtr memoryPointer)
        {
            /* todo: ensure that process handle exists here */
            var hresult = Kernel32.VirtualFreeEx(Desktop.Process, memoryPointer, 0,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
            Log.HResult(hresult ? HRESULT.S_OK : HRESULT.E_FAIL, "Kernel32.VirtualFreeEx");
        }
        public static T VirtualFree<T>(IntPtr memoryPointer) where T : struct
        {
            var result = new T[1];

            /* todo: ensure that process handle exists here */
            var hresult = Kernel32.ReadProcessMemory(Desktop.Process,
                memoryPointer,
                Marshal.UnsafeAddrOfPinnedArrayElement(result, 0),
                Marshal.SizeOf(typeof(T)),
                out var _);
            Log.HResult(hresult ? HRESULT.S_OK : HRESULT.E_FAIL, "Kernel32.ReadProcessMemory");

            hresult = Kernel32.VirtualFreeEx(Desktop.Process, memoryPointer, 0,
                Kernel32.MEM_ALLOCATION_TYPE.MEM_RELEASE);
            Log.HResult(hresult ? HRESULT.S_OK : HRESULT.E_FAIL, "Kernel32.VirtualFreeEx");

            return result[0];
        }
        #endregion Public Methods

        #region private Variables
        private const string _ProgramManagerWindowName = "Progman";
        private const string _DefView = "SHELLDLL_DefView";
        private const string _ListView = "SysListView32";
        #endregion private Variables
    }
}