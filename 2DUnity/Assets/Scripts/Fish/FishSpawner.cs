using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> fishPrefabs;
    [SerializeField] private float fishInterval = 2f;
    [SerializeField] private float fishSpawnX = 5f;
    [SerializeField] private float fishSpawnY = -10f;

    private void Start()
    {
        StartCoroutine(FishSpawn());
    }

    public IEnumerator FishSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(fishInterval);

            GameObject randomFish = fishPrefabs[Random.Range(0, fishPrefabs.Count)];

            Vector2 randomPos = new Vector2(transform.position.x + Random.Range(-fishSpawnX, fishSpawnX), transform.position.y + Random.Range(-fishSpawnY, fishSpawnY));

            Instantiate(randomFish, randomPos, randomFish.transform.rotation);
        }
    }
}
