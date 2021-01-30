namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(Actor))]
    public class NPC : MonoBehaviour
    {
        #region Public Types
        public interface IEncounterAction
        {
            void Execute(NPC npc);
        }

        public sealed class TalkAction : IEncounterAction
        {
            public string Message;
            public string[] Options;
            public void Execute(NPC npc)
            {
                Dialog.Character(npc.Character, Message, Options);
            }
        }
        public sealed class TooltipAction : IEncounterAction
        {
            public string Tooltip;
            public void Execute(NPC npc)
            {
                npc._Actor.Tooltip = Tooltip;
            }
        }
        #endregion Public Types

        #region Public Variables
        public Identifier Character => character;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private Identifier character;
        [SerializeField] private List<Encounter> _Encounters;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Actor = GetComponent<Actor>();
        }
        #endregion Unity Methods

        #region Private Types
        [Serializable]
        private sealed class Encounter
        {
            public int Number;

            [SerializeReference, HideLabel]
            public IEncounterAction Action;
        }
        #endregion Private Types

        #region Private Variables
        [Inject] private Overworld _Overworld;

        [ShowInInspector, LabelText("Encounter count"), ReadOnly]
        private int _Count;
        private Actor _Actor;
        private readonly List<Vector2Int> _VisitedStages = new List<Vector2Int>();
        #endregion Private Variables

        #region Private Methods
        private void OnTriggerEnter2D(Collider2D collider)
        {
            /* exclude visited stages */
            var currentStage = _Overworld.CurrentStageId;
            if (_VisitedStages.Contains(currentStage)) { return; }
            _VisitedStages.Add(currentStage);

            /* do stuff */
            var rigidbody = collider.attachedRigidbody;
            if (rigidbody == null) { return; }

            var explorer = rigidbody.GetComponent<Explorer>();
            if (explorer == null) { }

            var found = _Encounters.Find(x => x.Number == _Count);
            found?.Action.Execute(this);

            _Count++;
        }
        #endregion Private Methods
    }
}