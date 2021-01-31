namespace Mcpgnz.Utilities
{
    using System.IO;
    using UnityEngine;

    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        #region Public Variables
        public static T Instance
        {
            get
            {
                if (_Instance != null) { return _Instance; }

                var resources = Resources.LoadAll<T>(string.Empty);
                if (resources.Length == 0) { throw new InvalidDataException(typeof(T).Name + " scriptable object not found"); }
                if (resources.Length > 1) { throw new InvalidDataException("More than one" + typeof(T).Name + " scriptable object found"); }

                _Instance = resources[0];
                return _Instance;
            }
        }
        #endregion Public Variables

        #region Private Variables
        private static T _Instance;
        #endregion Private Variables
    }
}