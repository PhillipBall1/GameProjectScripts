using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAnimals : MonoBehaviour
{

    public static bool animalsDone;
    [Header("Respawn Animal Pooler")]
    public ObjectPooler bearPoolerrr;
    public ObjectPooler wolf1Poolerr;
    public ObjectPooler wolf2Poolerr;
    public ObjectPooler boarPoolerrr;
    public ObjectPooler deerMPoolerr;
    public ObjectPooler deerFPoolerr;
    public ObjectPooler cow1Poolerrr;
    public ObjectPooler cow2Poolerrr;
    public ObjectPooler cow3Poolerrr;
    public ObjectPooler rabbitPooler;
    public ObjectPooler sheepPoolerr;
    public ObjectPooler foxPoolerrrr;

    [NonSerialized]public int bearAmounttt = 0;
    [NonSerialized]public int wolf1Amountt = 0;
    [NonSerialized]public int wolf2Amountt = 0;
    [NonSerialized]public int boarAmounttt = 0;
    [NonSerialized]public int deerMAmountt = 0;
    [NonSerialized]public int deerFAmountt = 0;
    [NonSerialized]public int cow1Amounttt = 0;
    [NonSerialized]public int cow2Amounttt = 0;
    [NonSerialized]public int cow3Amounttt = 0;
    [NonSerialized]public int rabbitAmount = 0;
    [NonSerialized]public int sheepAmountt = 0;
    [NonSerialized]public int foxAmountttt = 0;

    TerrainData mTerrainData;
    float[,,] mSplatmapData;
    int alphamapWidth;
    int alphamapHeight;
    int mNumTextures;
    int terrainIDX;

    private void Start()
    {
        StartCoroutine(CheckIfAnimalsDone());
        mTerrainData = Terrain.activeTerrain.terrainData;
        alphamapWidth = mTerrainData.alphamapWidth;
        alphamapHeight = mTerrainData.alphamapHeight;

        mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);
    }

    private void Update()
    {
        if (NodeSpawner.nodesDone)
        {
            SpawnAnimal(bearPoolerrr);
            SpawnAnimal(wolf1Poolerr);
            SpawnAnimal(wolf2Poolerr);
            SpawnAnimal(boarPoolerrr);
            SpawnAnimal(deerMPoolerr);
            SpawnAnimal(deerFPoolerr);
            SpawnAnimal(cow1Poolerrr);
            SpawnAnimal(cow2Poolerrr);
            SpawnAnimal(cow3Poolerrr);
            SpawnAnimal(rabbitPooler);
            SpawnAnimal(sheepPoolerr);
            SpawnAnimal(foxPoolerrrr);
        }
    }

    private void SpawnAnimal(ObjectPooler pooler)
    {
        int currentAmount = 0;
        if (pooler == bearPoolerrr) currentAmount = bearAmounttt;
        if (pooler == wolf1Poolerr) currentAmount = wolf1Amountt;
        if (pooler == wolf2Poolerr) currentAmount = wolf2Amountt;
        if (pooler == boarPoolerrr) currentAmount = boarAmounttt;
        if (pooler == deerMPoolerr) currentAmount = deerMAmountt;
        if (pooler == deerFPoolerr) currentAmount = deerFAmountt;
        if (pooler == cow1Poolerrr) currentAmount = cow1Amounttt;
        if (pooler == cow2Poolerrr) currentAmount = cow2Amounttt;
        if (pooler == cow3Poolerrr) currentAmount = cow3Amounttt;
        if (pooler == rabbitPooler) currentAmount = rabbitAmount;
        if (pooler == sheepPoolerr) currentAmount = sheepAmountt;
        if (pooler == foxPoolerrrr) currentAmount = foxAmountttt;
        float x = 1000;
        float y = 4000;
        RaycastHit hit;
        Vector3 randomTerrainPos = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));
        if (Physics.Raycast(randomTerrainPos, Vector3.down, out hit, 10000) && currentAmount < pooler.poolAmount)
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

                    if (pooler == bearPoolerrr) bearAmounttt++;
                    if (pooler == wolf1Poolerr) wolf1Amountt++;
                    if (pooler == wolf2Poolerr) wolf2Amountt++;
                    if (pooler == boarPoolerrr) boarAmounttt++;
                    if (pooler == deerMPoolerr) deerMAmountt++;
                    if (pooler == deerFPoolerr) deerFAmountt++;
                    if (pooler == cow1Poolerrr) cow1Amounttt++;
                    if (pooler == cow2Poolerrr) cow2Amounttt++;
                    if (pooler == cow3Poolerrr) cow3Amounttt++;
                    if (pooler == rabbitPooler) rabbitAmount++;
                    if (pooler == sheepPoolerr) sheepAmountt++;
                    if (pooler == foxPoolerrrr) foxAmountttt++;
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

    private IEnumerator CheckIfAnimalsDone()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (bearAmounttt == bearPoolerrr.poolAmount && 
                wolf1Amountt == wolf1Poolerr.poolAmount &&
                wolf2Amountt == wolf2Poolerr.poolAmount &&
                boarAmounttt == boarPoolerrr.poolAmount &&
                deerMAmountt == deerMPoolerr.poolAmount &&
                deerFAmountt == deerFPoolerr.poolAmount &&
                cow1Amounttt == cow1Poolerrr.poolAmount &&
                cow2Amounttt == cow2Poolerrr.poolAmount &&
                cow3Amounttt == cow3Poolerrr.poolAmount &&
                rabbitAmount == rabbitPooler.poolAmount &&
                sheepAmountt == sheepPoolerr.poolAmount &&
                foxAmountttt == foxPoolerrrr.poolAmount)
            {
                animalsDone = true;
                break;
            }
            else
            {
                animalsDone = false;
            }
        }
    }
}
