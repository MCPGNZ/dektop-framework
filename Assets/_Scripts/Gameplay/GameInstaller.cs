namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class GameInstaller : PreviewInstaller<Game>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Game).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Game _Game;
        #endregion Inspector Variables
    }
}