using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float interactionRange = 3f;
    private LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, interactionRange, interactableLayer);

        if (hit.collider != null)
        {
            Debug.Log("상호작용: " + hit.collider.name);
            hit.collider.GetComponent<IInteractable>()?.Interact();
        }
    }
}
