namespace Mcpgnz.DesktopFramework.Demos
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DesktopWatcherDemo : MonoBehaviour
    {
        void Start()
        {
            DesktopEx.OnItemCreated += (string name) => Debug.Log($"created: {name}");
            DesktopEx.OnItemDeleted += (string name) => Debug.Log($"deleted: {name}");
            DesktopEx.OnItemChanged += (string name) => Debug.Log($"changed: {name}");
        }
    }
}
