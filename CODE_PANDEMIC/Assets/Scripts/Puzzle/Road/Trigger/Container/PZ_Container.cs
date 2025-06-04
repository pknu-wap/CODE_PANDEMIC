using UnityEngine;
using System.Collections;

public class PZ_Container : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private int _zombieCount = 3;

    private bool _isTriggered = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInParent<PlayerController>() || _isTriggered)
        {
            return;
        }

        _animator.SetBool("IsOpened", true);

        _isTriggered = true;

        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 spawnPos = _spawnTransform.position;
        spawnPos.y -= 1f;
        if (Managers.Data.Monsters.TryGetValue(5, out MonsterData data))
        {
            while (_zombieCount-- > 0)
            {
                int randomForce = Random.Range(0, 4);
                Vector2 forceVec = new Vector2(0, -randomForce);


                Managers.Resource.Instantiate(data.Prefab, gameObject.transform, (zombie) =>
                {
                    zombie.GetComponent<AI_Base>().SetInfo(data);

                    Transform zombieTransform = zombie.GetComponent<Transform>();
                    zombieTransform.localPosition = spawnPos;
                });

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}