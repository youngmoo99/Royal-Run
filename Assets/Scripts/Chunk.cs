using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };

    void Start()
    {
        SpawnFence();
    }

    void SpawnFence()
    {   
        List<int> avilableLanes = new List<int> {0, 1, 2};
        int fencesToSpawn = Random.Range(0, lanes.Length);

        for (int i = 0; i < fencesToSpawn; i++)
        {   
            if (avilableLanes.Count <= 0) break;

            int randomLaneIndex = Random.Range(0, avilableLanes.Count);
            int selectedLane = avilableLanes[randomLaneIndex];
            avilableLanes.RemoveAt(randomLaneIndex);

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }
}
