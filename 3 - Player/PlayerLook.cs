using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform map;
    public float mouseSense = 1f;
    public Transform playerBody;
    public Transform spine;

    private InputManager inputManager;

    private Player player;

    private PlayerAttack playerAttack;

    public Camera mapCam;

    private float xRotation;

    public static bool mapOpen;

    private void Start()
    {
        
        inputManager = FindObjectOfType<InputManager>();
        mapCam.gameObject.SetActive(false);
        map.gameObject.SetActive(false);
        player = playerBody.GetComponent<Player>();
        playerAttack = playerBody.GetComponent<PlayerAttack>();
    }

    private void FixedUpdate()
    {
        if (player.AllUIDown() && LoadingScreen.done)
        {
            MapOpen();
            LookAround();
        }
    }

    private void LookAround()
    {
        float mouseX = playerAttack.sideRecoil + Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime * 100;
        float mouseY = playerAttack.upRecoil + Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime * 100; 

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        spine.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(0, mouseX, 0);
        
    }

    private void MapOpen()
    {
        if (inputManager.GetKey("Map"))
        {
            map.gameObject.SetActive(true);
            mapCam.gameObject.SetActive(true);
        }
        else
        {
            map.gameObject.SetActive(false);
            mapCam.gameObject.SetActive(false);
        }
    }

    private void SavePreviousPos()
    {

    }
}
