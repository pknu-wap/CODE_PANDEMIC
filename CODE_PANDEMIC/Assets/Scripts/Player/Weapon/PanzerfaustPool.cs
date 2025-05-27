using System.Collections.Generic;
using UnityEngine;

public class PanzerfaustPool : MonoBehaviour
{
    public static PanzerfaustPool Instance;

    [SerializeField] private GameObject PanzerfaustProjectilePrefab;
    [SerializeField] private int poolSize = 5;

    private Queue<GameObject> panzerfaustPool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject rocket = Instantiate(PanzerfaustProjectilePrefab);
            rocket.SetActive(false);
            panzerfaustPool.Enqueue(rocket);
        }
    }

    public GameObject GetRocket()
    {
        if (panzerfaustPool.Count > 0)
        {
            GameObject rocket = panzerfaustPool.Dequeue();
            rocket.SetActive(true);
            return rocket;
        }
        else
        {
            GameObject bullet = Instantiate(PanzerfaustProjectilePrefab);
            return bullet;
        }
    }

    public void ReturnRocket(GameObject rocket)
    {
        rocket.SetActive(false);
        panzerfaustPool.Enqueue(rocket);
    }
}
