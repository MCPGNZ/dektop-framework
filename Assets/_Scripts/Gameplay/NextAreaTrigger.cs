
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
            if (collision.gameObject.name != "Explorer [actor]") { Debug.Log("Should not happen lol."); }
            if (_LeftWall) { _OverworldArea.GoLeft(); }
            if (_RightWall) { _OverworldArea.GoRight(); }
            if (_TopWall) { _OverworldArea.GoUp(); }
            if (_BottomWall) { _OverworldArea.GoDown(); }
        }

        [SerializeField] private OverworldArea _OverworldArea;
        [SerializeField] private bool _LeftWall = false;
        [SerializeField] private bool _RightWall = false;
        [SerializeField] private bool _TopWall = false;
        [SerializeField] private bool _BottomWall = false;
    }
}