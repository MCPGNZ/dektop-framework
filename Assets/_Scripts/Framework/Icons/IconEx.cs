namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Object = UnityEngine.Object;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(menuName = "Framework/Icon"), HideMonoScript]
    public sealed class IconEx : ScriptableObject
    {
        #region Public Methods
        public void Apply(IItemEx item)
        {
            var path = item.Path;

            CreateIcon(Path.Combine(path, "icon.ico"));
            CreateDesktopIni(Path.Combine(path, "desktop.ini"));
            CreateHidden(Path.Combine(path, ".hidden"));

            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.ReadOnly);
            SHChangeNotify(0x08000000, 0x0000, (IntPtr)null, (IntPtr)null);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Object _Asset;
        [SerializeField, ReadOnly] private string _Path;
        #endregion Inspector Variables

        #region Unity Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_Asset == null) { return; }

            /* todo: hardcoded */
            _Path = AssetDatabase.GetAssetPath(_Asset).Replace("Assets/StreamingAssets/", string.Empty);
        }
#endif
        #endregion Unity Methods

        #region Private Methods
        private void CreateIcon(string path)
        {
            File.Copy(Path.Combine(Application.streamingAssetsPath, _Path), path);
            File.SetAttributes(path,
                File.GetAttributes(path)
                | FileAttributes.Hidden);

        }
        private void CreateDesktopIni(string path)
        {
            File.WriteAllLines(path, new[]
            {
                "[.ShellClassInfo]",
                "IconResource=icon.ico,0",
                "[ViewState]",
                "Mode=",
                "Vid=",
                "FolderType=Generic"
            }, Encoding.UTF8);

            File.SetAttributes(path,
                File.GetAttributes(path)
                | FileAttributes.Hidden);
        }
        private void CreateHidden(string path)
        {
            File.WriteAllLines(path, new[]
            {
                "desktop.ini",
                "icon.ico"
            }, Encoding.UTF8);

            File.SetAttributes(path,
                File.GetAttributes(path)
                | FileAttributes.Hidden);

        }
        #endregion Private Methods

        #region Import
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(
            int wEventId, int uFlags, IntPtr dwItem1, IntPtr dwItem2);
        #endregion Import

    }
}