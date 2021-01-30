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
        }

        [Serializable]
        public sealed class AvatarEntry
        {
            [HideLabel, HorizontalGroup, LabelWidth(64)]
            public Identifier Identifier;

            [HideLabel, HorizontalGroup, AssetsOnly]
            public IconEx Avatar;
        }
        #endregion Public Types

        #region Public Variables
        public static GameObject FindPrefab(Identifier id)
        {
            var found = Instance._Identifiers.Find(x => x.Identifier == id);
            if (found == null) { throw new InvalidOperationException($"prefab not found for: {Enum.GetName(typeof(Identifier), id)}"); }

            return found.Prefab;
        }
        public static IconEx FindAvatar(Identifier id)
        {
            var found = Instance._Avatars.Find(x => x.Identifier == id);
            if (found == null) { throw new InvalidOperationException($"avatar not found for: {Enum.GetName(typeof(Identifier), id)}"); }

            return found.Avatar;
        }

        public static Vector2Int UnitySize => Instance._UnitySize;
        public static Vector2Int StageSize => Instance._StageSize;

        public static float MovementSpeed => Instance._MovementSpeed;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField, BoxGroup("Stages")]
        private Vector2Int _UnitySize = new Vector2Int(1920, 1080);

        [SerializeField, BoxGroup("Stages")]
        private Vector2Int _StageSize = new Vector2Int(21, 10);

        [SerializeField, BoxGroup("Movement")]
        private float _MovementSpeed = 1.0f;

        [SerializeField]
        internal List<IdentifierEntry> _Identifiers;

        [SerializeField]
        private List<AvatarEntry> _Avatars;
        #endregion Inspector Variables

    }
}