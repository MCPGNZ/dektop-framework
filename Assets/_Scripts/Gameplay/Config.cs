namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Framework/Config")]
    public sealed class Config : ScriptableObjectSingleton<Config>
    {
        #region Public Variables
        public static Vector2Int UnitySize => Instance._UnitySize;
        public static Vector2Int StageSize => Instance._StageSize;

        public static float MovementSpeed => Instance._MovementSpeed;
        public static bool DisableIcons => Instance._DisableIcons;
        public static GameObject Wall => Instance._Wall;
        public static GameObject SpikeEnemy => Instance._SpikeEnemy;
        public static GameObject MineEnemy => Instance._MineEnemy;

        public static IconEx ExplorerAvatar => Instance._ExplorerAvatar;
        public static IconEx ClippyAvatar => Instance._ClippyAvatar;
        public static IconEx ErrorAvatar => Instance._ErrorAvatar;
        #endregion Public Variables

        #region Private Variables
        [SerializeField, BoxGroup("Stages")]
        private Vector2Int _UnitySize = new Vector2Int(1920, 1080);

        [SerializeField, BoxGroup("Stages")]
        private Vector2Int _StageSize = new Vector2Int(21, 10);

        [SerializeField, BoxGroup("Movement")]
        private float _MovementSpeed = 1.0f;

        [SerializeField, BoxGroup("DebugMode")]
        private bool _DisableIcons = false;

        [SerializeField, BoxGroup("Prefabs")]
        private GameObject _Wall;
        [SerializeField, BoxGroup("Prefabs")]
        private GameObject _SpikeEnemy;
        [SerializeField, BoxGroup("Prefabs")]
        private GameObject _MineEnemy;

        [SerializeField, BoxGroup("Avatars")]
        private IconEx _ExplorerAvatar;

        [SerializeField, BoxGroup("Avatars")]
        private IconEx _ClippyAvatar;

        [SerializeField, BoxGroup("Avatars")]
        private IconEx _ErrorAvatar;
        #endregion Private Variables
    }
}