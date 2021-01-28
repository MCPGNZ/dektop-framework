using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mcpgnz.DesktopFramework
{
    public class LevelArea : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            TextAsset asset = (TextAsset)Resources.Load("_A1");
            loadFromString( asset.ToString() );
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            // remove all things created by level
            foreach (var item in loadedObjects)
            {
                Destroy(item);
            }
        }

        private void loadFromString(string level)
        {
            string[] lines = level.Split('\n');
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                // if you encounter an empty line, stop reading file
                if (line.Length < 2) { break; }
                // don't read the last char, its a newline
                for (int x = 0; x < line.Length - 1; x++)
                {
                    addActor(x, y, line[x]);
                }
            }
        }

        private void addActor(int x, int y, char type)
        {
            if (type == ' ') { return; }
            Debug.Log(String.Format("At {0}, {1} I will spawn type: \"{2}\".", x, y, type));

            if (type == '#')
            {
                GameObject item = Instantiate(wallPrefab);
                loadedObjects.Add(item);
            }
        }

        #region Inspector Variables
        [SerializeField] private string _Name;
        [SerializeField] public GameObject wallPrefab;
        #endregion Inspector Variables

        #region Private Variables
        private List<GameObject> loadedObjects = new List<GameObject>();
        #endregion
    }
}