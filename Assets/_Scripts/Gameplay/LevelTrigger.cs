namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public sealed class LevelTrigger : MonoBehaviour, ILevelElement
    {
        #region Public Methods
        public void Initialize(Level level)
        {
            _Level = level;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Level _Level;
        #endregion Inspector Variables

        #region Unity Methods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name != "Explorer [actor]") { Debug.Log("Should not happen lol."); }
            _OverworldArea.Load(_Level);
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private OverworldArea _OverworldArea;
        #endregion Private Variables
    }
}