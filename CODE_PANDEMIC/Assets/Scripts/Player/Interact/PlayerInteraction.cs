using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    void Interact()
    {
        Vector2 direction = transform.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, interactionRange, interactableLayer);

        if (hit.collider != null)
        {
            Debug.Log("interact: " + hit.collider.name);
            hit.collider.GetComponent<IInteractable>()?.Interact();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * interactionRange);
    }
}
