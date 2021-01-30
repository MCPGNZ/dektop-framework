namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using GoogleSheetsToUnity;
    using GoogleSheetsToUnity.Utils;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class LevelParser : MonoBehaviour
    {
        #region Private Variables
        private const string ExplorerId = "E";
        private const string WallId = "#";
        private const string PortalEntryId = "P";
        private const string PortalExitId = "K";
        private const string SpikeEnemyId = "x";
        private const string MineEnemyId = "m";
        #endregion Private Variables

        #region Public Methods
        public Map World => _Map;

        /// <summary>
        /// Starting position of the Explorer
        /// note: this only works if the player is in the 0, 0 stage
        /// </summary>
        public Vector3 Explorer_UnityPosition
        {
            get
            {
                var cell = World.FindUnique(ExplorerId);
                var normalizedPosition = Coordinates.GridToNormalized(cell.GlobalId, Config.StageSize);
                var unityPosition = Coordinates.NormalizedToUnity(normalizedPosition);

                return unityPosition;
            }
        }
        #endregion Public Methods

        #region Public Types
        public enum CellType
        {
            Empty,
            Explorer,
            Wall,
            SpikeEnemy,
            MineEnemy,
            PortalEntry,
            PortalExit,
        }

        [Serializable]
        public sealed class Map
        {
            #region Public Variables
            public Vector2Int Size;
            public Row this[int key]
            {
                get => _Rows[key];
                set => _Rows[key] = value;
            }
            #endregion Public Variables

            #region Public Methods
            public Map(Vector2Int size)
            {
                Size = size;
                _Rows = new Row[size.x];

                for (int x = 0; x < size.x; ++x)
                {
                    _Rows[x] = new Row
                    {
                        Cells = new Cell[size.y]
                    };
                }
            }

            /// <summary>
            /// Fails if there is not exactly one cell with _key_
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Cell FindUnique(string key)
            {
                var found = FindAll(key);
                if (found.Count != 1) { throw new InvalidDataException($"should found only one {key} but found: {found.Count}"); }

                return found[0];
            }

            /// <summary>
            /// Finds all cell with matching _key_
            /// </summary>
            public List<Cell> FindAll(string key)
            {
                var result = new List<Cell>();
                foreach (var row in _Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        if (cell.Data == key)
                        {
                            result.Add(cell);
                        }
                    }
                }
                return result;
            }

            /// <summary>
            /// Extracts Stage from map
            /// </summary>
            public Map Stage(Vector2Int stageId)
            {
                var result = new Map(Config.StageSize);

                var xOffset = Config.StageSize.x * stageId.y;
                var yOffset = Config.StageSize.y * stageId.y;

                for (int x = 0; x < result.Size.x; ++x)
                {
                    for (int y = 0; y < result.Size.y; ++y)
                    {
                        result[x][y] = this[x + xOffset][y + yOffset];
                    }
                }

                return result;
            }
            #endregion Public Methods

            #region Private Variables
            [SerializeField]
            private Row[] _Rows;
            #endregion Private Variables

        }

        [Serializable]
        public sealed class Row
        {
            public Cell this[int key]
            {
                get => Cells[key];
                set => Cells[key] = value;
            }
            public Cell[] Cells;
        }

        [Serializable]
        public sealed class Cell
        {
            private static CellType? ParseCellType(string data)
            {
                if (data.Length == 0) { return CellType.Empty; }
                switch (data)
                {
                    case WallId: return CellType.Wall;
                    case ExplorerId: return CellType.Explorer;
                    case SpikeEnemyId: return CellType.SpikeEnemy;
                    case MineEnemyId: return CellType.MineEnemy;
                    default: break;
                }
                if (data.StartsWith(PortalEntryId)) { return CellType.PortalEntry; }
                if (data.StartsWith(PortalExitId)) { return CellType.PortalExit; }

                return null;
            }

            public Cell(Vector2Int globalId, string data)
            {
                this.GlobalId = globalId;
                this.Data = data;

                var type = ParseCellType(data);
                if (!type.HasValue)
                    throw new ArgumentException($"unsupported cell value: {data} at {globalId}");
                this.Type = type.Value;

                Debug.Log($"{this.Type} at {this.GlobalId}");
            }

            #region Public Variables
            public readonly Vector2Int GlobalId;
            public readonly string Data;
            public readonly CellType Type;
            public Vector2Int StageId
            {
                get
                {
                    return new Vector2Int(GlobalId.x / Config.StageSize.x, GlobalId.y / Config.StageSize.y);
                }
            }
            /// <summary>
            /// position, in tiles, within current stage.
            /// </summary>
            public Vector2Int LocalPositionGrid
            {
                get { return new Vector2Int(GlobalId.x % Config.StageSize.x, GlobalId.y % Config.StageSize.y); }
            }
            public string MatchingPortalExitKey
            {
                get
                {
                    if (Type != CellType.PortalEntry)
                        throw new InvalidOperationException("MatchingExitPortalKey read on non-entry-portal");
                    return PortalExitId + Data.Substring(PortalEntryId.Length);
                }
            }

            #endregion Public Variables
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField] private string _SpreadSheetId = "17PZjawA0xeLcKWSJjHh22cn-BePWG1xDu0cgBKQ-zv4";
        [SerializeField] private string _WorksheetId = "Level";
        [SerializeField] private Map _Map;
        #endregion Inspector Variables

        #region Private Methods
        [Button]
        private void Parse()
        {
            SpreadsheetManager.ReadPublicSpreadsheet(new GSTU_Search(_SpreadSheetId, _WorksheetId, "A1", "ZZ128"), OnSheetLoaded);
        }

        private void OnSheetLoaded(GstuSpreadSheet spreadSheet)
        {
            /* calculate row and column count */
            int rowCount = 0;
            int columnCount = 0;

            foreach (var cell in spreadSheet.Cells)
            {
                var row = cell.Value.Row();
                var column = GoogleSheetsToUnityUtilities.NumberFromExcelColumn(cell.Value.Column());

                Debug.Log(row + " : " + column);

                if (row > rowCount) { rowCount = row; }
                if (column > columnCount) { columnCount = column; }
            }

            if (columnCount % Config.StageSize.x != 0)
            {
                throw new InvalidOperationException($"wrong number of columns in spreadsheet: {columnCount}");
            }
            if (rowCount % Config.StageSize.y != 0)
            {
                throw new InvalidOperationException($"wrong number of rows in spreadsheet: {rowCount}");
            }

            /* create map */
            _Map = new Map(new Vector2Int(columnCount, rowCount));

            /* fill map */
            foreach (var cell in spreadSheet.Cells)
            {
                var row = cell.Value.Row() - 1;
                var column = GoogleSheetsToUnityUtilities.NumberFromExcelColumn(cell.Value.Column()) - 1;

                _Map[column][row] = new Cell(globalId: new Vector2Int(column, row), data: cell.Value.value);
            }
        }
        #endregion Private Methods
    }
}