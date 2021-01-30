
namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    public class DamageReaction : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_TimeToRevert < 0) return;
            _TimeToRevert -= Time.deltaTime;
            if (_TimeToRevert < 0) RevertIcon();
        }

        public void OnDamage()
        {
            var baseActor = GetComponent<Actor>();
            baseActor.ChangeIcon(_AlternativeIcon);
            _TimeToRevert = _Time;
        }

        private void RevertIcon()
        {
            var baseActor = GetComponent<Actor>();
            baseActor.ChangeIcon(_BaseIcon);
        }

        [SerializeField] private IconEx _AlternativeIcon;
        [SerializeField] private IconEx _BaseIcon;
        [SerializeField] private float _Time = 0.5f;

        private float _TimeToRevert = -1.0f;
    }
}