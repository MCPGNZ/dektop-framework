namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class GameInstaller : PreviewInstaller<Lifetime>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(lifetime).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Lifetime lifetime;
        #endregion Inspector Variables
    }
}