using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    public GameObject prefab; // The object to spawn
    public float minDistance = 10f; // The minimum distance to spawn from this object
    public float maxDistance = 100f; // The maximum distance to spawn from this object
    public int numToSpawn = 100; // The number of objects to spawn

    void Start()
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            // Generate a random position within the specified range
            Vector3 randomPos = Random.onUnitSphere * Random.Range(minDistance, maxDistance);

            // Generate a random rotation
            Quaternion randomRot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            // Create the object at the random position with the random rotation
            Instantiate(prefab, transform.position + randomPos, randomRot,transform);
        }
    }
}
