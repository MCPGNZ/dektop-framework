namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// All objects used in the game,
    /// properties (prefab, avatar, etc) defined in the Config asset
    /// </summary>
    public enum Identifier
    {
        Unknown = 0,

        /* characters */
        Explorer = 100,
        Bin = 101,
        Error = 102,
        Clippy = 103,

        Windows31 = 104,
        Windows7 = 105,
        Windows95 = 106,
        Windows98 = 107,
        Windows8 = 108,
        Windows10 = 109,
        Windows2000 = 110,

        /* environment */
        Empty = 200,
        Wall = 201,
        Heart = 202,
        Note = 203,

        /* enemies */
        SpikeEnemy = 300,
        MineEnemy = 301,
        BatEnemy = 302,

        /* portals */
        PortalEntry = 400,
        PortalExit = 401
    }

    public static class ObjectTypes
    {
        /// <summary>
        /// Returns display name
        /// </summary>
        public static string ToName(this Identifier id)
        {
            return Enum.GetName(typeof(Identifier), id);
        }

        /// <summary>
        /// Returns parse tag
        /// </summary>
        public static string ToTag(this Identifier id)
        {
            var found = Config.Instance._Identifiers.Find(x => x.Identifier == id);
            return found?.Tag;
        }

        /// <summary>
        /// Returns identifier enum
        /// </summary>
        public static Identifier ToID(this string tagWithMaybeParams)
        {
            int colonIndex = tagWithMaybeParams.IndexOf(':');
            string tag = colonIndex < 0 ? tagWithMaybeParams : tagWithMaybeParams.Substring(0, colonIndex);

            if (string.IsNullOrWhiteSpace(tag)) { return Identifier.Empty; }

            var identifiers = Config.Instance._Identifiers;
            var found = identifiers.Find(x => x.Tag == tag);
            if (found != null) { return found.Identifier; }

            if (tag.StartsWith(Identifier.MineEnemy.ToTag())) { return Identifier.MineEnemy; }
            if (tag.StartsWith(Identifier.BatEnemy.ToTag())) { return Identifier.BatEnemy; }
            if (tag.StartsWith(Identifier.PortalEntry.ToTag())) { return Identifier.PortalEntry; }
            if (tag.StartsWith(Identifier.PortalExit.ToTag())) { return Identifier.PortalExit; }
            if (tag.StartsWith(Identifier.Note.ToTag())) { return Identifier.Note; }

            Debug.LogWarning($"unrecognized tag: {tag} (full: {tagWithMaybeParams})");

            return Identifier.Unknown;
        }
    }
}