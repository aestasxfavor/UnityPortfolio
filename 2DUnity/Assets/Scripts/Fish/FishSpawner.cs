using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> fishPrefabs;      // 이건 나중에 지역특색 고려해서 할거라 ex) 남해: 멸치위주, 동해: 오징어 위주, 노르웨이해역: 연어위주 등

    [SerializeField] private SpawnerConfigSO spawnerConfig;

    private readonly List<GameObject> aliveFish = new();

    private void Start()
    {
        StartCoroutine(FishSpawn());
    }

    public IEnumerator FishSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnerConfig.fishInterval);

            for(int i = aliveFish.Count - 1; i >= 0; i--)
            {
                if(aliveFish[i] == null)
                    aliveFish.RemoveAt(i);
            }

            if (aliveFish.Count >= spawnerConfig.maxFishSpawn)
                continue;

            GameObject randomFish = fishPrefabs[Random.Range(0, fishPrefabs.Count)];

            Vector2 randomPos = new Vector2(transform.position.x + Random.Range(-spawnerConfig.fishSpawnX, spawnerConfig.fishSpawnX), transform.position.y + Random.Range(-spawnerConfig.fishSpawnY, spawnerConfig.fishSpawnY));

            Instantiate(randomFish, randomPos, randomFish.transform.rotation);
        }
    }
}
