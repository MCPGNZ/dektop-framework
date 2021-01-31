namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class LifepointController : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private GameObject _HeartPrefab;
        [SerializeField] private float _PlacementSpread = 45.0f;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
            if (_Explorer.Lives > _Hearts.Count)
            {
                AddHeart();
            }
            if (_Explorer.Lives < _Hearts.Count)
            {
                RemoveHeart();
            }
        }
        #endregion Unity Methods

        #region Private Variables
        private readonly List<GameObject> _Hearts = new List<GameObject>();
        private int _AutoIncrement = 1;

        [Inject] private Explorer _Explorer;
        [Inject] private Overworld _Overworld;
        #endregion Private Variables

        #region Private Methods
        private void AddHeart()
        {
            var item = Instantiate(_HeartPrefab, _Overworld.transform);
            _Hearts.Add(item);
            var displacement = new Vector3(
                Random.Range(-_PlacementSpread, _PlacementSpread),
                Random.Range(-_PlacementSpread, _PlacementSpread), 0);
            item.transform.position = gameObject.transform.position + displacement;

            var actorName = $"Heart{_AutoIncrement++}";
            item.GetComponent<Actor>().Create(actorName);
        }
        private void RemoveHeart()
        {
            if (_Hearts.Count < 1) return;
            var item = _Hearts[_Hearts.Count - 1];
            _Hearts.RemoveAt(_Hearts.Count - 1);

            var reaction = item.GetComponent<DamageReaction>();
            reaction.OnDamage();
            reaction.WhenDone(() => Destroy(item));
        }
        #endregion Private Methods

    }
}