using UnityEngine;

public class PZ_Fire_Tutorial : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

        Managers.UI.ShowPopupUI<UI_TutorialPopUp>("PZ_Fire_Tutorial_UI_Prefab");

        Destroy(gameObject);
    }
}