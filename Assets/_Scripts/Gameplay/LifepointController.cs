namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;

    public class LifepointController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_HUD.Lifepoints > _Hearts.Count)
            {
                AddHeart();
            }
            if (_HUD.Lifepoints < _Hearts.Count)
            {
                RemoveHeart();
            }
        }

        private void AddHeart()
        {
            var item = Instantiate(_HeartPrefab);
            _Hearts.Add(item);
            var displacement = new Vector3(Random.Range(-15.05f, 15.05f), Random.Range(-30.05f, 30.05f), 0);
            item.transform.position = gameObject.transform.position + displacement;
            var actorName = string.Format("Heart{0}", _AutoIncrement++);
            item.GetComponent<Actor>().Create(actorName);
        }
        private void RemoveHeart()
        {
            if (_Hearts.Count < 1) return;
            var item = _Hearts[0];
            _Hearts.RemoveAt(0);
            Destroy(item);
        }

        private readonly List<GameObject> _Hearts = new List<GameObject>();
        private int _AutoIncrement = 1;

        [SerializeField] private HUDController _HUD;
        [SerializeField] private GameObject _HeartPrefab;
    }
}