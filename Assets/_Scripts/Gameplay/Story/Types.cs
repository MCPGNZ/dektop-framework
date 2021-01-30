namespace Mcpgnz.DesktopFramework
{
    using System.Collections.Generic;

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
        public static Identifier Find(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) { return Identifier.Empty; }

            foreach (var entry in Tags)
            {
                if (entry.Value == tag)
                {
                    return entry.Key;
                }
            }
            if (tag.StartsWith(Tags[Identifier.PortalEntry])) { return Identifier.PortalEntry; }
            if (tag.StartsWith(Tags[Identifier.PortalExit])) { return Identifier.PortalExit; }

            return Identifier.Unknown;
        }

        public static readonly Dictionary<Identifier, string> Tags =
            new Dictionary<Identifier, string>
            {
                {Identifier.Explorer, "E"},
                {Identifier.Empty, " "},
                {Identifier.Wall, "#"},
                {Identifier.SpikeEnemy, "x"},
                {Identifier.MineEnemy, "m"},
                {Identifier.PortalEntry, "P"},
                {Identifier.PortalExit, "K"}
            };
    }
}