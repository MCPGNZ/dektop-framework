﻿namespace Mcpgnz.DesktopFramework
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
        public Cell ExplorerCell => World.FindUnique(ExplorerId);
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
            PortalExit
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
            #region Public Variables
            public Vector2Int GlobalId;
            public string Data;
            public CellType Type;
            public Vector2Int StageId => new Vector2Int(GlobalId.x / Config.StageSize.x, GlobalId.y / Config.StageSize.y);

            /// <summary>
            /// position, in tiles, within current stage.
            /// </summary>
            public Vector2Int LocalPositionGrid => new Vector2Int(GlobalId.x % Config.StageSize.x, GlobalId.y % Config.StageSize.y);

            /// <summary>
            /// position, in unity, within current stage.
            /// </summary>
            public Vector3 LocalUnityPosition
            {
                get
                {
                    var normalized = Coordinates.GridToNormalized(LocalPositionGrid, Config.StageSize);
                    return Coordinates.NormalizedToUnity(normalized);
                }
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

            #region Public Methods
            public Cell(Vector2Int globalId, string data)
            {
                GlobalId = globalId;
                Data = data;

                var type = ParseCellType(data);
                if (!type.HasValue)
                {
                    Debug.LogWarning($"ignoring unsupported cell value: {data} at {globalId}");
                    type = CellType.Empty;
                }
                Type = type.Value;

                Debug.Log($"{Type} at {GlobalId}");
            }
            #endregion Public Methods

            #region Private Methods
            private static CellType? ParseCellType(string data)
            {
                if (data.Length == 0) { return CellType.Empty; }
                switch (data)
                {
                    case WallId: return CellType.Wall;
                    case ExplorerId: return CellType.Explorer;
                    case SpikeEnemyId: return CellType.SpikeEnemy;
                    case MineEnemyId: return CellType.MineEnemy;
                }
                if (data.StartsWith(PortalEntryId)) { return CellType.PortalEntry; }
                if (data.StartsWith(PortalExitId)) { return CellType.PortalExit; }

                return null;
            }
            #endregion Private Methods
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField] private string _SpreadSheetId = "17PZjawA0xeLcKWSJjHh22cn-BePWG1xDu0cgBKQ-zv4";
        [SerializeField] private string _WorksheetId = "Level";

        [SerializeField, HideInInspector] private Map _Map;
        #endregion Inspector Variables

        #region Private Methods
        [Button]
        private void Parse()
        {
            Debug.Log("Parse started");
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

                _Map[column][row] = new Cell(new Vector2Int(column, row), cell.Value.value);
            }

            Debug.Log("Parse success");
        }
        #endregion Private Methods
    }
}