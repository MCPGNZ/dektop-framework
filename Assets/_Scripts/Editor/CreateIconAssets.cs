namespace Mcpgnz.DesktopFramework.Editor
{
    using System;
    using System.IO;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEngine;

    public static class CreateIconAssets
    {
        #region Public Methods
        [InfoBox("Select desired icons in Project window")]
        [Button]
        public static void Create()
        {
            var icons = Selection.objects;
            if (icons.Length == 0) { throw new InvalidOperationException("no icons selected"); }

            foreach (var icon in icons)
            {
                var path = AssetDatabase.GetAssetPath(icon);
                if (Path.GetExtension(path) != ".ico") { continue; }

                var asset = ScriptableObject.CreateInstance<IconEx>();
                asset.name = Path.GetFileNameWithoutExtension(path) + " [icon]";
                asset._Asset = icon;

                var savePath = Path.Combine("Assets/Resources/Icons", asset.name + ".asset");

                if (File.Exists(
                    Path.Combine(
                        Application.dataPath, "Resources/Icons", asset.name + ".asset")) == false)
                {
                    AssetDatabase.CreateAsset(asset, savePath);
                    Debug.Log($"created: {asset.name}");
                }
                else
                {
                    Debug.Log($"skipped: {asset.name}");
                }

            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion Public Methods
    }
}