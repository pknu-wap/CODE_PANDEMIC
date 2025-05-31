using UnityEngine;

public class PZ_Boss_Enter : MonoBehaviour
{
    [SerializeField] private GameObject _block;

    private void Start()
    {
        if (Managers.Game.IsClearStage()) Destroy(gameObject);  
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
  
        GameObject block = Instantiate(_block);
        block.transform.SetParent(transform.parent, worldPositionStays: true);
        block.transform.position = new Vector3(6.16f, -20, 0);

        block.GetComponent<PZ_Hospital_Boss_Block>().StartMove();

        Destroy(gameObject);
    }
}