namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelArea : MonoBehaviour
    {
        #region Unity Methods
        private void Start()
        {
            var asset = (TextAsset)Resources.Load("_A1");
            loadFromString(asset.ToString());
        }
        private void OnDestroy()
        {
            // remove all things created by level
            foreach (var item in loadedObjects)
            {
                Destroy(item);
            }
        }
        #endregion Unity Methods

        #region Private Methods
        private void loadFromString(string level)
        {
            var lines = level.Split('\n');
            var gridSize = new Vector2Int(lines[0].Length, lines.Length);

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                // if you encounter an empty line, stop reading file
                if (line.Length < 2) { break; }
                // don't read the last char, its a newline
                for (int x = 0; x < line.Length - 1; x++)
                {
                    addActor(new Vector2Int(x, y), gridSize, line[x]);
                }
            }
        }
        private void addActor(Vector2Int cell, Vector2Int gridSize, char type)
        {
            if (type == ' ') { return; }
            Debug.Log($"At {cell.x}, {cell.y} I will spawn type: \"{type}\".");

            if (type == '#')
            {
                Create(wallPrefab, cell, gridSize);
            }
        }
        #endregion Private Methods

        #region Inspector Variables
        [SerializeField] private string _Name;
        [SerializeField] private Transform _Root;
        [SerializeField] public GameObject wallPrefab;
        #endregion Inspector Variables

        #region Private Variables
        private readonly List<GameObject> loadedObjects = new List<GameObject>();
        #endregion Private Variables

        #region Private Methods
        private GameObject Create(GameObject prefab, Vector2Int cell, Vector2Int gridSize)
        {
            var normalizedPosition = Coordinates.GridToNormalized(cell, gridSize);
            var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

            var item = Instantiate(prefab, _Root);
            item.transform.localPosition = unityPosition;
            loadedObjects.Add(item);

            return item;
        }
        #endregion Private Methods
    }
}