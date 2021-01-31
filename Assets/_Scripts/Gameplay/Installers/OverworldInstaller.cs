namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class OverworldInstaller : PreviewInstaller<Overworld>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Overworld).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Overworld _Overworld;
        #endregion Inspector Variables
    }
}