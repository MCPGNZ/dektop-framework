﻿namespace Mcpgnz.DesktopFramework
{
    using Mcpgnz.Utilities;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Framework/Config")]
    public sealed class Config : ScriptableObjectSingleton<Config>
    {
        #region Public Variables
        public static float MovementSpeed => Instance._MovementSpeed;

        public static GameObject Wall => Instance._Wall;
        #endregion Public Variables

        #region Private Variables
        [SerializeField, BoxGroup("Movement")]
        private float _MovementSpeed = 1.0f;

        [SerializeField, BoxGroup("Prefabs")]
        private GameObject _Wall;
        #endregion Private Variables
    }
}