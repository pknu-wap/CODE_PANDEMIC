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

    // ������ ���� ����
    public void Interact()
    {
        if (_isInteracted)
        {
            return;
        }

        Debug.Log("������ ��ȣ �ۿ�");

        StartCoroutine(SettingLeverPosition());

        _animator.SetBool("IsInteracted", true);

        // ���⿡ ������ ���� ���� ���� ���� ����

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