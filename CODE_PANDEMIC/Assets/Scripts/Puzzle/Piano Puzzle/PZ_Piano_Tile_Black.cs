using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PZ_Piano_Tile_Black : UI_Base
{
    // ���� �ǹ� enum
    private enum PianoNoteBlack
    {
        DoSharp, // ����
        ReSharp, // ����
        FaSharp, // �Ę�
        SolSharp, // �֘�
        LaSharp, // ���
    }

    private RectTransform _rectTransform;
    private Image _image;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteBlack[] _pianoNoteTypes; // �ǹ� �� ����
    private PianoNoteBlack _pianoTileNote; // ���� �ǹ� ��

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

        // �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.sizeDelta = new Vector2(100, 300);

        // enum ������ �迭�� ������
        _pianoNoteTypes = (PianoNoteBlack[])System.Enum.GetValues(typeof(PianoNoteBlack));

        BindEvent(gameObject, OnButtonClick);
    }

    // �ǹ� �⺻ ����
    public void TileSetup(int index)
    {
        Setting();

        // �߰��� �� ���� Ÿ��
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

    // �ǹ� Ŭ�� �̺�Ʈ
    public void OnButtonClick()
    {
        Debug.Log("���� ���� �ǹ� : " + _pianoTileNote.ToString());

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