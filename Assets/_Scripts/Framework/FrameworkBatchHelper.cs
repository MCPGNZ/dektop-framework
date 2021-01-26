namespace Mcpgnz.DesktopFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FrameworkBatchHelper : MonoBehaviour
    {
        // Update is called once per frame
        void LateUpdate()
        {
            DesktopEx.Flush();
        }
    }
}
