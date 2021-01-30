namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class Story : MonoBehaviour
    {
        #region Public Types
        public enum Character
        {
            Explorer = 0,
            Bin = 1,
            Windows = 2,
            Error = 3,
            Clippy = 4
        }
        #endregion Public Types

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
            Dialog.Character(Character.Error, "Error", "Error.", "Error?", "Malkovich!");

        START:
            var response = Dialog.Character(Character.Clippy, "No!", "Yes", "No");
            switch (response)
            {
                case "Yes":
                {
                    goto START;
                }
                case "No":
                {
                    Dialog.Character(Character.Clippy, "I am FED UP", "???");
                    Dialog.Character(Character.Clippy, "No more playing games!", ":<");
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