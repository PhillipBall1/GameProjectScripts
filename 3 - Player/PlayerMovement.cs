using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    
    public Animator anim;
    public Camera cam;
    public Transform head;
    public float moveSpeed;

    private InputManager inputManager;

    private Vector3 movement;
    private Vector3 velocity;

    private CharacterController playerController;

    private Player player;

    private float speed;
    private float gravity = -9f;
    private float jumpHeight = 1f;
    private bool isGrounded;

    TerrainData mTerrainData;
    float[,,] mSplatmapData;
    int alphamapWidth;
    int alphamapHeight;
    int mNumTextures;
    int terrainIDX;

    public Canvas spawnPlayer;
    public TMP_Text spawnText;

    private string dotText;
    private int dotCount;
    bool subPlus;

    public float slideFriction = 0.3f;
    private Vector3 hitNormal;

    private void Start()
    {
        mTerrainData = Terrain.activeTerrain.terrainData;
        alphamapWidth = mTerrainData.alphamapWidth;
        alphamapHeight = mTerrainData.alphamapHeight;
        mSplatmapData = mTerrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
        mNumTextures = mSplatmapData.Length / (alphamapWidth * alphamapHeight);

        inputManager = FindObjectOfType<InputManager>();
        player = transform.GetComponent<Player>();
        playerController = transform.GetComponent<CharacterController>();
        StartCoroutine(ResetPlayerPosition());
        StartCoroutine(DotText());
    }
    private void Update()
    {
        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.z);

        if (player.JustMenuDown() && LoadingScreen.done)
        {
            Sprint();
            MovementAnimation();
        }
        cam.transform.position = head.position;
    }

    private void FixedUpdate()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");

        if (player.JustMenuDown() && LoadingScreen.done)
        {
            Vector3 move = transform.right * movement.x + transform.forward * movement.z;
            playerController.Move(move * speed * Time.deltaTime);
        }
        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }


    private void MovementAnimation()
    {
        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        bool isGrounded2 = Physics.Raycast(newPos, -Vector3.up, 0.24f);
        isGrounded = Vector3.Angle(Vector3.up, hitNormal) <= playerController.slopeLimit;
        //JUMPING
        if (inputManager.GetKeyDown("Jump") && isGrounded && isGrounded2)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("jump");
        }
        if (!isGrounded)
        {
            anim.SetBool("isFalling", true);
            velocity.y += -0.9f;
            velocity.x += (1f - hitNormal.y) * hitNormal.x * (0.9f - slideFriction);
            velocity.z += (1f - hitNormal.y) * hitNormal.z * (0.9f - slideFriction);
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
            anim.SetBool("isFalling", false);
        }
        
        //IDLE
        if (movement == Vector3.zero)
        {
            anim.SetBool("isIdle", true);
        }
        else
        {   
            anim.SetBool("isIdle", false);
        }
        //RUNNING
        if (movement != Vector3.zero  && inputManager.GetKeyDown("Sprint"))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        //WALKING
        if(movement != Vector3.zero && !inputManager.GetKeyDown("Sprint"))
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        
    }
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            speed = moveSpeed * 2;
        }
        else
        {
            speed = moveSpeed;
        }
    }

    public IEnumerator DotText()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            
            
            if (dotCount == 3)
            {
                subPlus = true;
            }
            if(dotCount == 0)
            {
                subPlus = false;
            }
            if (subPlus)
            {
                dotCount--;
            }
            if(!subPlus)
            {
                dotCount++;
            }

            switch (dotCount)
            {
                case 0: dotText = ""; break;
                case 1: dotText = "."; break;
                case 2: dotText = ".."; break;
                case 3: dotText = "..."; break;
            }

            spawnText.text = "Spawning" + dotText;
        }
    }

    public IEnumerator ResetPlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.001f);
            break;
            //int x = 1024;
            //RaycastHit hit;
            //Vector3 randomTerrainPos = new Vector3(UnityEngine.Random.Range(840, 860), 10000, UnityEngine.Random.Range(-x, x));
            //if (Physics.Raycast(randomTerrainPos, Vector3.down, out hit, 20000))
            //{
            //    if (hit.transform.tag == "Terrain")
            //    {
            //        Vector3 position = hit.point;
            //        Vector3 TerrainCord = ToSplat(position);
            //        terrainIDX = 0;
            //        float comp = 0f;
            //        for (int i = 0; i < mNumTextures; i++)
            //        {
            //            if (comp < mSplatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
            //                terrainIDX = i;
            //        }
            //
            //        if (terrainIDX == 0)
            //        {
            //            playerController.enabled = false;
            //            transform.position = new Vector3(hit.point.x, hit.point.y + 3, hit.point.z);
            //            playerController.enabled = true;
            //            spawnPlayer.gameObject.SetActive(false);
            //            break;
            //        }
            //    }
            //}
            //if (MainMenu.newGame)
            //{
            //    int x = 1024;
            //    RaycastHit hit;
            //    Vector3 randomTerrainPos = new Vector3(UnityEngine.Random.Range(840, 860), 10000, UnityEngine.Random.Range(-x, x));
            //    if (Physics.Raycast(randomTerrainPos, Vector3.down, out hit, 20000))
            //    {
            //        if (hit.transform.tag == "Terrain")
            //        {
            //            Vector3 position = hit.point;
            //            Vector3 TerrainCord = ToSplat(position);
            //            terrainIDX = 0;
            //            float comp = 0f;
            //            for (int i = 0; i < mNumTextures; i++)
            //            {
            //                if (comp < mSplatmapData[(int)TerrainCord.z, (int)TerrainCord.x, i])
            //                    terrainIDX = i;
            //            }
            //
            //            if (terrainIDX == 0)
            //            {
            //                playerController.enabled = false;
            //                transform.position = new Vector3(hit.point.x, hit.point.y + 3, hit.point.z);
            //                playerController.enabled = true;
            //                spawnPlayer.gameObject.SetActive(false);
            //                break;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPositionX"), PlayerPrefs.GetFloat("PlayerPositionY"), PlayerPrefs.GetFloat("PlayerPositionZ"));
            //    break;
            //}
        }
        
    }

    private Vector3 ToSplat(Vector3 pos)
    {
        Vector3 vectorRet = new Vector3();
        Terrain terrain = Terrain.activeTerrain;
        Vector3 terrainPos = terrain.transform.position;
        vectorRet.x = ((pos.x - terrainPos.x) / terrain.terrainData.size.x) * terrain.terrainData.alphamapWidth;
        vectorRet.z = ((pos.z - terrainPos.z) / terrain.terrainData.size.z) * terrain.terrainData.alphamapHeight;
        return vectorRet;
    }
}
