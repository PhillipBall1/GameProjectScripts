using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMobs : MonoBehaviour
{
    private int amountToSpawn;
    private float constant = 1.3f;
    public DayAndNight dayNight;
    public int mobCount = 1;

    [Header("Respawn Mob Pooler")]
    public ObjectPooler zombiePooler;

    private int mobsSpawned;

    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (dayNight.isNight && DayAndNight.dayNumber > 1)
        {
            amountToSpawn = Mathf.RoundToInt(Mathf.Pow(DayAndNight.dayNumber, constant));
            if (mobsSpawned < amountToSpawn)
            {
                int rand = UnityEngine.Random.Range(1, mobCount + 1);
                switch (rand)
                {
                    case 1: SpawnMob(zombiePooler); break;
                }
            }
        }
        else
        {
            mobsSpawned = 0;
        }
    }

    private void SpawnMob(ObjectPooler pooler)
    {
        RaycastHit hit;
        Vector3 randomTerrainPos = GetRandomPosition();
        if (Physics.Raycast(randomTerrainPos, Vector3.down, out hit, 10000))
        {
            if (hit.transform.tag == "Terrain")
            {
                GameObject g = pooler.GetPooledObject();
                if (g != null)
                {
                    g.transform.position = hit.point;
                    g.transform.rotation = Quaternion.identity;
                    g.SetActive(true);
                    mobsSpawned++;
                }
            }

        }
    }

    private Vector3 GetRandomPosition()
    {
        var spawnDistance = 80.0f;
        var tolerance = 10f;
        var offset = player.transform.position + (-player.transform.forward * spawnDistance);
        var position = offset + new Vector3(UnityEngine.Random.Range(-tolerance, tolerance), 4000, UnityEngine.Random.Range(-tolerance, tolerance));
        return position;
    }
}
