using System.Collections;
using UnityEngine;

public class PZ_Generator : MonoBehaviour, IInteractable
{
    private Transform _handleTransform;
    private Animator _animator;

    private bool _isInteracted = false;

    private void Start()
    {
        _handleTransform = transform.Find("PZ_Generator_Handle");
        _animator = GetComponentInChildren<Animator>();
    }

    // 발전기 퍼즐 띄우기
    public void Interact()
    {
        if (_isInteracted)
        {
            return;
        }

        Debug.Log("발전기 상호 작용");

        StartCoroutine(SettingLeverPosition());

        _animator.SetBool("IsInteracted", true);

        // 여기에 발전기 퍼즐 띄우는 로직 구현 예정

        _isInteracted = true;
    }

    private IEnumerator SettingLeverPosition()
    {
        yield return new WaitForSeconds(0.18f);

        _handleTransform.localPosition = new Vector3(-0.06f, 0.919f, 0);

        yield return new WaitForSeconds(0.18f);

        _handleTransform.localPosition = new Vector3(-0.242f, 0.88f, 0);
    }
}