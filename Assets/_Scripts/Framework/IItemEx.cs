namespace Mcpgnz.DesktopFramework
{
    using System;
    using UnityEngine;

    public interface IItemEx
    {
        #region Variables
        event Action<string> OnNameChanged;
        event Action<Vector2Int> OnPositionChanged;

        string Name { get; set; }
        string Path { get; }
        string Directory { get; }

        Vector2Int Position { get; set; }
        #endregion Variables
    }
}