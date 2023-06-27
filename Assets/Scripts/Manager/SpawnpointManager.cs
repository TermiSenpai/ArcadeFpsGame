using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointManager : MonoBehaviour
{
    public static SpawnpointManager Instance;

    public Transform[] spawnpoints;

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetRandomSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length - 1)];
    }
}
