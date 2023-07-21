using UnityEngine;

public class SpawnpointManager : MonoBehaviour
{
    public static SpawnpointManager Instance;

    Transform lastPos;
    private  Spawnpoint[] spawnpoints;

    private void Awake()
    {
        Instance = this;
        spawnpoints = GetComponentsInChildren<Spawnpoint>();
    }

    public Transform GetRandomSpawnPoint()
    {
        Transform pos = spawnpoints[Random.Range(0, spawnpoints.Length - 1)].transform;
        if (pos == lastPos) GetRandomSpawnPoint();
        lastPos = pos;
        return pos;
    }
}
