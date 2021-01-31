namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class MusicController : MonoBehaviour
    {
        void Start()
        {
            _Source = GetComponent<AudioSource>();
        }
        private void Awake()
        {
            Overworld.LevelChanged += OnLevelChange;
        }
        private void OnDestroy()
        {
            Overworld.LevelChanged -= OnLevelChange;
        }
        void OnLevelChange(Vector2Int levelId)
        {
            if (_MusicProgression == 0)
            {
                Sounds.WindowsStartup.Play();
                _MusicProgression++;
            }
            else if (_MusicProgression == 1)
            {
                _Source.clip = _AdventureMusic;
                _Source.Play();
                _MusicProgression++;
            }
            else if (_MusicProgression == 2 && levelId == _DarkLevel)
            {
                _Source.clip = _DarkMusic;
                _Source.Play();
                _MusicProgression++;
            }
            else if (_MusicProgression < 4 && levelId == _BossLevel)
            {
                _Source.clip = _BossMusic;
                _Source.Play();
                _MusicProgression = 4;
            }
        }

        [SerializeField] private AudioClip _AdventureMusic;
        [SerializeField] private AudioClip _DarkMusic;
        [SerializeField] private AudioClip _BossMusic;

        //[Inject] private Overworld _Overworld;

        private AudioSource _Source;
        private int _MusicProgression = 0;
        [SerializeField] private Vector2Int _DarkLevel = new Vector2Int(3, 2);
        [SerializeField] private Vector2Int _BossLevel = new Vector2Int(1, 4);
    }
}