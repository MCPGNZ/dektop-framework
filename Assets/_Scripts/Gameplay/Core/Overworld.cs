namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;
    using static LevelParser;
    using Random = UnityEngine.Random;

    public class Overworld : MonoBehaviour
    {
        #region Public Variables
        public static Action LevelChanged;
        #endregion Public Variables

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
            LevelChanged?.Invoke();
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Transform _Root;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
            if (_PortalLock > 0.0f) { _PortalLock -= Time.deltaTime; }
        }
        #endregion Unity Methods

        #region Private Types
        public interface IPooled
        {
            void OnCreate();
            void OnRelease();
        }
        private sealed class Pool
        {
            #region Public Methods
            public static GameObject Instantiate(Identifier identifier,
                Vector2Int cell, Vector2Int gridSize, Transform parent,
                DiContainer container)
            {
                if (_FreeList.TryGetValue(identifier, out var result))
                {
                    /* if the free list is empty */
                    if (result.Count != 0)
                    {
                        var last = result.Dequeue();

                        /* handle state change */
                        OnCreate(last, cell, gridSize);
                        return last;
                    }
                }

                var prefab = Config.FindPrefab(identifier);

                var item = container.InstantiatePrefab(prefab, parent);
                var normalizedPosition = Coordinates.GridToNormalized(cell, gridSize);
                var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

                item.transform.localPosition = unityPosition;

                _Created.Add(item, identifier);
                return item;
            }

            public static void Release(GameObject obj)
            {
                if (obj == null) { return; }

                var identifier = _Created[obj];

                /* add to freelist */
                if (_FreeList.TryGetValue(identifier, out var result) == false)
                {
                    _FreeList.Add(identifier, new Queue<GameObject>());
                }

                /* check if contains */
                if (_FreeList[identifier].Contains(obj)) { return; }

                /* add to freelist */
                _FreeList[identifier].Enqueue(obj);

                /* handle state change */
                OnRelease(obj);
            }
            public static void ReleaseAll()
            {
                foreach (var entry in _Created)
                {
                    Release(entry.Key);
                }
            }
            #endregion Public Methods

            #region Private Methods
            private static void OnCreate(GameObject item, Vector2Int cell, Vector2Int gridSize)
            {
                /* update position */
                var normalizedPosition = Coordinates.GridToNormalized(cell, gridSize);
                var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);
                item.transform.localPosition = unityPosition;

                /* notify */
                var pooled = item.GetComponent<IPooled>();
                pooled?.OnCreate();

                /* activate */
                item.SetActive(true);
            }
            private static void OnRelease(GameObject item)
            {
                var pooled = item.GetComponent<IPooled>();
                pooled?.OnRelease();

                item.SetActive(false);
            }
            #endregion Private Methods

            #region Private Variables
            /* prefab to instances */
            private static readonly Dictionary<Identifier, Queue<GameObject>> _FreeList =
                new Dictionary<Identifier, Queue<GameObject>>();

            /* prefab to instances */
            private static readonly Dictionary<GameObject, Identifier> _Created =
                new Dictionary<GameObject, Identifier>();
            #endregion Private Variables
        }
        #endregion Private Types

        #region Private Variables
        [Inject] private DiContainer _Container;
        [Inject] private Explorer _Explorer;
        [Inject] private LevelParser _Parser;

        private Vector2Int _CurrentStageId;
        private int _AutoIncrement = 1;

        private float _PortalLock;
        #endregion Private Variables

        #region Private Methods
        private void Release()
        {
            Pool.ReleaseAll();
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
            var item = Pool.Instantiate(cell.Type, position, gridSize, transform, _Container);

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
        #endregion Private Methods
    }

}