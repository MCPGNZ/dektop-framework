namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class Story : MonoBehaviour
    {
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
            Dialog.Error("Error", "Error.", "Error?", "Malkovich!");

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
            _Overworld.TeleportExplorerTo(_Parser.ExplorerCell);

            /* load first level */
            _Overworld.Load(_Parser.ExplorerCell.StageId);
        }
        #endregion Private Methods

    }
}
