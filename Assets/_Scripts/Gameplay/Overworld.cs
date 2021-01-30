﻿namespace Mcpgnz.DesktopFramework
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
        [SerializeField] private GameObject Explorer;

        [SerializeField] private LevelParser _Parser;
        #endregion Inspector Variables

        #region Private Variables
        [Inject] private DiContainer _Container;

        private Vector2Int _CurrentStageId;
        private readonly List<GameObject> _Objects = new List<GameObject>();
        private int _AutoIncrement = 1;
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

        public void TeleportExplorerTo(Cell cell)
        {
            if (_CurrentStageId != cell.StageId)
            {
                Load(cell.StageId);
            }
            Explorer.transform.position = Coordinates.NormalizedToUnity(Coordinates.GridToNormalized(cell.LocalPositionGrid, Config.StageSize));
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