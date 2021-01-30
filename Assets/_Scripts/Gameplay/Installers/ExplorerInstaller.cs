namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class ExplorerInstaller : PreviewInstaller<Explorer>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Explorer).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Explorer _Explorer;
        #endregion Inspector Variables
    }
}