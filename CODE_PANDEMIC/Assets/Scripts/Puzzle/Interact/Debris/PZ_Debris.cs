using UnityEngine;

public class PZ_Debris : PZ_Interact_Base
{
    public override void Interact(GameObject player)
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PZ_FireExtinguisher>())
        {
            Destroy(gameObject);
        }
    }
}