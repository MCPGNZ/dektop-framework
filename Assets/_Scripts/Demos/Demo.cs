namespace Mcpgnz.DesktopFramework.Demos
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public sealed class Demo : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private Camera _Camera;
        [SerializeField] private MeshFilter _Model;
        #endregion Inspector Variables

        #region Unity Methods
        public void Awake()
        {
            FrameworkEx.Initialize();

            _Vertices = _Model.sharedMesh.vertices.ToList();
            _Vertices = _Vertices.Distinct().ToList();

            for (int i = 0; i < _Vertices.Count; ++i)
            {
                _Representation.Add(DesktopEx.CreateDirectory($"vertex[{i}]"));
            }
        }

        public void Update()
        {
            /* rotate model */
            _Model.transform.Rotate(7.0f * Time.deltaTime,
                13.0f * Time.deltaTime, 21.0f * Time.deltaTime);

            /* update folders */
            var matrix = _Model.transform.localToWorldMatrix;

            for (int i = 0; i < _Vertices.Count; ++i)
            {
                var worldPosition = matrix * _Vertices[i];
                var cameraPosition = _Camera.WorldToScreenPoint(worldPosition);

                _Representation[i].DesktopPosition = new Vector2Int((int)cameraPosition.x,
                    (int)cameraPosition.y);
            }
        }

        public void OnDestroy()
        {
            foreach (var directory in _Representation)
            {
                directory.Delete();
            }

            FrameworkEx.Cleanup();
        }
        #endregion Unity Methods

        #region Private Variables
        private List<Vector3> _Vertices;
        private readonly List<DirectoryEx> _Representation = new List<DirectoryEx>();
        #endregion Private Variables
    }
}