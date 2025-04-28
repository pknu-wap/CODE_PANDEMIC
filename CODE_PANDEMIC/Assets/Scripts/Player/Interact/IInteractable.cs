using UnityEngine;

public interface IInteractable
{
    void Interact(GameObject player);

    void OnHighLight();

    void OffHighLight();

    bool IsInteractable();
}