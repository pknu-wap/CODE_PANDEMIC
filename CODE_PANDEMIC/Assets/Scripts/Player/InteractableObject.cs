using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("리볼버를 획득했습니다.");
    }
}
