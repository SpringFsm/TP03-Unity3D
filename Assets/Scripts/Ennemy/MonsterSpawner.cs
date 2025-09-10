using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private DayNightCycle dayNightCycle;

    [SerializeField] private GameObject monster;

    [SerializeField] private Transform playerPos;

    private float spawnMinDistance = 10;
    private bool canSpawn;

    void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    void Update()
    {
        float timer = dayNightCycle.timer;
        if (timer >= 180 && timer <= 360)
        {
            if (canSpawn)
            {
                for (int i = 0; i <= 5; i++)
                {
                    DoMobSpawn();
                }
                canSpawn = false;
            }
        }
        else
        {
            canSpawn = true;
        }
    }

    private void DoMobSpawn()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomDist = Random.Range(spawnMinDistance, spawnMinDistance + 10f);
        Vector3 spawnPos = playerPos.position + new Vector3(randomDir.x, 0, randomDir.y) * randomDist;

        Instantiate(monster, spawnPos, Quaternion.identity);
    }
}
