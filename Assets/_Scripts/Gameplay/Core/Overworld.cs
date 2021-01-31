namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using UnityEngine;
    using UnityRawInput;
    using Zenject;
    using static LevelParser;

    public class Overworld : MonoBehaviour
    {
        #region Public Variables
        public static Action LevelChanged;
        public Vector2Int CurrentStageId => _CurrentStageId;
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

            /* force clean input */
            RawKeyInput.HandleKeyUp(RawKey.Left);
            RawKeyInput.HandleKeyUp(RawKey.Right);
            RawKeyInput.HandleKeyUp(RawKey.Up);
            RawKeyInput.HandleKeyUp(RawKey.Down);

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
            public static GameObject Instantiate(Identifier identifier, Transform parent, DiContainer container, Action<GameObject> initialize)
            {
                if (_FreeList.TryGetValue(identifier, out var result))
                {
                    /* if the free list is empty */
                    if (result.Count != 0)
                    {
                        var last = result.Dequeue();

                        /* handle state change */
                        OnActivated(last, initialize);
                        return last;
                    }
                }

                /* instantiate new entry */
                var prefab = Config.FindPrefab(identifier);
                var item = container.InstantiatePrefab(prefab, parent);
                _Instantiated.Add(item, identifier);

                OnActivated(item, initialize);
                return item;
            }

            public static void Release(GameObject obj)
            {
                if (obj == null) { return; }

                var identifier = _Instantiated[obj];

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
                OnDeactivated(obj);
            }
            public static void ReleaseAll()
            {
                foreach (var entry in _Instantiated)
                {
                    Release(entry.Key);
                }
            }
            #endregion Public Methods

            #region Private Methods
            private static void OnActivated(GameObject item, Action<GameObject> initialize)
            {
                initialize(item);

                /* notify */
                var pooled = item.GetComponent<IPooled>();
                pooled?.OnCreate();

                /* activate */
                item.SetActive(true);
            }
            private static void OnDeactivated(GameObject item)
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
            private static readonly Dictionary<GameObject, Identifier> _Instantiated =
                new Dictionary<GameObject, Identifier>();
            #endregion Private Variables
        }
        #endregion Private Types

        #region Private Variables
        [Inject, UsedImplicitly] private DiContainer _Container;
        [Inject, UsedImplicitly] private Explorer _Explorer;
        [Inject, UsedImplicitly] private LevelParser _Parser;

        private Vector2Int _CurrentStageId;
        private int _AutoIncrement = 1;

        private float _PortalLock;
        #endregion Private Variables

        #region Private Methods
        public void Release()
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
            if (cell.Type == Identifier.Unknown) { Debug.LogWarning("shouldn't get here"); return; }

            /* empty is empty... */
            if (cell.Type == Identifier.Empty) { return; }

            /* explorer is always on scene */
            if (cell.Type == Identifier.Explorer) { return; }

            Pool.Instantiate(cell.Type, transform, _Container,
                item =>
                {
                    InitializeActor(cell, item);
                    InitializePosition(item, position, gridSize);
                });
        }

        private void InitializeActor(Cell cell, GameObject item)
        {
            var actor = item.GetComponent<Actor>();
            if (actor != null)
            {
                actor.Create(_AutoIncrement++.ToString(), cell);
            }
        }

        private void InitializePosition(GameObject item, Vector2Int position, Vector2Int gridSize)
        {
            var normalizedPosition = Coordinates.GridToNormalized(position, gridSize);
            var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

            item.transform.localPosition = unityPosition;

            var component = item.GetComponent<Actor>();
            if (component != null) { component.UpdatePosition(true); }
        }
        #endregion Private Methods
    }
}