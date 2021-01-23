namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public sealed class DebugComponent : MonoBehaviour
    {
        #region Inspector variables
        [SerializeField] private int _Size = 128;
        [SerializeField] private int _Offset = 256;
        [SerializeField] private int _Speed = 1;
        #endregion Inspector variables

        #region Unity Methods
        private void Awake()
        {
            _Wunsz = _Desktop.Folders.Find(x => x.Name == "Wunsz");
        }
        private void FixedUpdate()
        {
            var direction = new Vector2(Mathf.Cos(_Alpha), Mathf.Sin(_Alpha));
            var vector = direction * _Size;
            var position = vector + new Vector2(_Offset, _Offset);

            _Wunsz.Position = new Vector2Int((int)position.x, (int)position.y);

            _Alpha += Time.deltaTime * _Speed;
        }
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Desktop _Desktop;

        private float _Alpha;
        private Item _Wunsz;
        #endregion Private Variables
    }
}