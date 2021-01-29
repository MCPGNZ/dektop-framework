namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;
    using UnityEngine;

    public class HUDController : MonoBehaviour
    {

        public int Lifepoints
        {
            get => _Lifepoints;
            set => _Lifepoints = value;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [SerializeField] private int _Lifepoints = 3;
    }
}