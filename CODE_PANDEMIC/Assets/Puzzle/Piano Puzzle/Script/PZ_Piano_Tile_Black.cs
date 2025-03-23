using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PZ_Piano_Tile_Black : MonoBehaviour, IPointerClickHandler
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
    private PZ_Piano_Base _pianoBase;
    private PianoNoteBlack[] _pianoNoteTypes; // 건반 음 종류
    private PianoNoteBlack _pianoTileNote; // 현재 건반 음

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _pianoBase = GetComponentInParent<PZ_Piano_Base>();

        // enum 값들을 배열로 가져옴
        _pianoNoteTypes = (PianoNoteBlack[])System.Enum.GetValues(typeof(PianoNoteBlack));
    }

    private void Start()
    {
        // 기본 세팅
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);
        _rectTransform.sizeDelta = new Vector2(100, 300);
    }

    // 건반 기본 세팅
    public void TileSetup(int index)
    {
        // 중간의 빈 검은 타일
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

    // 건반 클릭 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("누른 검은 건반 : " + _pianoTileNote.ToString());

        _pianoBase.IsPuzzleClear(_pianoTileNote.ToString());
    }
}