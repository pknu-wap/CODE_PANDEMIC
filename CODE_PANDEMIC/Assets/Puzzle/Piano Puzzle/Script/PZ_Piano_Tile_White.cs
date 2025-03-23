using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PZ_Piano_Tile_White : MonoBehaviour, IPointerClickHandler
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
    private Outline _outLine;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteWhite[] _pianoNoteTypes; // �ǹ� �� ����
    private PianoNoteWhite _pianoTileNote; // ���� �ǹ� ��

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _outLine = GetComponent<Outline>();
        _pianoBase = GetComponentInParent<PZ_Piano_Base>();

        // enum ������ �迭�� ������
        _pianoNoteTypes = (PianoNoteWhite[])System.Enum.GetValues(typeof(PianoNoteWhite));
    }

    private void Start()
    {
        // �⺻ ����
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.sizeDelta = new Vector2(150, 500);

        _outLine.effectDistance = new Vector2(3, 3);
    }

    // �ǹ� �⺻ ����
    public void TileSetup(int index)
    {
        int setTileX = -450 + 150 * index;
        _rectTransform.anchoredPosition = new Vector2(setTileX, 0);
        _pianoTileNote = _pianoNoteTypes[index];
    }

    // �ǹ� Ŭ�� �̺�Ʈ
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("���� �� �ǹ� : " + _pianoTileNote.ToString());

        _pianoBase.IsPuzzleClear(_pianoTileNote.ToString());
    }
}