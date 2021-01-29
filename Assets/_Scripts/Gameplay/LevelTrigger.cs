namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public sealed class LevelTrigger : MonoBehaviour, ILevelElement
    {
        #region Public Methods
        public void Initialize()
        {
        }
        #endregion Public Methods

        #region Unity Methods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name != "Explorer [actor]") { Debug.Log("Should not happen lol."); }
            // _Overworld.Load();
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Overworld _Overworld;
        #endregion Private Variables
    }
}