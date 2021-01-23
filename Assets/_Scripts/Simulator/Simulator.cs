// ReSharper disable All
namespace Mcpgnz.DesktopSimulator
{
    using System;
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class Simulator : MonoBehaviour
    {
        #region Public Methods
        public static Vector3 ToViewPosition(ItemEx item)
        {
            var position = item.Position;

            /* todo: proper conversion */
            return new Vector3(position.x, 1440 - position.y, 0.0f);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, BoxGroup("References")] private Canvas _ViewRoot;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            FrameworkEx.Initialize();
            DesktopEx.Initialize();

            Application.runInBackground = true;
        }
        private void Start()
        {
            CreateView();
        }

        private void OnDestroy()
        {
            FrameworkEx.Cleanup();
        }
        #endregion Unity Methods

        #region Private Variables
        private readonly List<FolderView> _FolderViews = new List<FolderView>();
        #endregion Private Variables

        #region Private Methods
        private void CreateView()
        {
            /* cleanup */
            DestroyView();

            /* build */
            var items = DesktopEx.Items;
            var folderPrefab = Config.Folder;

            foreach (var item in items)
            {
                var view = Instantiate(folderPrefab, _ViewRoot.transform);
                view.Bind(item);

                _FolderViews.Add(view);
            }

        }
        private void DestroyView()
        {
            foreach (var folderView in _FolderViews)
            {
                folderView.Unbind();
                Destroy(folderView.gameObject);
            }
            _FolderViews.Clear();
        }
        #endregion Private Methods
    }
}