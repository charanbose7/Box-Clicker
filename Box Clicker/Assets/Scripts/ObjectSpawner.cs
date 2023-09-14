using UnityEngine;
using System.Collections;
using Photon.Pun;

public class ObjectSpawner : MonoBehaviourPunCallbacks
{
    public GameObject objectToSpawn; // The object prefab to spawn
    public int numberOfObjectsToSpawn = 10;
    public Vector3 spawnBoundaryCenter; // Center of the spawn boundary
    public Vector3 spawnBoundarySize; // Size of the spawn boundary

    private bool canSpawn = false;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Only the master client will start the countdown and spawn objects.
            CountdownTimer countdownTimer = GetComponent<CountdownTimer>();
            countdownTimer.OnCountdownComplete += StartSpawning;
            countdownTimer.StartCountdown();
        }
    }

    private void StartSpawning()
    {
        canSpawn = true;
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnObjects());
        }
    }

    IEnumerator SpawnObjects()
    {
        while (canSpawn)
        {
            Vector3 randomPosition = GetRandomSpawnPosition();
            PhotonNetwork.Instantiate(objectToSpawn.name, randomPosition, Quaternion.identity);
            numberOfObjectsToSpawn--;

            if (numberOfObjectsToSpawn <= 0)
            {
                canSpawn = false;
            }

            yield return new WaitForSeconds(0.1f); // Optional delay between spawns
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnBoundaryCenter.x - spawnBoundarySize.x / 2, spawnBoundaryCenter.x + spawnBoundarySize.x / 2),
            Random.Range(spawnBoundaryCenter.y - spawnBoundarySize.y / 2, spawnBoundaryCenter.y + spawnBoundarySize.y / 2),
            Random.Range(spawnBoundaryCenter.z - spawnBoundarySize.z / 2, spawnBoundaryCenter.z + spawnBoundarySize.z / 2)
        );

        return randomPosition;
    }
}
