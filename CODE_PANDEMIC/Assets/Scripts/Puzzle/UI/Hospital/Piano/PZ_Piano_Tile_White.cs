using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PZ_Piano_Tile_White : UI_Base
{
    private enum PianoNoteWhite
    {
        Do,
        Re,
        Mi,
        Fa,
        Sol,
        La,
        Ti
    }

    private RectTransform _rectTransform;
    private Image _image;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteWhite[] _pianoNoteTypes;
    private PianoNoteWhite _pianoTileNote;

    private Material _normalMaterial;
    private Material _pressedMaterial;

    private void Setting()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _pianoBase = GetComponentInParent<PZ_Piano_Base>();

        Managers.Resource.LoadAsync<Material>("PZ_Piano_Tile_White_Normal_Material", (getMaterial) =>
        {
            _normalMaterial = getMaterial;

            _image.material = _normalMaterial;
            _image.SetMaterialDirty();
        });

        Managers.Resource.LoadAsync<Material>("PZ_Piano_Tile_White_Pressed_Material", (getMaterial) =>
        {
            _pressedMaterial = getMaterial;
        });

        _pianoNoteTypes = (PianoNoteWhite[])System.Enum.GetValues(typeof(PianoNoteWhite));

        BindEvent(gameObject, OnButtonClick);
    }

    public void TileSetup(int index)
    {
        Setting();

        int setTileX = -450 + 150 * index;
        _rectTransform.anchoredPosition = new Vector2(setTileX, 0);
        _pianoTileNote = _pianoNoteTypes[index];
    }

    public void OnButtonClick()
    {
        StartCoroutine(ChangeTileColor());

        _pianoBase.SelectedNote = _pianoTileNote.ToString();
        _pianoBase.CheckPuzzleClear();
    }

    private IEnumerator ChangeTileColor()
    {
        _image.material = _pressedMaterial;
        _image.SetMaterialDirty();

        yield return CoroutineHelper.WaitForSecondsRealtime(0.1f);

        _image.material = _normalMaterial;
        _image.SetMaterialDirty();
    }
}