using UnityEngine;
using UnityEngine.UI;

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
    private PZ_Piano_Base _pianoBase;
    private PianoNoteBlack[] _pianoNoteTypes; // �ǹ� �� ����
    private PianoNoteBlack _pianoTileNote; // ���� �ǹ� ��

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _pianoBase = GetComponentInParent<PZ_Piano_Base>();

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
        // �߰��� �� ���� Ÿ��
        if (index == 2)
        {
            GetComponent<Image>().enabled = false;
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

        _pianoBase.CheckPuzzleClear(_pianoTileNote.ToString());
    }
}