using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PZ_Piano_Tile_Black : UI_Base
{
    // 검은 건반 enum
    private enum PianoNoteBlack
    {
        DoSharp, // 도샾
        ReSharp, // 레샾
        FaSharp, // 파샾
        SolSharp, // 솔샾
        LaSharp, // 라샾
    }

    private RectTransform _rectTransform;
    private Image _image;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteBlack[] _pianoNoteTypes; // 건반 음 종류
    private PianoNoteBlack _pianoTileNote; // 현재 건반 음

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

        // enum 값들을 배열로 가져옴
        _pianoNoteTypes = (PianoNoteBlack[])System.Enum.GetValues(typeof(PianoNoteBlack));

        BindEvent(gameObject, OnButtonClick);
    }

    // 건반 기본 세팅
    public void TileSetup(int index)
    {
        Setting();

        // 중간의 빈 검은 타일
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

    // 건반 클릭 이벤트
    public void OnButtonClick()
    {
        Debug.Log("누른 검은 건반 : " + _pianoTileNote.ToString());

        StartCoroutine(ChangeTileColor());

        _pianoBase.CheckPuzzleClear(_pianoTileNote.ToString());
    }

    private IEnumerator ChangeTileColor()
    {
        _image.material = _pressedMaterial;
        _image.SetMaterialDirty();

        yield return new WaitForSeconds(0.1f);

        _image.material = _normalMaterial;
        _image.SetMaterialDirty();
    }
}