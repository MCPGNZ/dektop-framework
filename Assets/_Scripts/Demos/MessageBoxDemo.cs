namespace Mcpgnz.DesktopFramework.Demos
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using UnityEngine;
    using UnityRawInput;

    public class MessageBoxDemo : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            RawMouseInput.Start(workInBackground: true);
            RawMouseInput.OnMouseRightDown += SpawnMessageBox;
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
