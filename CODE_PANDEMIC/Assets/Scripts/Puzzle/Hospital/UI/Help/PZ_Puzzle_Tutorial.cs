using UnityEngine;

public class PZ_Puzzle_Tutorial : MonoBehaviour
{
    [SerializeField] private string _popupAddressable;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

       // Managers.UI.ShowPopupUI<UI_TutorialPopUp>(_popupAddressable);

        Destroy(gameObject);
    }
}