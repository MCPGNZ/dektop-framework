namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

    public class Grid : MonoBehaviour
    {

        [SerializeField] private Color _Color = Color.gray;

        public void OnDrawGizmos()
        {
            var increment = new Vector2(Config.UnitySize.x, Config.UnitySize.y) / new Vector2(Config.StageSize.x, Config.StageSize.y);

            Gizmos.color = _Color;
            for (int x = 0; x < Config.StageSize.x + 1; ++x)
            {
                Gizmos.DrawLine(
                    new Vector3(x * increment.x, 0.0f, 0.0f),
                    new Vector3(x * increment.x, -Config.UnitySize.y, 0.0f)
                    );
            }

            for (int y = 0; y < Config.StageSize.y + 1; ++y)
            {
                Gizmos.DrawLine(
                    new Vector3(0, -y * increment.y, 0.0f),
                    new Vector3(Config.UnitySize.x, -y * increment.y, 0.0f)
                );
            }
        }
    }
}