namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Sirenix.OdinInspector;
    using Vanara.Extensions;
    using Vanara.PInvoke;

    [ShowInInspector]
    public class Desktop
    {
        #region Public Variables
        public List<Item> Folders => _Folders;
        #endregion Public Variables

        #region Public Methods
        public Desktop()
        {
            Framework.Initialize();
            _FolderView = Query_FolderView();
        }
        #endregion Public Methods

        #region Private Variables
        private List<Item> _Folders => Query_Folders();
        internal Shell32.IFolderView2 _FolderView;
        #endregion Private Variables

        #region Private Methods
        private int Query_FolderCount()
        {
            return _FolderView.ItemCount(0);
        }
        private List<Item> Query_Folders()
        {
            var result = new List<Item>();

            var count = Query_FolderCount();
            for (int i = 0; i < count; ++i)
            {
                var shellItem = _FolderView.GetItem<Shell32.IShellItem>(i);
                var systemPath = Framework.Query_SystemPath(shellItem);
                if (string.IsNullOrEmpty(systemPath)) { continue; }

                var folder = new Item(_FolderView, shellItem, i);
                result.Add(folder);
            }

            return result;
        }

        private Shell32.IFolderView2 Query_FolderView()
        {
            /* acquire Shell32.IShellWindows */
            var hresult = Ole32.CoCreateInstance(CLSID.ShellWindows, null, Ole32.CLSCTX.CLSCTX_LOCAL_SERVER,
                typeof(Shell32.IShellWindows).GUID, out var shellWindowsObj);
            Log.HResult(hresult, "CoCreateInstance");

            var shellWindows = (Shell32.IShellWindows)shellWindowsObj;

            /* acquire IServiceProvider */
            var dispatch = shellWindows.FindWindowSW(0, 0,
                Shell32.ShellWindowTypeConstants.SWC_DESKTOP,
                out var hwnd,
                Shell32.ShellWindowFindWindowOptions.SWFO_NEEDDISPATCH);

            hresult = InteropExtensions.QueryInterface(dispatch,
                typeof(Shell32.IServiceProvider).GUID, out var serviceProviderObj);
            Log.HResult(hresult, "QueryInterface+IServiceProvider");

            var serviceProvider = (Shell32.IServiceProvider)serviceProviderObj;

            /* acquire IShellBrowser */
            hresult = serviceProvider.QueryService(Shell32.SID_STopLevelBrowser,
                typeof(Shell32.IShellBrowser).GUID, out var shellBrowserPtr);
            Log.HResult(hresult, "QueryService");

            var shellBrowser = (Shell32.IShellBrowser)Marshal.GetObjectForIUnknown(shellBrowserPtr);

            /* acquire IShellView */
            hresult = shellBrowser.QueryActiveShellView(out var shellView);
            Log.HResult(hresult, "QueryActiveShellView");

            /* acquire IFolderView */
            hresult = InteropExtensions.QueryInterface(shellView,
                typeof(Shell32.IFolderView2).GUID, out var folderViewObj);
            Log.HResult(hresult, $"{nameof(Desktop)}+{nameof(Query_FolderView)}: QueryInterface+IFolderVIew");

            return (Shell32.IFolderView2)folderViewObj;
        }
        #endregion Private Methods
    }
}