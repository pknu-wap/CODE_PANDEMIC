using System.Collections;
using UnityEngine;

public class PZ_Container : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private int _zombieCount = 3;

    [SerializeField] private MonsterData _monsterData;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>())
        {
            return;
        }

        _animator.SetBool("IsOpened", true);

        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 spawnPos = _spawnTransform.position;
        spawnPos.y -= 1f;

        while (_zombieCount-- > 0)
        {
            int randomForce = Random.Range(0, 4);
            Vector2 forceVec = new Vector2(0, -randomForce);

            GameObject zombie = Instantiate(_zombiePrefab, spawnPos, Quaternion.identity);
            zombie.GetComponent<AI_Base>().SetInfo(_monsterData);

            Rigidbody2D rigidbody2D = zombie.GetComponent<Rigidbody2D>();

            yield return new WaitForSeconds(0.1f);

            rigidbody2D.AddForce(forceVec, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);
        }

        Destroy(gameObject);
    }
}