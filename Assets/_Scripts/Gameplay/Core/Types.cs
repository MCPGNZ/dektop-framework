namespace Mcpgnz.DesktopFramework
{
    using System;

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
        Windows7 = 104,

        /* environment */
        Empty = 200,
        Wall = 201,

        /* enemies */
        SpikeEnemy = 300,
        MineEnemy = 301,

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
        public static Identifier ToID(this string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) { return Identifier.Empty; }

            var identifiers = Config.Instance._Identifiers;
            var found = identifiers.Find(x => x.Tag == tag);
            if (found != null) { return found.Identifier; }

            if (tag.StartsWith(Identifier.PortalEntry.ToTag())) { return Identifier.PortalEntry; }
            if (tag.StartsWith(Identifier.PortalExit.ToTag())) { return Identifier.PortalExit; }

            return Identifier.Unknown;
        }
    }
}