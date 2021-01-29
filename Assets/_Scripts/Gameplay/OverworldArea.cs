
namespace Mcpgnz.DesktopFramework
{
    using System.Collections;
    using UnityEngine;

    public class OverworldArea : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            LoadLevel("A1");
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Public Functions
        public void GoLeft()
        {
            var area = CurrentArea.GetComponent<LevelArea>();
            LoadLevel(LeftOf(area.LevelName));
            TeleportExplorerBy(0.9f, 0);
        }
        public void GoRight()
        {
            var area = CurrentArea.GetComponent<LevelArea>();
            LoadLevel(RightOf(area.LevelName));
            TeleportExplorerBy(-0.9f, 0);
        }
        public void GoUp()
        {
            var area = CurrentArea.GetComponent<LevelArea>();
            LoadLevel(TopOf(area.LevelName));
            TeleportExplorerBy(0, 0.9f);
        }
        public void GoDown()
        {
            var area = CurrentArea.GetComponent<LevelArea>();
            LoadLevel(UnderOf(area.LevelName));
            TeleportExplorerBy(0, -0.9f);
        }
        #endregion

        #region Private Functions
        private void LoadLevel(string areaName)
        {
            if (CurrentArea != null)
            {
                Destroy(CurrentArea);
            }
            var item = Instantiate(LevelPrefab, _Root);
            CurrentArea = item;
            var area = item.GetComponent<LevelArea>();
            area.LevelName = areaName;
        }
        private string LeftOf(string areaName)
        {
            char[] nextAreaName = { (char)(areaName[0] - 1), areaName[1] };
            return new string(nextAreaName);
        }
        private string RightOf(string areaName)
        {
            char[] nextAreaName = { (char)(areaName[0] + 1), areaName[1] };
            return new string(nextAreaName);
        }
        private string TopOf(string areaName)
        {
            char[] nextAreaName = { areaName[0], (char)(areaName[1] - 1) };
            return new string(nextAreaName);
        }
        private string UnderOf(string areaName)
        {
            char[] nextAreaName = { areaName[0], (char)(areaName[1] + 1) };
            return new string(nextAreaName);
        }
        private void TeleportExplorerBy(float x, float y)
        {
            var pos = Coordinates.UnityToNormalized(Explorer.transform.position);
            Explorer.transform.position = Coordinates.NormalizedToUnity(pos + new Vector2(x, y));
        }
        #endregion

        [SerializeField] public GameObject Explorer;
        [SerializeField] public GameObject CurrentArea;
        [SerializeField] public GameObject LevelPrefab;
        [SerializeField] private Transform _Root;
    }
}