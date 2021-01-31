namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class ParserInstaller : PreviewInstaller<LevelParser>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Parser).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private LevelParser _Parser;
        #endregion Inspector Variables
    }
}