using UnityEngine;

public class PZ_Boss_Enter : MonoBehaviour
{
    [SerializeField] private GameObject _block;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject block = Instantiate(_block);

        block.transform.position = new Vector3(6.16f, -20, 0);

        block.GetComponent<PZ_Hospital_Boss_Block>().StartMove();

        Destroy(gameObject);
    }
}