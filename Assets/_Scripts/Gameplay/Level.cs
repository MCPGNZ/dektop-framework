namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Level : MonoBehaviour
    {
        #region Public Variables
        public string Name => _Asset.name;
        #endregion Public Variables

        #region Unity Methods
        private void Awake()
        {
            Load(_Asset);
        }
        private void OnDestroy()
        {
            foreach (var item in _Objects)
            {
                Destroy(item);
            }
        }
        #endregion Unity Methods

        #region Private Methods
        private void Load(TextAsset level)
        {
            var map = Parse(level.text);
            var gridSize = new Vector2Int(map.Length, map[0].Length);

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    AddActor(new Vector2Int(x, y), gridSize, map[x][y]);
                }
            }
        }
        private void AddActor(Vector2Int cell, Vector2Int gridSize, char type)
        {
            if (type == ' ') { return; }
            if (type == '#')
            {
                var actor = Create<Actor>(Config.Wall, cell, gridSize);
                actor.Create($"Wall{Random.Range(-10.0f, 10.0f)}");
            }
        }
        #endregion Private Methods

        #region Inspector Variables
        [SerializeField] public TextAsset _Asset;
        #endregion Inspector Variables

        #region Private Variables
        private readonly List<GameObject> _Objects = new List<GameObject>();
        #endregion Private Variables

        #region Private Methods
        private T Create<T>(GameObject prefab, Vector2Int cell, Vector2Int gridSize) where T : Component
        {
            var normalizedPosition = Coordinates.GridToNormalized(cell, gridSize);
            var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

            var item = Instantiate(prefab, transform);
            item.transform.localPosition = unityPosition;
            _Objects.Add(item);

            return item.GetComponent<T>();
        }

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
        #endregion Private Methods
    }
}