using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteSpawner : MonoBehaviour
{

    [SerializeField] Vector2 meteoriteVelocity = new Vector2(0, -5);
    [SerializeField] Meteorite meteoritePrefab;

    [Range(0f, 30f)] float meteoriteLifespan = 20f;

    [Range (0f, 20f)]
    [SerializeField] float minPeriod = 3f;

    [Range (0f, 20f)]
    [SerializeField] float maxPeriod = 6f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            // Wait to spawn a meteorite
            float timeToWait = Random.Range(minPeriod, maxPeriod);
            yield return new WaitForSeconds(timeToWait);

            // Spawn a meteorite
            SpawnMeteorite();
        }
    }

    void SpawnMeteorite()
    {
        Meteorite meteorite = Instantiate(meteoritePrefab, transform.position, transform.rotation) as Meteorite;
        meteorite.SetVelocity(meteoriteVelocity);
        meteorite.SetLifespan(meteoriteLifespan);
    }

}
