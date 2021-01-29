namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class Overworld : MonoBehaviour
    {
        #region Public Methods
        public void Load(Vector2Int levelId)
        {
            var stage = _Parser.World.Stage(levelId);
            for (int x = 0; x < stage.Size.x; ++x)
            {
                for (int y = 0; y < stage.Size.y; ++y)
                {
                    AddActor(stage[x][y], new Vector2Int(x, y), stage.Size);
                }
            }
        }

        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Transform _Root;
        [SerializeField] private GameObject Explorer;

        [SerializeField] private LevelParser _Parser;
        #endregion Inspector Variables

        #region Private Variables
        [Inject] private DiContainer _Container;

        private readonly List<GameObject> _Objects = new List<GameObject>();
        #endregion Private Variables

        #region Private Methods
        private void Release()
        {
            foreach (var entry in _Objects)
            {
                Destroy(entry);
            }
            _Objects.Clear();
        }

        private void TeleportExplorerBy(float x, float y)
        {
            var pos = Coordinates.UnityToNormalized(Explorer.transform.position);
            Explorer.transform.position = Coordinates.NormalizedToUnity(pos + new Vector2(x, y));
        }

        private void AddActor(LevelParser.Cell cell, Vector2Int position, Vector2Int gridSize)
        {
            switch (cell.Data)
            {
                case "#":
                {
                    var actor = Create<Actor>(Config.Wall, position, gridSize);
                    actor.Create($"Wall{Random.Range(-10.0f, 10.0f)}");
                    break;
                }
            }
        }

        private T Create<T>(GameObject prefab, Vector2Int cell, Vector2Int gridSize) where T : Component
        {
            var normalizedPosition = Coordinates.GridToNormalized(cell, gridSize);
            var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

            var item = _Container.InstantiatePrefab(prefab, transform);
            item.transform.localPosition = unityPosition;

            _Objects.Add(item);
            return item.GetComponent<T>();
        }
        #endregion Private Methods
    }

}