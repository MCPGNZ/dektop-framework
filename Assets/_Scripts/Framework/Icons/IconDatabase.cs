namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using Mcpgnz.Utilities;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CreateAssetMenu(menuName = "Framework/Icon Database")]
    public class IconDatabase : ScriptableObjectSingleton<IconDatabase>
    {
        #region Public Types
        [Serializable]
        public sealed class Entry
        {
            public string Name;
            public Object Asset;
        }
        #endregion Public Types

        #region Public Methods
        public static Object Get(string iconName)
        {
            var found = Instance._Entries.Find(x => x.Name == iconName);
            return found?.Asset;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private List<Entry> _Entries;
        #endregion Inspector Variables
    }
}