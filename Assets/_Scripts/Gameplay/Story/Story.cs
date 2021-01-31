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
        private void Awake()
        {
            _Explorer.OnLifeLost += OnExplorerLifeLost;
        }
        private void OnDestroy()
        {
            _Explorer.OnLifeLost -= OnExplorerLifeLost;
        }

        private void Start()
        {
            StoryBegin();
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Overworld _Overworld;
        [Inject] private Explorer _Explorer;
        [Inject] private LevelParser _Parser;
        #endregion Private Variables

        #region Private Methods
        private void StoryBegin()
        {
            if (!Config.SkipIntro) { BeginDialogs(); }
            BeginAction();
        }
        private void StoryEnd()
        {
            EndDialogs();
            Lifetime.Quit();
        }

        private void BeginDialogs()
        {
            Dialog.Character(Identifier.Error, "Your game seems to be damaged. Explorer not found.", null, "Ok", "Yes", "Confirm");

        START:
            var response = Dialog.Character(Identifier.Clippy, "No!", null, "Yes", "No");
            switch (response)
            {
                case "Yes":
                {
                    goto START;
                }
                case "No":
                {
                    Dialog.Character(Identifier.Clippy, "I am FED UP", null, "???");
                    Dialog.Character(Identifier.Clippy, "No more playing games!", null, ":<");
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

        private void EndDialogs()
        {
            Dialog.Character(Identifier.Clippy, "You are no more", null, "Sorry...");
        }

        private void OnExplorerLifeLost(int count)
        {
            if (_Explorer.Lives <= 0)
            {
                StoryEnd();
            }
        }
        #endregion Private Methods

    }
}