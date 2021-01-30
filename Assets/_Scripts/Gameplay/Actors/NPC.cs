namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using Assets._Scripts.Gameplay.Actors;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(Actor))]
    public class NPC : MonoBehaviour
    {
        #region Public Variables
        [HideInInspector]
        public Actor Actor;
        public Identifier Character => character;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private Identifier character;
        [SerializeField] private NpcStory _Story;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            Actor = GetComponent<Actor>();
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Overworld _Overworld;

        [ShowInInspector, LabelText("Encounter count"), ReadOnly]
        private int _Count;

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

            var found = _Story.Encounters.Find(x => x.Number == _Count);
            if (found != null)
            {
                foreach (var action in found.Action)
                {
                    action.Execute(this);
                }
            }

            _Count++;
        }
        #endregion Private Methods
    }
}