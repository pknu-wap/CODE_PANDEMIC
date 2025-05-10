using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PZ_Piano_Tile_White : UI_Base
{
    // 흰 건반 enum
    private enum PianoNoteWhite
    {
        Do, // 도
        Re, // 레
        Mi, // 미
        Fa, // 파
        Sol, // 솔
        La, // 라
        Ti // 시
    }

    private RectTransform _rectTransform;
    private Image _image;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteWhite[] _pianoNoteTypes; // 건반 음 종류
    private PianoNoteWhite _pianoTileNote; // 현재 건반 음

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

        // enum 값들을 배열로 가져옴
        _pianoNoteTypes = (PianoNoteWhite[])System.Enum.GetValues(typeof(PianoNoteWhite));

        BindEvent(gameObject, OnButtonClick);
    }

    // 건반 기본 세팅
    public void TileSetup(int index)
    {
        Setting();

        int setTileX = -450 + 150 * index;
        _rectTransform.anchoredPosition = new Vector2(setTileX, 0);
        _pianoTileNote = _pianoNoteTypes[index];
    }

    // 건반 클릭 이벤트
    public void OnButtonClick()
    {
        Debug.Log("누른 흰 건반 : " + _pianoTileNote.ToString());

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