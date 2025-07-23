using System.Collections.Generic;

public class PZ_Piano_Base : PZ_Puzzle_UI_Main
{
    #region Base

    private List<PZ_Piano_Tile_White> _whiteList = new List<PZ_Piano_Tile_White>();
    private List<PZ_Piano_Tile_Black> _blackList = new List<PZ_Piano_Tile_Black>();

    private List<string> _correctPianoNotes = new List<string>();

    private int _maxPianoCount = 7;
    private int _currentIndex = 0;

    private string _selectedNote;

    public string SelectedNote { get { return _selectedNote; } set { _selectedNote = value; } }

    private void Start()
    {
        SettingCorrectPianoNotes();
        GetSpawnedPianoTiles();

        ReadyToPause();
    }

    private void SettingCorrectPianoNotes()
    {
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("La");
        _correctPianoNotes.Add("La");
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("Sol");
        _correctPianoNotes.Add("Mi");
    }

    #endregion

    #region Setting

    private void GetSpawnedPianoTiles()
    {
        GetComponentsInChildren(false, _whiteList);

        for (int index = 0; index < 7; index++)
        {
            _whiteList[index].TileSetup(index);
        }

        GetComponentsInChildren(false, _blackList);

        for (int index = 0; index < 6; index++)
        {
            _blackList[index].TileSetup(index);
        }
    }

    #endregion

    #region Clear

    public override void CheckPuzzleClear()
    {
        if (_correctPianoNotes[_currentIndex] != _selectedNote)
        {
            _currentIndex = 0;

            return;
        }

        if (_currentIndex == _maxPianoCount - 1 && _correctPianoNotes[_currentIndex] == _selectedNote)
        {
            PuzzleClear();
            _currentIndex = 0;

            return;
        }

        _currentIndex++;
    }

    #endregion
}