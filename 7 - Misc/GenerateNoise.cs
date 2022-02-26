using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Gaia;

public class GenerateNoise : MonoBehaviour
{
    public GameObject ball;
    private float x = 1000;
    private float y = 5000;

    [Header("Noise")]
    public int octaves = 1;
    public float freq = 0.5f;
    public float lacunarity = 2;
    public float persistence = 1;
    private float seed;
    public float zoom = 16;

    public float minFitness = 0.25f;
    public float failRate = 0;
    bool check = false;
    FractalGenerator noiseGenerator;
    
    // Start is called before the first frame update
    void Start()
    {
        seed = UnityEngine.Random.Range(1, 9999999);
        noiseGenerator = new FractalGenerator(freq, lacunarity, octaves, persistence, seed, FractalGenerator.Fractals.Perlin);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        Vector3 randomTerrainPos = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));
        Vector3 randomTerrainPos1 = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));
        Vector3 randomTerrainPos2 = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));
        Vector3 randomTerrainPos3 = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));
        Vector3 randomTerrainPos4 = new Vector3(UnityEngine.Random.Range(-x, x), y, UnityEngine.Random.Range(-x, x));

        if (Physics.Raycast(randomTerrainPos, Vector3.down, out hit, 10000))
        {
            float fitness = noiseGenerator.GetNormalisedValue((hit.point.x * (1f / zoom)), (hit.point.z * (1f / zoom)));
            if (hit.transform.tag == "Terrain")
            {
                bool check = false;
                if (fitness > minFitness) check = true;
                if (check) Instantiate(ball, hit.point, Quaternion.identity);
            }
        }
        if (Physics.Raycast(randomTerrainPos1, Vector3.down, out hit1, 10000))
        {
            float fitness = noiseGenerator.GetNormalisedValue( (hit1.point.x * (1f / zoom)),  (hit1.point.z * (1f / zoom)));
            if (hit1.transform.tag == "Terrain")
            {
                bool check = false;
                if (fitness > minFitness) check = true;
                if (check) Instantiate(ball, hit1.point, Quaternion.identity);
            }
        }
        if (Physics.Raycast(randomTerrainPos2, Vector3.down, out hit2, 10000))
        {
            float fitness = noiseGenerator.GetNormalisedValue((hit2.point.x * (1f / zoom)), (hit2.point.z * (1f / zoom)));
            if (hit2.transform.tag == "Terrain")
            {
                bool check = false;
                if (fitness > minFitness) check = true;
                if (check) Instantiate(ball, hit2.point, Quaternion.identity);
            }
        }
        if (Physics.Raycast(randomTerrainPos3, Vector3.down, out hit3, 10000))
        {
            float fitness = noiseGenerator.GetNormalisedValue((hit3.point.x * (1f / zoom)),  (hit3.point.z * (1f / zoom)));
            if (hit3.transform.tag == "Terrain")
            {
                bool check = false;
                if (fitness > minFitness) check = true;
                if (check) Instantiate(ball, hit3.point, Quaternion.identity);
            }
        }
        if (Physics.Raycast(randomTerrainPos4, Vector3.down, out hit4, 10000))
        {
            float fitness = noiseGenerator.GetNormalisedValue((hit4.point.x * (1f / zoom)),  (hit4.point.z * (1f / zoom)));
            if (hit4.transform.tag == "Terrain")
            {
                bool check = false;
                if (fitness > minFitness) check = true;
                if (check) Instantiate(ball, hit4.point, Quaternion.identity);
            }
        }
    }
}
