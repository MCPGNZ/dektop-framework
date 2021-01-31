namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    public sealed class StoryInstaller : PreviewInstaller<Story>
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInstance(_Story).AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Story _Story;
        #endregion Inspector Variables
    }
}