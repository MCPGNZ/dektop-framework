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
        public sealed class PrefabEntry
        {
            public Identifier Identifier;
            public GameObject Prefab;
        }

        [Serializable]
        public sealed class AvatarEntry
        {
            public Identifier Identifier;
            public IconEx Avatar;
        }
        #endregion Public Types

        #region Public Variables
        public static GameObject FindPrefab(Identifier id)
        {
            return Instance._Prefabs.Find(x => x.Identifier == id).Prefab;
        }
        public static IconEx FindAvatar(Identifier id)
        {
            return Instance._Avatars.Find(x => x.Identifier == id).Avatar;
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
        private List<PrefabEntry> _Prefabs;

        [SerializeField]
        private List<AvatarEntry> _Avatars;
        #endregion Inspector Variables

    }
}