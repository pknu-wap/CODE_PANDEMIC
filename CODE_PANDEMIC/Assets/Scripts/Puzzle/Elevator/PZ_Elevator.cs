using UnityEngine;

public class PZ_Elevator : MonoBehaviour, IInteractable
{
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform=GetComponent<RectTransform>();

        _rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
    }

    // ���������� ��ȣ �ۿ�
    public void Interact()
    {
        Debug.Log("���������� ��ȣ �ۿ�");
        
        // ���⿡ ���� ������ �Ѿ�� ��� ����
    }
}