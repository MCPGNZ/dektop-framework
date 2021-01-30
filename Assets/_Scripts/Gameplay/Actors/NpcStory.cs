﻿namespace Assets._Scripts.Gameplay.Actors
{
    using System;
    using System.Collections.Generic;
    using Mcpgnz.DesktopFramework;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Framework/NPC Story")]
    internal class NpcStory : ScriptableObject
    {
        #region Public Variables
        public List<Encounter> Encounters;
        #endregion Public Variables

        #region Public Types
        public interface IEncounterAction
        {
            void Execute(NPC npc);
        }

        public sealed class TalkAction : IEncounterAction
        {
            public string Message;
            public string Tooltip;
            public IconEx Icon;

            [ListDrawerSettings(Expanded = true)]
            public string[] Options;
            public void Execute(NPC npc)
            {
                if (Icon != null) { Dialog.Character(npc.Character, Message, Icon, Options); }
                else { Dialog.Character(npc.Character, Message, Options); }

                if (string.IsNullOrWhiteSpace(Tooltip))
                {
                    npc.Actor.Tooltip = Tooltip;
                }

            }
        }

        public sealed class DisappearAction : IEncounterAction
        {
            public void Execute(NPC npc)
            {
                npc.gameObject.SetActive(false);
            }
        }

        [Serializable]
        public sealed class Encounter
        {
            public int Number;

            [SerializeReference, HideLabel]
            public List<IEncounterAction> Action;
        }
        #endregion Public Types
    }
}