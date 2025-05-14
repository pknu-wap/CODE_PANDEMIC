using System.Collections;
using UnityEngine;

public class PZ_Container : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private int _zombieCount = 3;

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

        Vector2 forceVec = new Vector2(0, -3);

        while (_zombieCount-- > 0)
        {
            GameObject zombie = Instantiate(_zombiePrefab, spawnPos, Quaternion.identity);
            Rigidbody2D rigidbody2D = zombie.GetComponent<Rigidbody2D>();

            yield return new WaitForSeconds(0.1f);

            rigidbody2D.AddForce(forceVec, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);
        }

        Destroy(gameObject);
    }
}