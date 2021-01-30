namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class HUDInstaller : PreviewInstaller<HUDController>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Controller).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private HUDController _Controller;
        #endregion Inspector Variables
    }
}