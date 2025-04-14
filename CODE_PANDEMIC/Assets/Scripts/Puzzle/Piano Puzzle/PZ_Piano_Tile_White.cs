using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PZ_Piano_Tile_White : UI_Base
{
    // �� �ǹ� enum
    private enum PianoNoteWhite
    {
        Do, // ��
        Re, // ��
        Mi, // ��
        Fa, // ��
        Sol, // ��
        La, // ��
        Ti // ��
    }

    private RectTransform _rectTransform;
    private Image _image;
    private Outline _outLine;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteWhite[] _pianoNoteTypes; // �ǹ� �� ����
    private PianoNoteWhite _pianoTileNote; // ���� �ǹ� ��

    private Material _normalMaterial;
    private Material _pressedMaterial;

    private void Setting()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _outLine = GetComponent<Outline>();
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

        // �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.sizeDelta = new Vector2(150, 500);

        _outLine.effectDistance = new Vector2(3, 3);

        // enum ������ �迭�� ������
        _pianoNoteTypes = (PianoNoteWhite[])System.Enum.GetValues(typeof(PianoNoteWhite));

        BindEvent(gameObject, OnButtonClick);
    }

    // �ǹ� �⺻ ����
    public void TileSetup(int index)
    {
        Setting();

        int setTileX = -450 + 150 * index;
        _rectTransform.anchoredPosition = new Vector2(setTileX, 0);
        _pianoTileNote = _pianoNoteTypes[index];
    }

    // �ǹ� Ŭ�� �̺�Ʈ
    public void OnButtonClick()
    {
        Debug.Log("���� �� �ǹ� : " + _pianoTileNote.ToString());

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