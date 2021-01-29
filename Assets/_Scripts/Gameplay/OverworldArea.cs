
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

        public void GoLeft()
        {
            var area = CurrentArea.GetComponent<LevelArea>();
            LoadLevel(LeftOf(area.LevelName));
        }
        public void GoRight()
        {
            var area = CurrentArea.GetComponent<LevelArea>();
            LoadLevel(RightOf(area.LevelName));
        }

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

        [SerializeField] public GameObject CurrentArea;
        [SerializeField] public GameObject LevelPrefab;
        [SerializeField] private Transform _Root;
    }
}