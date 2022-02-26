using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedCullingSystem.DynamicCullingCore;

public class ObjectPooler : MonoBehaviour
{

    public static ObjectPooler current;
    public GameObject pooledObject;
    public int poolAmount;
    public bool dynamicCull;
    public bool willGrow;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for(int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            //if (dynamicCull)
            //{
            //    MeshRenderer r  = obj.transform.GetChild(0).GetComponent<MeshRenderer>();
            //    DynamicCulling.Instance.AddObjectForCulling(r);
            //}
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }
        return null;
    }
}
