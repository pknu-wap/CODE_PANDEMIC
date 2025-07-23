using System;
using UnityEngine;
using System.Collections;

public class PZ_Generator : PZ_Puzzle_Interact_Side, IInteractable
{
    [SerializeField] private Transform _handleTransform;
    private Animator _animator;

    private bool _isInteracted = false;

    private int _rememberCount = 5;

    public static event Action TurnOnGenerator;
    PZ_Remember_Board _rememberBoard;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _highlightMaterial;

    public override void SetInfo(PuzzleData data)
    {
        base.SetInfo(data);
        _rememberCount = data.RememberCount;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Interact(GameObject player)
    {
        if (_isInteracted)
        {
            return;
        }

        Managers.UI.ShowPopupUI<PZ_Remember_Board>("PZ_Remember_Board_Prefab", null, (popupPuzzle) =>
        {
            _rememberBoard = popupPuzzle;
            popupPuzzle.Setting(this, _rememberCount);
        });
    }

    private IEnumerator SettingLeverPosition()
    {
        yield return CoroutineHelper.WaitForSeconds(0.18f);

        _handleTransform.localPosition = new Vector3(-0.06f, 0.919f, 0);

        yield return CoroutineHelper.WaitForSeconds(0.18f);

        _handleTransform.localPosition = new Vector3(-0.242f, 0.88f, 0);
    }

    public override void CheckPuzzleClear()
    {
        PuzzleClear();
    }

    public override void PuzzleClear()
    {
        StartCoroutine(SettingLeverPosition());

        _animator.SetBool("IsInteracted", true);

        _isInteracted = true;
        Managers.Game.ClearPuzzle(_data.ID);
        Managers.Game.AddClearPuzzleCount();
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