using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Preview : MonoBehaviour
{
    public GameObject prefab;

    public bool flatRequired = false;
    public bool isFreeDeployable = false;
    public bool snaps = false;

    public bool snappingToWalls;
    public bool snapsToFoundations;
    public bool snapsToDoorWays;
    public bool snapsToCeilings;

    public Transform groundCheck;
    public Transform groundCheck1;
    public Transform groundCheck2;
    public Transform groundCheck3;


    [NonSerialized]
    public bool isDeployable;

    private Player player;
    private Terrain terrain;
    private IODataBase dataBase;
    private MeshRenderer myRend;
    private PlayerCraft playerCraft;
    private BuildSystem buildSystem;

    private float groundDistance = 0.8f;
    private string currentTag;

    private LayerMask buildingMask;
    private LayerMask groundMask;

    private bool isSnapped;
    private bool isGrounded;
    private bool isGrounded1;
    private bool isGrounded2;
    private bool isGrounded3;
    private bool checkFoundation;
    private bool completelyGrounded;

    private Material goodHS;
    private Material badHS;


    private void Start()
    {
        buildingMask = LayerMask.GetMask("Building");
        groundMask = LayerMask.GetMask("Ground");
        dataBase = FindObjectOfType<IODataBase>();

        goodHS = dataBase.goodHS;
        badHS = dataBase.badHS;

        terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Terrain>();
        playerCraft = FindObjectOfType<PlayerCraft>();
        player = FindObjectOfType<Player>();
        buildSystem = FindObjectOfType<BuildSystem>();
        myRend = GetComponent<MeshRenderer>();
        ChangeColor();
    }

    private void Update()
    {
        if (isSnapped)
        {
            ChangeColor();
        }
        if (!isSnapped)
        {
            ChangeColor();
        }

        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        if (groundCheck1 != null)
        {
            isGrounded1 = Physics.CheckSphere(groundCheck1.position, groundDistance, groundMask);
        }
        if (groundCheck2 != null)
        {
            isGrounded2 = Physics.CheckSphere(groundCheck2.position, groundDistance, groundMask);
        }
        if (groundCheck3 != null)
        {
            isGrounded3 = Physics.CheckSphere(groundCheck3.position, groundDistance, groundMask);
        }

        if (isGrounded && isGrounded1 && isGrounded2 && isGrounded3)
        {
            completelyGrounded = true;
        }
        else
        {
            completelyGrounded = false;
        }
        GroundedLogic();

        //Deployable Required
        DeployableLogic();

        //Default
        //BuildingSupported();

        //Other
        DebuggingPreview();
    }

    private void OnTriggerStay(Collider col)
    {
        //Check Tags
        if (snapsToCeilings)
        {
            currentTag = "CeilingSP";
        }
        if (snappingToWalls)
        {
            currentTag = "WallSP";
        }
        if (snapsToFoundations)
        {
            currentTag = "FoundationSP";
        }
        if (snapsToDoorWays)
        {
            currentTag = "DoorSP";
        }
        //-----------------------------------

        if (col.CompareTag("Twig"))
        {
            isSnapped = false;
        }
        if (col.CompareTag("Wood"))
        {
            isSnapped = false;
        }
        if (col.CompareTag("Stone"))
        {
            isSnapped = false;
        }
        if (col.CompareTag("Metal"))
        {
            isSnapped = false;
        }
        if (col.CompareTag("HardMetal"))
        {
            isSnapped = false;
        }

        //FIX BUILKDING DICK HEAD

        if (currentTag != null)
        {
            if (col.CompareTag(currentTag))
            {
                if (flatRequired)
                {
                    if (completelyGrounded)
                    {
                        buildSystem.PauseBuild(true);
                        transform.position = col.transform.position;
                        isSnapped = true;
                    }
                }
                else
                {
                    buildSystem.PauseBuild(true);
                    transform.position = col.transform.position;
                    isSnapped = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {   
        if(col.gameObject.layer == 12)
        {
            Debug.Log("HERE");
            isSnapped = false;
        }
    }
    public void Place()
    {
        if (player.deployableEquipped)
        {
            isDeployable = true;
            playerCraft.RemoveOneItemAndAmount(player.deployable, 1);
            
            Instantiate(prefab, transform.position, transform.rotation);
            Destroy(gameObject);
            player.deployableEquipped = false;
        }
        else
        {
            playerCraft.RemoveOneItemAndAmount(dataBase.wood, 25);
            if (playerCraft.canCraft)
            {
                isDeployable = false;
                Instantiate(prefab, transform.position, transform.rotation);
            }
        }
    }

    private void ChangeColor()
    {
        if (isSnapped)
        {
            GameObject[] h = GameObject.FindGameObjectsWithTag("BP");
            foreach (GameObject g in h)
            {
                g.GetComponent<MeshRenderer>().material = goodHS;
            }
        }
        else
        {
            GameObject[] h = GameObject.FindGameObjectsWithTag("BP");
            foreach (GameObject g in h)
            {
                g.GetComponent<MeshRenderer>().material = badHS;
            }
        }
        
    }

    public bool GetSnapped()
    {
        return isSnapped;
    }

    private void DeployableLogic()
    {
        if (isFreeDeployable)
        {
            isGrounded = Physics.Raycast(groundCheck.position, -Vector3.up, 4f);

            if (isGrounded)
            {

                isSnapped = true;
            }
            else
            {
                isSnapped = false;
            }
        }
    }


    private void GroundedLogic()
    {
        if (flatRequired)
        {
            checkFoundation = Physics.CheckSphere(groundCheck.position, 5f, buildingMask);
            if (completelyGrounded)
            {
                if (checkFoundation && !isSnapped)
                {
                    isSnapped = false;
                }
                else
                {
                    isSnapped = true;
                }
            }
            else
            {
                isSnapped = false;
            }
        }
    }

    private void DebuggingPreview()
    {
        if (flatRequired)
        {
            if (!isGrounded)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Corner 1 is Not in Terrain");
                }
            }
            if (!isGrounded1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Corner 2 is Not in Terrain");
                }
            }
            if (!isGrounded2)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Corner 3 is Not in Terrain");
                }
            }
            if (!isGrounded3)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Corner 4 is Not in Terrain");
                }
            }
            if (checkFoundation && !isSnapped)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Too Close to Another Building");
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isSnapped)
                {
                    Debug.Log("BRO WHY");
                }
            }
        }
    }
}
