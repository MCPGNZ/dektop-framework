namespace Mcpgnz.DesktopFramework.Demos
{
    using System.Windows.Forms;
    using UnityRawInput;
    using UnityEngine;

    public class SetWallpaperDemo : MonoBehaviour
    {
        void Awake()
        {
            RawMouseInput.Start(workInBackground: true);
            RawMouseInput.OnMouseRightDown += OpenNewWallpaper;
        }

        void OnDestroy()
        {
            RawMouseInput.Stop();
        }

        void OpenNewWallpaper(MousePosition pos)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Framework.Wallpaper.Set(dialog.FileName, Framework.Wallpaper.Style.Stretched);
            }
        }
    }
}
