using UnityEngine;

public class PZ_Boss_Enter : MonoBehaviour
{
    [SerializeField] private GameObject _block;
    private bool _entering;
    private void Start()
    {
        if (Managers.Game.IsClearStage()) Destroy(gameObject);
        _entering = false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_entering) return;
        _entering = true;
        GameObject block = Instantiate(_block);
        block.transform.SetParent(transform.parent, worldPositionStays: true);
        block.transform.position = new Vector3(6.16f, -20, 0);

        block.GetComponent<PZ_Hospital_Boss_Block>().StartMove();

        Destroy(gameObject);
    }
}