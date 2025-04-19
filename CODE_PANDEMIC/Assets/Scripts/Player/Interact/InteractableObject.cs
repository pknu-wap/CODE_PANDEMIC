using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Get revolver");
    }

    public void OnHighLight()
    {
        Debug.Log("On Highlight");
    }

    public void OffHighLight()
    {
        Debug.Log("Off Highlight");
    }

    public bool IsInteractable()
    {
        return true;
    }
}