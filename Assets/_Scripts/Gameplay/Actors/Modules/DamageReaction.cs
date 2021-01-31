
namespace Mcpgnz.DesktopFramework
{
    using System;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class DamageReaction : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            _BaseShakePosition = gameObject.transform.position * 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (_TimeToRevert < 0) return;
            _TimeToRevert -= Time.deltaTime;
            if (_TimeToRevert < 0) RevertIcon();
            ShakeIt();
        }

        public void OnDamage()
        {
            var baseActor = GetComponent<Actor>();
            baseActor.ChangeIcon(_AlternativeIcon);
            _TimeToRevert = _Time;
        }

        public void WhenDone(Action action)
        {
            _OnRevert = action;
        }

        private void RevertIcon()
        {
            var baseActor = GetComponent<Actor>();
            baseActor.ChangeIcon(_BaseIcon);
            if (_OnRevert != null) _OnRevert();
        }

        private void ShakeIt()
        {
            if (_ShakeStrength == 0) { return; }
            // Debug.Log($"Shaking from {_BaseShakePosition.x}, {_BaseShakePosition.y}");
            var isRigidbody = GetComponent<Rigidbody2D>() != null;
            var displacement = new Vector3(Random.Range(-_ShakeStrength, _ShakeStrength), Random.Range(-_ShakeStrength, _ShakeStrength), 0);
            if (!isRigidbody) gameObject.transform.position = _BaseShakePosition + displacement;
        }

        [SerializeField] private IconEx _AlternativeIcon;
        [SerializeField] private IconEx _BaseIcon;
        [SerializeField] private float _Time = 0.5f;
        [SerializeField] private float _ShakeStrength = 15.0f;

        private float _TimeToRevert = -1.0f;
        private Action _OnRevert;
        private Vector3 _BaseShakePosition;
    }
}