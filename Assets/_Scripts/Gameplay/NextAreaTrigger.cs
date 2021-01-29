
namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

    public class NextAreaTrigger : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name != "explorer") { Debug.Log("Should not happen lol."); }
            if (_LeftWall) { _OverworldArea.GoLeft(); }
            if (_RightWall) { _OverworldArea.GoRight(); }
        }

        [SerializeField] private OverworldArea _OverworldArea;
        [SerializeField] private bool _LeftWall = false;
        [SerializeField] private bool _RightWall = false;
        [SerializeField] private bool _TopWall = false;
        [SerializeField] private bool _BottomWall = false;
    }
}