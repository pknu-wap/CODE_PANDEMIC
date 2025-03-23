using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PZ_Piano_Tile_White : MonoBehaviour, IPointerClickHandler
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
    private Outline _outLine;
    private PZ_Piano_Base _pianoBase;
    private PianoNoteWhite[] _pianoNoteTypes; // 건반 음 종류
    private PianoNoteWhite _pianoTileNote; // 현재 건반 음

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _outLine = GetComponent<Outline>();
        _pianoBase = GetComponentInParent<PZ_Piano_Base>();

        // enum 값들을 배열로 가져옴
        _pianoNoteTypes = (PianoNoteWhite[])System.Enum.GetValues(typeof(PianoNoteWhite));
    }

    private void Start()
    {
        // 기본 세팅
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.sizeDelta = new Vector2(150, 500);

        _outLine.effectDistance = new Vector2(3, 3);
    }

    // 건반 기본 세팅
    public void TileSetup(int index)
    {
        int setTileX = -450 + 150 * index;
        _rectTransform.anchoredPosition = new Vector2(setTileX, 0);
        _pianoTileNote = _pianoNoteTypes[index];
    }

    // 건반 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("누른 흰 건반 : " + _pianoTileNote.ToString());

        _pianoBase.IsPuzzleClear(_pianoTileNote.ToString());
    }
}