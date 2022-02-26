using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

public class TreeSpawner : MonoBehaviour
{

    public static bool treesDone;
    public bool debug;
    [Header("Noise")]
    public int octaves = 1;
    public float freq = 0.5f;
    public float lacunarity = 2;
    public float persistence = 1;
    private float seed;
    public float zoom = 16;

    public float minFitness = 0.25f;
    public float failRate = 0;


    [Header("Poolers")]
    public ObjectPooler treePooler1;
    public ObjectPooler treePooler2;
    public ObjectPooler treePooler3;
    public ObjectPooler treePooler4;
    public ObjectPooler treePooler5;
    public ObjectPooler treePooler6;
    public ObjectPooler treePooler7;
    public ObjectPooler treePooler8;

    [NonSerialized]public int treeAmount1;
    [NonSerialized]public int treeAmount2;
    [NonSerialized]public int treeAmount3;
    [NonSerialized]public int treeAmount4;
    [NonSerialized]public int treeAmount5;
    [NonSerialized]public int treeAmount6;
    [NonSerialized]public int treeAmount7;
    [NonSerialized]public int treeAmount8;

    FractalGenerator noiseGenerator;

    TerrainData mTerrainData;
    float[,,] mSplatmapData;
    int alphamapWidth;
    int alphamapHeight;
    int mNumTextures;
    int terrainIDX;
    private void Start()
    {
        StartCoroutine(CheckIfTreesDone());
        seed = UnityEngine.Random.Range(1, 9999999);
        noiseGenerator = new FractalGenerator(freq, lacunarity, octaves, persistence, seed, FractalGenerator.Fractals.Perlin);

        mTerrainData = Terrain.activeTerrain.terrainData;
        alphamapWidth = mTerrainData.alphamapWidth;
        alphamapHeight = mTerrainData.alphamapHeight;

        mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);
    }
    private void Update()
    {
        SpawnFarmable(treePooler1);
        SpawnFarmable(treePooler2);
        SpawnFarmable(treePooler3);
        SpawnFarmable(treePooler4);
        SpawnFarmable(treePooler5);
        SpawnFarmable(treePooler6);
        SpawnFarmable(treePooler7);
        SpawnFarmable(treePooler8);
    }

    private void SpawnFarmable(ObjectPooler pooler)
    {
        float currentAmount = 0;
        if (pooler == treePooler1) currentAmount = treeAmount1;
        if (pooler == treePooler2) currentAmount = treeAmount2;
        if (pooler == treePooler3) currentAmount = treeAmount3;
        if (pooler == treePooler4) currentAmount = treeAmount4;
        if (pooler == treePooler5) currentAmount = treeAmount5;
        if (pooler == treePooler6) currentAmount = treeAmount6;
        if (pooler == treePooler7) currentAmount = treeAmount7;
        if (pooler == treePooler8) currentAmount = treeAmount8;
        float x = 1000;
        float y = 4000;
        RaycastHit hit;
        Vector3 randomTerrainPos = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));
        if (Physics.Raycast(randomTerrainPos, Vector3.down, out hit, 10000) && currentAmount < pooler.poolAmount)
        {
            float fitness = noiseGenerator.GetNormalisedValue(seed + (hit.point.x * (1f / zoom)), seed + (hit.point.z * (1f / zoom)));
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

                bool check = false;
                if (fitness > minFitness)
                {
                    check = true;
                }

                bool rightTerrian = false;
                if (terrainIDX == 1 || terrainIDX == 2 || terrainIDX == 3)
                {
                    rightTerrian = true;
                }

                if (debug) Debug.Log("Able to Spawn: " + check);
                if (debug) Debug.Log("Noise Current Value: " + fitness);

                GameObject g = pooler.GetPooledObject();
                if (g != null && check && rightTerrian)
                {
                    g.transform.position = hit.point;
                    if (pooler == treePooler1)
                    {
                        g.transform.rotation = Quaternion.FromToRotation(g.transform.up, hit.normal) * g.transform.rotation;
                    }
                    else
                    {
                        g.transform.rotation = Quaternion.identity;
                    }
                    g.SetActive(true);
                    if (pooler == treePooler1) treeAmount1++;
                    if (pooler == treePooler2) treeAmount2++;
                    if (pooler == treePooler3) treeAmount3++;
                    if (pooler == treePooler4) treeAmount4++;
                    if (pooler == treePooler5) treeAmount5++;
                    if (pooler == treePooler6) treeAmount6++;
                    if (pooler == treePooler7) treeAmount7++;
                    if (pooler == treePooler8) treeAmount8++;
                    g.transform.parent = pooler.transform;
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

    private IEnumerator CheckIfTreesDone()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(treeAmount1 == treePooler1.poolAmount && 
               treeAmount4 == treePooler4.poolAmount &&
               treeAmount2 == treePooler2.poolAmount && 
               treeAmount5 == treePooler5.poolAmount && 
               treeAmount3 == treePooler3.poolAmount && 
               treeAmount6 == treePooler6.poolAmount &&
               treeAmount7 == treePooler7.poolAmount &&
               treeAmount8 == treePooler8.poolAmount)
            {
                treesDone = true;
                break;
            }
            else
            {
                treesDone = false;
            }
        }
    }
}
