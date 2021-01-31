namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.DesktopFramework.Framework;
    using System.Collections;
    using UnityEngine;
    using Zenject;

    public class Story : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Vector2Int _BeginCutsceneStage;
        [SerializeField] private Vector2Int _BeginGameplayStage;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Explorer.OnLifeChange += OnExplorerLifeChange;
        }
        private void OnDestroy()
        {
            _Explorer.OnLifeChange -= OnExplorerLifeChange;
        }

        private void Start()
        {
            if (!Config.SkipIntro) { BeginDialogs(); }

            /* setup explorer starting position */
            var cell = _Parser.World.FindUnique(Identifier.Explorer);
            _Explorer.transform.position = cell.LocalUnityPosition;

            /* load first level */
            _Overworld.Load(_BeginCutsceneStage);
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Overworld _Overworld;
        [Inject] private Explorer _Explorer;
        [Inject] private LevelParser _Parser;
        #endregion Private Variables

        #region Private Methods
        public void StoryBegin()
        {
            /* setup explorer starting position */
            var cell = _Parser.World.FindUnique(Identifier.Explorer);
            _Explorer.transform.position = cell.LocalUnityPosition;

            /* load first level */
            _Overworld.Load(_BeginGameplayStage);
        }
        private void StoryEnd()
        {
            StartCoroutine(StoryEndAsync());
        }

        private IEnumerator StoryEndAsync()
        {
            yield return new WaitForSeconds(1);
            _Overworld.Release();
            Wallpaper.Set("blescreen.png");
            yield return new WaitForSeconds(3);
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
        private void EndDialogs()
        {
            Dialog.Character(Identifier.Clippy, "You are no more", null, "Sorry...");
        }

        private void OnExplorerLifeChange(int count)
        {
            if (_Explorer.Lives <= 0)
            {
                StoryEnd();
            }
        }
        #endregion Private Methods

    }
}