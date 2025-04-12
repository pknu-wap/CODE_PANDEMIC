using UnityEngine;

public class PZ_Interact_Bed : MonoBehaviour, IInteractable
{
    private RectTransform _rectTransform;

    private bool _isInteracted = false;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        _rectTransform.sizeDelta = new Vector3(2, 2, 1);
    }

    // ħ�� ��ȣ �ۿ�
    public void Interact()
    {
        if (_isInteracted)
        {
            return;
        }

        Debug.Log("������ ������ ȹ��");

        // ���⿡ ������ ȹ�� ��� ����

        _isInteracted = true;
    }
}