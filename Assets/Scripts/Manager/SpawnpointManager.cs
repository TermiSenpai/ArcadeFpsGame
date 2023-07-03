using UnityEngine;

public class SpawnpointManager : MonoBehaviour
{
    public static SpawnpointManager Instance;

    private  Spawnpoint[] spawnpoints;

    private void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }

    public Transform GetRandomSpawnPoint()
    {
        return spawnpoints[Random.Range(0, spawnpoints.Length - 1)].transform;
    }
}
