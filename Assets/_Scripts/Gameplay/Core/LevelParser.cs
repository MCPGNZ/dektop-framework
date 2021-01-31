namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using GoogleSheetsToUnity;
    using GoogleSheetsToUnity.Utils;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class LevelParser : MonoBehaviour
    {
        #region Public Methods
        public Map World => _Map;

        public Note GetNoteById(string id)
        {
            Debug.Log($"got {_Notes.Count} notes");
            Debug.Log($"notes = {string.Join("\n", _Notes.Select(n => n.ToString()))}");
            var matchingNotes = _Notes.Where(n => n.Id == id).ToList();
            if (matchingNotes.Count == 0)
                throw new ArgumentException($"no note for id = {id}");
            if (matchingNotes.Count > 1)
                throw new ArgumentException($"multiple notes for id = {id}. This isn't supposed to happen, OnClippyNotesSheetLoaded is fucked up.");
            return matchingNotes[0];
        }
        #endregion Public Methods

        #region Public Types
        [Serializable]
        public sealed class Note
        {
            public string Id;
            public string Icon;
            public string Message;

            public override string ToString()
            {
                return $"id = {Id}; icon = {Icon}; mesage = {Message}";
            }
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
                    for (int y = 0; y < size.y; ++y)
                        _Rows[x][y] = new Cell(new Vector2Int(x, y), "");
                }
            }

            /// <summary>
            /// Fails if there is not exactly one cell with _key_
            /// </summary>
            public Cell FindUnique(string key)
            {
                var found = FindAll(key);
                if (found.Count != 1) { throw new InvalidDataException($"should found only one {key} but found: {found.Count}"); }

                return found[0];
            }

            /// <summary>
            /// Fails if there is not exactly one cell with _key_
            /// </summary>
            public Cell FindUnique(Identifier identifier)
            {
                var found = FindAll(identifier);
                if (found.Count != 1) { throw new InvalidDataException($"should found only one {identifier.ToName()} but found: {found.Count}"); }

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
            /// Finds all cell with matching _key_
            /// </summary>
            public List<Cell> FindAll(Identifier key)
            {
                var result = new List<Cell>();
                foreach (var row in _Rows)
                {
                    foreach (var cell in row.Cells)
                    {
                        if (cell.Type == key)
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

                var xOffset = Config.StageSize.x * stageId.x;
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
            public Identifier Type;

            public bool Discarded;

            public bool HasParameters { get { return Data.Contains(':'); } }
            public string Parameters
            {
                get
                {
                    if (!HasParameters)
                        return "";
                    return Data.Substring(Data.IndexOf(':') + 1);
                }
            }

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
                    if (Type != Identifier.PortalEntry)
                        throw new InvalidOperationException("MatchingExitPortalKey read on non-entry-portal");

                    return Identifier.PortalExit.ToTag() + ":" + Parameters;
                }
            }
            #endregion Public Variables

            #region Public Methods
            public Cell(Vector2Int globalId, string data)
            {
                GlobalId = globalId;
                Data = data;

                Type = data.ToID();
                if (Type == Identifier.Unknown)
                {
                    Debug.LogWarning($"ignoring unsupported cell value: {data} at {globalId}");
                    Type = Identifier.Empty;
                }
                Debug.Log($"{Type} at {GlobalId}");
            }
            #endregion Public Methods
        }
        #endregion Public Types

        #region Inspector Variables
        [SerializeField] private string _SpreadSheetId = "17PZjawA0xeLcKWSJjHh22cn-BePWG1xDu0cgBKQ-zv4";
        [SerializeField] private string _MapWorksheetId = "Level";
        [SerializeField] private string _NotesWorksheetId = "Notes";

        [SerializeField, HideInInspector] private Map _Map;
        [SerializeField] private List<Note> _Notes;
        #endregion Inspector Variables

        #region Private Methods
        [Button]
        private void Parse()
        {
            Debug.Log("Parse started");
            SpreadsheetManager.ReadPublicSpreadsheet(new GSTU_Search(_SpreadSheetId, _MapWorksheetId, "A1", "ZZ128"), OnMapSheetLoaded);
            SpreadsheetManager.ReadPublicSpreadsheet(new GSTU_Search(_SpreadSheetId, _NotesWorksheetId, "A1", "Z128"), OnClippyNotesSheetLoaded);
        }

        private void OnMapSheetLoaded(GstuSpreadSheet spreadSheet)
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

                var c = new Cell(new Vector2Int(column, row), cell.Value.value);
                if (c.Type == Identifier.Unknown)
                {
                    Debug.Log($"WAT {c.GlobalId} = {c.Data} / {c.Type}");
                }
                _Map[column][row] = c;
            }

            Debug.Log("Map parse success");
        }

        private void OnClippyNotesSheetLoaded(GstuSpreadSheet spreadSheet)
        {
            _Notes = new List<Note>();

            try
            {
                for (int rowIdx = 1; ; ++rowIdx)
                {
                    string id = spreadSheet.Cells["A" + rowIdx].value;
                    string icon = spreadSheet.Cells["B" + rowIdx].value;
                    string message = spreadSheet.Cells["C" + rowIdx].value;

                    if (string.IsNullOrWhiteSpace(id)) break;
                    if (_Notes.Where(n => n.Id == id).Any())
                    {
                        Debug.LogError($"Duplicate note id: {id} at row {rowIdx}, overwriting");
                    }

                    _Notes.Add(new Note { Id = id, Icon = icon, Message = message });
                    Debug.Log($"Loaded note: {id} = {message} (icon = {icon})");
                }
            }
            catch (KeyNotFoundException)
            {
            }

            Debug.Log($"Notes parse success, {_Notes.Count} loaded");
        }
        #endregion Private Methods
    }
}