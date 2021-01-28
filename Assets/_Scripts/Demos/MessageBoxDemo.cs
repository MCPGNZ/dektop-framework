namespace Mcpgnz.DesktopFramework.Demos
{
    using System.Windows.Forms;
    using UnityEngine;
    using UnityRawInput;

    public class MessageBoxDemo : MonoBehaviour
    {
        void Awake()
        {
            RawMouseInput.Start(workInBackground: true);
            RawMouseInput.OnMouseRightDown += SpawnMessageBox;
        }

        void OnDestroy()
        {
            RawMouseInput.Stop();
        }

        void SpawnMessageBox(MousePosition pos)
        {
            switch (MessageBox.Show("tak czy nie?", "kapszon", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk))
            {
                case DialogResult.Yes:
                case DialogResult.No:
                case DialogResult.Cancel:
                    break;
            }
        }
    }
}
