using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PZ_Piano_Tile_Black : UI_Base
{
    private enum PianoNoteBlack
    {
        DoSharp,
        ReSharp,
        FaSharp,
        SolSharp,
        LaSharp,
    }

    private RectTransform _rectTransform;
    private Image _image;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteBlack[] _pianoNoteTypes;
    private PianoNoteBlack _pianoTileNote;

    private Material _normalMaterial;
    private Material _pressedMaterial;

    private void Setting()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _pianoBase = GetComponentInParent<PZ_Piano_Base>();

        Managers.Resource.LoadAsync<Material>("PZ_Piano_Tile_Black_Normal_Material", (getMaterial) =>
        {
            _normalMaterial = getMaterial;

            _image.material = _normalMaterial;
            _image.SetMaterialDirty();
        });

        Managers.Resource.LoadAsync<Material>("PZ_Piano_Tile_Black_Pressed_Material", (getMaterial) =>
        {
            _pressedMaterial = getMaterial;
        });

        _pianoNoteTypes = (PianoNoteBlack[])System.Enum.GetValues(typeof(PianoNoteBlack));

        BindEvent(gameObject, OnButtonClick);
    }

    public void TileSetup(int index)
    {
        Setting();

        if (index == 2)
        {
            _image.enabled = false;
            return;
        }

        int setTileX = -375 + 150 * index;

        _rectTransform.anchoredPosition = new Vector2(setTileX, 100);

        if (index > 2)
        {
            _pianoTileNote = _pianoNoteTypes[index - 1];

        }
        else
        {
            _pianoTileNote = _pianoNoteTypes[index];
        }
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