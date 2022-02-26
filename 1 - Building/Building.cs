using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Building : MonoBehaviour
{
    private int health;
    private int maxHealth;
    private Player player;

    public bool onUpgrade;
    public Slider healthBar;
    public Canvas canvas;
    public TMP_Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        canvas.gameObject.SetActive(false);
        SetHealth();
        health = maxHealth;
        healthBar.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        healthText.text = healthBar.value + "/" + healthBar.maxValue;
        if (onUpgrade)
        {
            health = maxHealth;
            onUpgrade = false;
        }
        if(player.hammerEquipped)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out hit, 5))
            {
                if(hit.transform.parent != null)
                {
                    if (hit.transform.parent.GetComponent<Building>()) canvas.gameObject.SetActive(true);
                    else canvas.gameObject.SetActive(false);

                }
                else canvas.gameObject.SetActive(false);
            }
            else canvas.gameObject.SetActive(false);
        }
        else canvas.gameObject.SetActive(false);
    }

    private void SetHealth()
    {
        switch (transform.tag)
        {
            //Building Prefabs
            case "Twig": maxHealth = 50; break;
            case "Wood": maxHealth = 100; break;
            case "Stone": maxHealth = 200; break;
            case "Metal": maxHealth = 400; break;
            case "HardMetal": maxHealth = 800; break;

            //Deployables
            case "WoodDoor": maxHealth = 100; break;
            case "MetalDoor": maxHealth = 400; break;
        }
    }


    public void TakeDamage(int damage) 
    {
        health -= damage;
        healthBar.value = health;
    }
}
