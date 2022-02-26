using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public Player player;
    public DayAndNight dayTime;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public InventoryObject hotBar;

    // Start is called before the first frame update
    void Start()
    {
        if(player != null)
        {
            dayTime = FindObjectOfType<DayAndNight>();
            player = FindObjectOfType<Player>();
        }
    }

    public void PreConfirmSave()
    {
        //Open a panel with an exit and confirm button
    }

    public void SaveGameButton()
    {
        
        //Player Postiton
        PlayerPrefs.SetFloat("PlayerPositionX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", player.transform.position.y + 0.5f);
        PlayerPrefs.SetFloat("PlayerPositionZ", player.transform.position.z);

        //Player Stats
        PlayerPrefs.SetInt("CurrentFood", player.currentFood);
        PlayerPrefs.SetInt("CurrentHealth", player.currentHealth);
        PlayerPrefs.SetInt("CurrentWater", player.currentWater);

        Debug.Log(PlayerPrefs.GetInt("CurrentFood"));
        Debug.Log(PlayerPrefs.GetInt("CurrentHealth"));
        Debug.Log(PlayerPrefs.GetInt("CurrentWater"));
        //Player Inventory/Gear
        inventory.Save();
        equipment.Save();
        hotBar.Save();

        //Day and Time
        PlayerPrefs.SetInt("CurrentDay", DayAndNight.dayNumber);
        PlayerPrefs.SetFloat("CurrentTime", dayTime.timeOfDay);
    }
}
