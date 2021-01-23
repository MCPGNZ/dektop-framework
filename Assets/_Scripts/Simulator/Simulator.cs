// ReSharper disable All
namespace Mcpgnz.DesktopSimulator
{
    using System.Collections.Generic;
    using Mcpgnz.DesktopFramework;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    public sealed class Simulator : MonoBehaviour
    {
        #region Public Methods
        public static Vector3 ToViewPosition(Item item)
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
            Application.runInBackground = true;
        }
        private void Start()
        {
            CreateView();
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Desktop _Desktop;

        private readonly List<FolderView> _FolderViews = new List<FolderView>();
        #endregion Private Variables

        #region Private Methods
        private void CreateView()
        {
            /* cleanup */
            DestroyView();

            /* build */
            var folders = _Desktop.Folders;
            var folderPrefab = Config.Folder;

            foreach (var folder in folders)
            {
                var view = Instantiate(folderPrefab, _ViewRoot.transform);
                view.Bind(folder);

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