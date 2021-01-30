namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;
    using static LevelParser;

    public class Overworld : MonoBehaviour
    {
        #region Public Methods
        public void Load(Vector2Int levelId)
        {
            /* cleanup */
            Release();

            /* load */
            var stage = _Parser.World.Stage(levelId);
            for (int x = 0; x < stage.Size.x; ++x)
            {
                for (int y = 0; y < stage.Size.y; ++y)
                {
                    AddActor(stage[x][y], new Vector2Int(x, y), stage.Size);
                }
            }
            _CurrentStageId = levelId;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Transform _Root;
        [SerializeField] private LevelParser _Parser;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
            if (_PortalLock > 0.0f) { _PortalLock -= Time.deltaTime; }
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private DiContainer _Container;
        [Inject] private Explorer _Explorer;

        private Vector2Int _CurrentStageId;
        private readonly List<GameObject> _Objects = new List<GameObject>();
        private int _AutoIncrement = 1;

        private float _PortalLock;
        #endregion Private Variables

        #region Private Methods
        private void Release()
        {
            /* todo: object pool */
            foreach (var entry in _Objects)
            {
                Destroy(entry);
            }
            _Objects.Clear();
        }

        public void TeleportExplorerTo(Cell cell)
        {
            /* portals are blocked for set amount of time */
            if (_PortalLock > 0.0f) { return; }

            /* lock portal for some time */
            _PortalLock = Config.PortalLock;

            /* stages are only reloaded if the portals leads to another stage */
            if (_CurrentStageId != cell.StageId)
            {
                Load(cell.StageId);
            }

            /* move the player */
            _Explorer.transform.position = Coordinates.NormalizedToUnity(Coordinates.GridToNormalized(cell.LocalPositionGrid, Config.StageSize));
        }
        public void TeleportExplorerTo(string targetKey)
        {
            var cell = _Parser.World.FindUnique(targetKey);
            TeleportExplorerTo(cell);
        }

        private void AddActor(Cell cell, Vector2Int position, Vector2Int gridSize)
        {
            /* empty is empty... */
            if (cell.Type == Identifier.Empty) { return; }

            /* explorer is always on scene */
            if (cell.Type == Identifier.Explorer) { return; }

            /* handle rest */
            var name = cell.Type.ToString();
            var prefab = Config.FindPrefab(cell.Type);
            var item = Create(prefab, position, gridSize);

            switch (cell.Type)
            {
                case Identifier.PortalEntry:
                {

                    var portal = item.GetComponent<Actor>();
                    portal.Create($"PortalEntry{Random.Range(-10.0f, 10.0f)}");
                    portal.GetComponent<PortalEntry>().PortalExitKey = cell.MatchingPortalExitKey;
                    break;
                }
                default:
                {
                    var actor = item.GetComponent<Actor>();
                    if (actor != null) { actor.Create($"{name}{_AutoIncrement++}"); }
                    break;
                }
            }
        }

        private GameObject Create(GameObject prefab, Vector2Int cell, Vector2Int gridSize)
        {
            var normalizedPosition = Coordinates.GridToNormalized(cell, gridSize);
            var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

            var item = _Container.InstantiatePrefab(prefab, transform);
            item.transform.localPosition = unityPosition;

            _Objects.Add(item);
            return item;
        }
        #endregion Private Methods
    }

}