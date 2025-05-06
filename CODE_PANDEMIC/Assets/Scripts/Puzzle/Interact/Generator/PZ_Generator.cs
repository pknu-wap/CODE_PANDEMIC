using System;
using UnityEngine;
using System.Collections;

public class PZ_Generator : PZ_Puzzle_Base, IInteractable
{
    [SerializeField] private Transform _handleTransform;
    private Animator _animator;

    private bool _isInteracted = false;

    private int _rememberCount = 5;

    public static event Action TurnOnGenerator;

    // 하이라이트 기능
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    public void SetInfo(PuzzleData data)
    {
        _rememberCount = data.RememberCount;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // 발전기 퍼즐 띄우기
    public void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        Managers.UI.ShowPopupUI<PZ_Remember_Board>("PZ_Remember_Board_Prefab", null, (popupPuzzle) =>
        {
            popupPuzzle.Setting(this, _rememberCount);
        });
    }

    private IEnumerator SettingLeverPosition()
    {
        yield return new WaitForSeconds(0.18f);

        _handleTransform.localPosition = new Vector3(-0.06f, 0.919f, 0);

        yield return new WaitForSeconds(0.18f);

        _handleTransform.localPosition = new Vector3(-0.242f, 0.88f, 0);
    }

    public void StartPuzzleClear()
    {
        PuzzleClear();
    }

    protected override void PuzzleClear()
    {
        StartCoroutine(SettingLeverPosition());

        _animator.SetBool("IsInteracted", true);

        _isInteracted = true;

        TurnOnGenerator?.Invoke();
    }

    public void OnHighLight()
    {
        _spriteRenderer.material = _highlightMaterial;
    }

    public void OffHighLight()
    {
        _spriteRenderer.material = _defaultMaterial;
    }

    public bool IsInteractable()
    {
        return _isInteracted;
    }
}