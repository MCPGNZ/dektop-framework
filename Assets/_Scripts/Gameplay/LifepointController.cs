namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class LifepointController : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private GameObject _HeartPrefab;
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
        #endregion Private Variables

        #region Private Methods
        private void AddHeart()
        {
            var item = Instantiate(_HeartPrefab);
            _Hearts.Add(item);
            var displacement = new Vector3(Random.Range(-15.05f, 15.05f), Random.Range(-30.05f, 30.05f), 0);
            item.transform.position = gameObject.transform.position + displacement;

            var actorName = $"Heart{_AutoIncrement++}";
            item.GetComponent<Actor>().Create(actorName);
        }
        private void RemoveHeart()
        {
            if (_Hearts.Count < 1) return;
            var item = _Hearts[0];
            _Hearts.RemoveAt(0);
            Destroy(item);
        }
        #endregion Private Methods

    }
}