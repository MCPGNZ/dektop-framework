namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class Story : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Vector2Int _Begin;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            Begin();
            BeginAction();
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Overworld _Overworld;
        [Inject] private Explorer _Explorer;
        [Inject] private LevelParser _Parser;
        #endregion Private Variables

        #region Private Methods
        private void Begin()
        {

        START:
            var response = Dialog.Clipper("No!", "Yes", "No");
            switch (response)
            {
                case "Yes":
                {
                    goto START;
                }
                case "No":
                {
                    Dialog.Clipper("I am FED UP", "???");
                    Dialog.Clipper("No more playing games!", ":<");
                    break;
                }
            }

        }

        private void BeginAction()
        {
            /* setup explorer starting position */
            _Explorer.transform.position = _Parser.Explorer_UnityPosition;

            /* load first level */
            _Overworld.Load(_Begin);
        }
        #endregion Private Methods

    }
}