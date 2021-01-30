namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using Mcpgnz.DesktopFramework.Framework;
    using UnityEngine;
    using UnityRawInput;

    public sealed class Lifetime : MonoBehaviour
    {
        #region Unity Methods
        public void Awake()
        {
            Application.runInBackground = true;

            FrameworkEx.Initialize();
            RawKeyInput.Start(true);

            SetupDesktop();
            SetupItems();

            Minimize();
        }
        private void OnDestroy()
        {
            RawKeyInput.Stop();
            FrameworkEx.Cleanup();

            RestoreItems();
            RestoreSettings();
        }
        #endregion Unity Methods

        #region Private Types
        private sealed class StoredItem
        {
            #region Public Variables
            public readonly string Name;
            public Vector2Int DesktopPosition;
            #endregion Public Variables

            #region Public Methods
            public StoredItem(string name, Vector2Int desktopPosition)
            {
                Name = name;
                DesktopPosition = desktopPosition;
            }
            #endregion Public Methods

        }
        #endregion Private Types

        #region Private Variables
        private bool _AutoArrange;
        private bool _GridAlign;

        private List<StoredItem> _StoredItems;
        #endregion Private Variables

        #region Private Methods
        private void SetupDesktop()
        {
            Wallpaper.Backup();
            _AutoArrange = DesktopEx.Style(DesktopEx.FolderFlags.FWF_AUTOARRANGE);
            _GridAlign = DesktopEx.Style(DesktopEx.FolderFlags.FWF_SNAPTOGRID);

            Wallpaper.Set("BCG1.jpg");
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_AUTOARRANGE, false);
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_SNAPTOGRID, false);
        }

        private void SetupItems()
        {
            var items = DesktopEx.Items;

            /* store items */
            _StoredItems = new List<StoredItem>();
            foreach (var item in items)
            {
                var found = _StoredItems.Find(x => x.Name == item.Name);
                if (found != null)
                {
                    found.DesktopPosition = item.DesktopPosition;
                    continue;
                }

                _StoredItems.Add(new StoredItem(item.Name, item.DesktopPosition));
            }

            /* hide items */
            foreach (var item in items)
            {
                /* to the purgatory! */
                item.DesktopPosition = new Vector2Int(-8192, -8192);
            }
        }

        private void RestoreSettings()
        {
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_AUTOARRANGE, _AutoArrange);
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_SNAPTOGRID, _GridAlign);
            Wallpaper.Restore();
        }
        private void RestoreItems()
        {
            var items = DesktopEx.Items;
            foreach (var storedItem in _StoredItems)
            {
                var found = items.Find(x => x.Name == storedItem.Name);
                if (found != null)
                {
                    found.DesktopPosition = storedItem.DesktopPosition;
                }
            }
        }

        private void Minimize()
        {
            // don't minimize the editor
#if !UNITY_EDITOR
            Framework.GameWindow.Minimize();
#endif
        }
        #endregion Private Methods

    }
}