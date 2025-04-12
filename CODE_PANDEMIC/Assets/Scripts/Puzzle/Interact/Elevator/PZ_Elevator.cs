using UnityEngine;

public class PZ_Elevator : MonoBehaviour, IInteractable
{
    private RectTransform _rectTransform;
    private Animator _animator;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();

        _rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    // ���������� ��ȣ �ۿ�
    public void Interact()
    {
        Debug.Log("���� ������ �̵�");

        _animator.SetBool("IsOpened", true);

        // ���⿡ ���� ������ �Ѿ�� ��� ����
    }
}