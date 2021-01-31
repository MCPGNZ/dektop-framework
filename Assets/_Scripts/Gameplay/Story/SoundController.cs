namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class SoundController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            _Source = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void Awake()
        {
            _Explorer.OnLifeChange += OnExplorerLifeChange;
        }
        private void OnDestroy()
        {
            _Explorer.OnLifeChange -= OnExplorerLifeChange;
        }

        void OnExplorerLifeChange(int count)
        {
            if (count < 0) { _Source.PlayOneShot(_LifelossSound); }
            if (count > 0) { _Source.PlayOneShot(_LifegainSound); }
        }

        [SerializeField] private AudioClip _LifegainSound;
        [SerializeField] private AudioClip _LifelossSound;

        [Inject] private Explorer _Explorer;

        private AudioSource _Source;
    }
}