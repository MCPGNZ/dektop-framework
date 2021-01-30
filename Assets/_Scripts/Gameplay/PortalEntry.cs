﻿namespace Mcpgnz.DesktopFramework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    public class PortalEntry : MonoBehaviour
    {
        #region Unity Methods
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("teleportz");
            _Overworld.TeleportExplorerTo(PortalExitKey);
        }
        #endregion Unity Methods

        #region Public Variables
        [ShowInInspector, HideInEditorMode]
        public string PortalExitKey;
        #endregion Public Variables

        #region Private Variables
        [Inject]
        private Overworld _Overworld;
        #endregion Private Variables
    }
}