using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Farmables : MonoBehaviour
{
    public string objName;

    [Header("Player Inventory")]
    public InventoryObject inventory;

    [Header("Type of Farmable")]
    public bool node;
    public bool tree;


    [Header("Pop-Up Text")]
    public GameObject floatingText;

    public int health;

    private TreeSpawner treeSpawner;
    private NodeSpawner nodeSpawner;


    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(150, 300);
        treeSpawner = GameObject.FindGameObjectWithTag("Manager").GetComponent<TreeSpawner>();
        nodeSpawner = GameObject.FindGameObjectWithTag("Manager").GetComponent<NodeSpawner>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            switch (objName)
            {
                case "Tree1": treeSpawner.treeAmount1 -= 1; break;
                case "Tree2": treeSpawner.treeAmount2 -= 1; break;
                case "Tree3": treeSpawner.treeAmount3 -= 1; break;
                case "Tree4": treeSpawner.treeAmount4 -= 1; break;
                case "Tree5": treeSpawner.treeAmount5 -= 1; break;
                case "Tree6": treeSpawner.treeAmount6 -= 1; break;
                case "Tree7": treeSpawner.treeAmount7 -= 1; break;
                case "Tree8": treeSpawner.treeAmount8 -= 1; break;
                case "Metal": nodeSpawner.stoneAmount -= 1; break;
                case "Stone": nodeSpawner.metalAmount -= 1; break;
            }

            if (objName.Contains("tree"))
            {
                //Spawn a fallen tree?
            }
            else
            {
                //Particles explode for stone?
            }

            gameObject.SetActive(false);
            health = Random.Range(150, 300);
        }
    }
}
