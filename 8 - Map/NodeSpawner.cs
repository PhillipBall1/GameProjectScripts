using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    public static bool nodesDone;

    [Header("Poolers")]
    public ObjectPooler stonePooler;
    public ObjectPooler metalPooler;

    [NonSerialized]
    public int stoneAmount;
    [NonSerialized]
    public int metalAmount;

    private int stoneSpawnAmount;
    private int metalSpawnAmount;

    TerrainData mTerrainData;
    float[,,] mSplatmapData;
    int alphamapWidth;
    int alphamapHeight;
    int mNumTextures;
    int terrainIDX;

    private void Start()
    {
        StartCoroutine(CheckIfNodesDone());
        stoneSpawnAmount = stonePooler.poolAmount;
        metalSpawnAmount = metalPooler.poolAmount;

        mTerrainData = Terrain.activeTerrain.terrainData;
        alphamapWidth = mTerrainData.alphamapWidth;
        alphamapHeight = mTerrainData.alphamapHeight;

        mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);


    }

    private void Update()
    {
        if (TreeSpawner.treesDone)
        {
            SpawnFarmable(stoneSpawnAmount, stonePooler);
            SpawnFarmable(metalSpawnAmount, metalPooler);
            SpawnFarmable(stoneSpawnAmount, stonePooler);
            SpawnFarmable(metalSpawnAmount, metalPooler);
            SpawnFarmable(stoneSpawnAmount, stonePooler);
            SpawnFarmable(metalSpawnAmount, metalPooler);
            SpawnFarmable(stoneSpawnAmount, stonePooler);
            SpawnFarmable(metalSpawnAmount, metalPooler);
            SpawnFarmable(stoneSpawnAmount, stonePooler);
            SpawnFarmable(metalSpawnAmount, metalPooler);
            SpawnFarmable(stoneSpawnAmount, stonePooler);
            SpawnFarmable(metalSpawnAmount, metalPooler);
        }
    }

    private void SpawnFarmable(int maxAmount, ObjectPooler pooler)
    {
        float currentAmount = 0;
        if (pooler == stonePooler) currentAmount = stoneAmount;
        if (pooler == metalPooler) currentAmount = metalAmount;
        float x = 1000;
        float y = 4000;
        if (currentAmount < maxAmount)
        {
            RaycastHit hit;
            Vector3 randomTerrainPos = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));
            if (Physics.Raycast(randomTerrainPos, Vector3.down, out hit, 10000))
            {
                if (hit.transform.tag == "Terrain")
                {
                    Vector3 position = hit.point;
                    Vector3 TerrainCord = ConvertToSplatMapCoordinate(position);
                    terrainIDX = 0;
                    float comp = 0f;
                    for (int i = 0; i < mNumTextures; i++)
                    {
                        if (comp < mSplatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
                            terrainIDX = i;
                    }

                    bool rightTerrian = false;
                    if (terrainIDX == 1 || terrainIDX == 2 || terrainIDX == 3)
                    {
                        rightTerrian = true;
                    }

                    GameObject g = pooler.GetPooledObject();
                    if (g != null && rightTerrian)
                    {
                        g.transform.position = hit.point;
                        g.transform.rotation = Quaternion.identity;
                        g.SetActive(true);
                        if (pooler == stonePooler) stoneAmount++;
                        if (pooler == metalPooler) metalAmount++;
                        g.transform.parent = pooler.transform;
                    }
                }

            }
        }
    }

    private Vector3 ConvertToSplatMapCoordinate(Vector3 pos)
    {
        Vector3 vecRet = new Vector3();
        Terrain ter = Terrain.activeTerrain;
        Vector3 terPosition = ter.transform.position;
        vecRet.x = ((pos.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
        vecRet.z = ((pos.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
        return vecRet;
    }

    private IEnumerator CheckIfNodesDone()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (stoneAmount == stonePooler.poolAmount && metalAmount == metalPooler.poolAmount)
            {
                nodesDone = true;
                break;
            }
            else
            {
                nodesDone = false;
            }
        }
    }
}
