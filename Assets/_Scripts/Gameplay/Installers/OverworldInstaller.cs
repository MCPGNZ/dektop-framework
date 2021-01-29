namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class OverworldInstaller : PreviewInstaller<OverworldArea>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Overworld).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private OverworldArea _Overworld;
        #endregion Inspector Variables
    }
}