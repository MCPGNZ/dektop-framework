namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class Story : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Level _Begin;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            _Overworld.Load(_Begin);
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private OverworldArea _Overworld;
        #endregion Private Variables
    }
}