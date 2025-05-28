using UnityEngine;

public class PZ_Puzzle_Tutorial : MonoBehaviour
{
    [SerializeField] private Define.InteractType _popup;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

        Managers.Game.AddInteractCount(_popup);

        Destroy(gameObject);
    }
}