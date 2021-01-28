namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelArea : MonoBehaviour
    {
        #region Unity Methods
        private void Start()
        {
            var asset = (TextAsset)Resources.Load("_A1");
            loadFromString(asset.text);
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
            var map = Parse(level);
            var gridSize = new Vector2Int(map.Length, map[0].Length);

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    addActor(new Vector2Int(x, y), gridSize, map[x][y]);
                }
            }
        }
        private void addActor(Vector2Int cell, Vector2Int gridSize, char type)
        {
            if (type == ' ') { return; }
            Debug.Log($"At {cell.x}, {cell.y} I will spawn type: \"{type}\".");

            if (type == '#')
            {
                var actor = Create<Actor>(wallPrefab, cell, gridSize);
                actor.Create(String.Format("Wall{0}", UnityEngine.Random.Range(-10.0f, 10.0f)));
            }
        }
        #endregion Private Methods

        #region Inspector Variables
        [SerializeField] private Transform _Root;
        [SerializeField] public GameObject wallPrefab;
        #endregion Inspector Variables

        #region Private Variables
        private readonly List<GameObject> loadedObjects = new List<GameObject>();
        #endregion Private Variables

        #region Private Methods
        private char[][] Parse(string level)
        {
            var lines = level.Split('\n');

            /* find row count */
            int rowCount;
            for (rowCount = 0; rowCount < lines.Length; ++rowCount)
            {
                if (string.IsNullOrWhiteSpace(lines[rowCount]))
                {
                    break;
                }
            }

            /* find column count */
            int columnCount = lines[0].Length - 1;

            /* create map */
            var map = new char[columnCount][];

            for (int x = 0; x < columnCount; ++x)
            {
                map[x] = new char[rowCount];

                /* fill */
                for (int y = 0; y < rowCount; ++y)
                {
                    map[x][y] = lines[y][x];
                }
            }

            return map;
        }

        private T Create<T>(GameObject prefab, Vector2Int cell, Vector2Int gridSize) where T : Component
        {
            var normalizedPosition = Coordinates.GridToNormalized(cell, gridSize);
            var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

            var item = Instantiate(prefab, _Root);
            item.transform.localPosition = unityPosition;
            loadedObjects.Add(item);

            return item.GetComponent<T>();
        }
        #endregion Private Methods
    }
}