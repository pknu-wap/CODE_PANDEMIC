﻿using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerController _playerController;
    private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    private RaycastHit2D _prevHit;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        ObjectHighLight();

        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Vector2 direction = _playerController.moveInput;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, interactionRange, interactableLayer);

        if (hit.collider)
        {
            Debug.Log("interact: " + hit.collider.name);
            _prevHit = hit;
            hit.collider.GetComponent<IInteractable>()?.Interact();
        }
    }

    private void ObjectHighLight()
    {
        Vector2 direction = _playerController.moveInput;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, interactionRange, interactableLayer);

        if (hit.collider && !hit.collider.GetComponent<IInteractable>().IsInteractable())
        {
            _prevHit = hit;
            hit.collider.GetComponent<IInteractable>()?.OnHighLight();
        }
        else
        {
            if (_prevHit)
            {
                _prevHit.collider.GetComponent<IInteractable>()?.OffHighLight();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * interactionRange);
    }
}