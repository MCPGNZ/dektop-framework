namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using Mcpgnz.DesktopFramework.Framework;
    using UnityEngine;
    using UnityRawInput;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public sealed class Lifetime : MonoBehaviour
    {
        #region Public Methods
        public static void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion Public Methods

        #region Unity Methods
        public void Awake()
        {
            Application.runInBackground = true;

            FrameworkEx.Initialize();

            RawKeyInput.Start(true);
            RawKeyInput.InterceptMessages = true;
            RawKeyInput.OnKeyUp += OnExitKey;

            SetupDesktop();
            SetupItems();

            Minimize();

            string result = TextInput.Show("tytul", "dupadsaksag af sdf asdf asd ga eakgjre bkjre akjr gkjd gkjew fkaejr vkjd vakjfdbkejv fdjv kjfd vksj tkhjs ekrjfiaehab ihr ceidhv aieht ivj fng bwhtri gv?", "bin.ico");
            Debug.LogError($"AAA KURWA {result}");
        }
        private void OnDestroy()
        {
            RawKeyInput.Stop();
            FrameworkEx.Cleanup();

            RestoreItems();
            RestoreDesktop();
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
        private DesktopEx.FolderViewMode _Mode;
        private int _IconSize;

        private readonly Dictionary<DesktopEx.FolderFlags, bool> _Style =
            new Dictionary<DesktopEx.FolderFlags, bool>();

        private List<StoredItem> _StoredItems;
        #endregion Private Variables

        #region Private Methods
        private void SetupDesktop()
        {
            Wallpaper.Backup();

            Style(DesktopEx.FolderFlags.FWF_AUTOARRANGE, false);
            Style(DesktopEx.FolderFlags.FWF_SNAPTOGRID, false);

            DesktopEx.Icons(ref _Mode, ref _IconSize);
            var normalizedSize = new Vector2(1.0f, 1.0f) / (Config.StageSize + new Vector2Int(1, 1));
            var desktopSize = Coordinates.NormalizedToDesktop_Size(normalizedSize);
            Debug.Log(Mathf.Min(desktopSize.x, desktopSize.y));
            DesktopEx.Icons(_Mode, Mathf.Min(desktopSize.x, desktopSize.y));
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

        private void RestoreDesktop()
        {
            DesktopEx.Icons(_Mode, _IconSize);

            /* restore styles */
            foreach (var entry in _Style)
            {
                DesktopEx.Style(entry.Key, entry.Value);
            }

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
        private void OnExitKey(RawKey key)
        {
            if (key == RawKey.Escape)
            {
                Quit();
            }
        }

        private void Style(DesktopEx.FolderFlags flag, bool newValue)
        {
            _Style.Add(flag, DesktopEx.Style(flag));
            DesktopEx.Style(flag, newValue);
        }
        #endregion Private Methods

    }
}