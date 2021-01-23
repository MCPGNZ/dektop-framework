namespace Mcpgnz.Utilities
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    /// <summary>
    ///     Binds type _T_ and shows it in inspector
    ///     note: by default binds InterfacesAndSelfTo[_T_], AsSingle, NonLazy
    /// </summary>
    public class PreviewInstaller<T> : MonoInstaller
    {
        #region Public Methods
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<T>().AsSingle().NonLazy();
        }
        #endregion Public Methods

        #region Unity Methods
        public override void Start()
        {
            base.Start();

            // _Preview = Container.Resolve<T>();
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeReference, InlineProperty, BoxGroup("Preview"), HideLabel] protected T _Preview;
        #endregion Inspector Variables
    }
}