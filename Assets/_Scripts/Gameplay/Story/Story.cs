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
            if (!Config.SkipIntro) { BeginDialogs(); }
            BeginAction();
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Overworld _Overworld;
        [Inject] private Explorer _Explorer;
        [Inject] private LevelParser _Parser;
        #endregion Private Variables

        #region Private Methods
        private void BeginDialogs()
        {
            Dialog.Character(Identifier.Error, "Error", "Error.", "Error?", "Malkovich!");

        START:
            var response = Dialog.Character(Identifier.Clippy, "No!", "Yes", "No");
            switch (response)
            {
                case "Yes":
                {
                    goto START;
                }
                case "No":
                {
                    Dialog.Character(Identifier.Clippy, "I am FED UP", "???");
                    Dialog.Character(Identifier.Clippy, "No more playing games!", ":<");
                    break;
                }
            }

        }
        private void BeginAction()
        {
            /* setup explorer starting position */
            var cell = _Parser.World.FindUnique(Identifier.Explorer);
            _Explorer.transform.position = cell.LocalUnityPosition;

            /* load first level */
            _Overworld.Load(_Begin);
        }
        #endregion Private Methods

    }
}