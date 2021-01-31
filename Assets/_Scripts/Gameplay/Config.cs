namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using Mcpgnz.Utilities;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Framework/Config")]
    public sealed class Config : ScriptableObjectSingleton<Config>
    {
        #region Public Types
        [Serializable]
        public sealed class IdentifierEntry
        {
            [HideLabel, HorizontalGroup, LabelWidth(64)]
            public Identifier Identifier;

            [HideLabel, HorizontalGroup, LabelWidth(64)]
            public string Tag;

            [HideLabel, HorizontalGroup, AssetsOnly]
            public GameObject Prefab;

            [HideLabel, HorizontalGroup, AssetsOnly]
            public IconEx Avatar;
        }
        #endregion Public Types

        #region Public Variables
        public static bool DisableIcons => Instance._DisableIcons;
        public static bool SkipIntro => Instance._SkipIntro;
        public static bool SaveInput => Instance._SaveInput;

        public static GameObject FindPrefab(Identifier id)
        {
            var found = Instance._Identifiers.Find(x => x.Identifier == id);
            if (found == null) { throw new InvalidOperationException($"prefab not found for: {id.ToName()}"); }

            return found.Prefab;
        }
        public static IconEx FindAvatar(Identifier id)
        {
            var found = Instance._Identifiers.Find(x => x.Identifier == id);
            if (found == null) { throw new InvalidOperationException($"avatar not found for: {id.ToName()}"); }

            return found.Avatar;
        }

        public static Vector2Int UnitySize => Instance._UnitySize;
        public static Vector2Int StageSize => Instance._StageSize;

        public static float PortalLock => Instance._PortalLock;

        public static float MovementSpeed => Instance._MovementSpeed;

        public static int MinResponseSize => Instance._MinResponseSize;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField, BoxGroup("Development")]
        private bool _DisableIcons;

        [SerializeField, BoxGroup("Development")]
        private bool _SkipIntro;

        [SerializeField, BoxGroup("Development")]
        private bool _SaveInput;

        [SerializeField, BoxGroup("Stages")]
        private Vector2Int _UnitySize = new Vector2Int(1920, 1080);

        [SerializeField, BoxGroup("Stages")]
        private Vector2Int _StageSize = new Vector2Int(21, 10);

        [SerializeField, BoxGroup("Movement")]
        private float _MovementSpeed = 1.0f;

        [SerializeField, BoxGroup("Portals")]
        private float _PortalLock = 1.0f;

        [SerializeField, BoxGroup("Settings")]
        private int _MinResponseSize = 196;

        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 32)]
        internal List<IdentifierEntry> _Identifiers;
        #endregion Inspector Variables

    }
}