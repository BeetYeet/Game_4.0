using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public int spawnCount = 999999;
    private int spawnedCount = 0;
    public float spawnRate = 1f;

    [Range(0f, 1f)]
    public float spawnRateVariance = 0f;

    public float timeSinceLast = 0f;

    public GameObject prefab;

    public bool IsDone()
    {
        if (spawnCount > 0)
            return false;
        return true;
    }

    private void Update()
    {
        if (spawnCount > 0)
        {
            if (spawnRateVariance <= Random.value)
            {
                // Spawn by time
                if (timeSinceLast * spawnRate > 1f)
                {
                    Spawn();
                }
                else
                {
                    timeSinceLast += Time.deltaTime;
                }
            }
            else
            {
                // Spawn by random
                if (Time.deltaTime * spawnRate > Random.value)
                {
                    Spawn();
                }
                else
                {
                    timeSinceLast += Time.deltaTime;
                }
            }
        }
        //if (Time.timeSinceLevelLoad != 0f)
        //    Debug.Log(spawnedCount / Time.timeSinceLevelLoad);
    }

    private void Spawn()
    {
        timeSinceLast = 0f;
        spawnCount--;
        spawnedCount++;
        Instantiate(prefab, transform.position + new Vector3(Random.Range(-transform.localScale.x, transform.localScale.x) / 2f, 0f, Random.Range(-transform.localScale.z, transform.localScale.z)), Quaternion.Euler(0f, Random.value * 360f, 0f), ZombieSpawnerController.enemyHolder);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x, 2f, transform.localScale.z));
    }
}