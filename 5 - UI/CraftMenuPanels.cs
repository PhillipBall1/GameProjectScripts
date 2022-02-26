using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftMenuPanels : MonoBehaviour
{
    public TMP_Text menuText;
    public GameObject template;
    public Transform parent;

    public List<ItemObject> toolItems;
    public List<ItemObject> weaponItems;
    public List<ItemObject> armorItems;
    public List<ItemObject> medicalItems;
    public List<ItemObject> foodItems;
    public List<ItemObject> clothingItems;
    public List<ItemObject> constructionItems;
    public List<ItemObject> survivalItems;

    public void ToolsMenuOpen()
    {
        ListItems(toolItems, "Craft Tools");
    }
    public void WeaponsMenuOpen()
    {
        ListItems(weaponItems, "Craft Weapons");
    }
    public void ArmorMenuOpen()
    {
        ListItems(armorItems, "Craft Armor");
    }
    public void MedicalMenuOpen()
    {
        ListItems(medicalItems, "Craft Medical Items");
    }
    public void FoodMenuOpen()
    {
        ListItems(foodItems, "Craft Food Items");
    }
    public void ClothingMenuOpen()
    {
        ListItems(clothingItems, "Craft Clothing");
    }
    public void ConstructionMenuOpen()
    {
        ListItems(constructionItems, "Craft Construction Items");
    }
    public void SurvivalMenuOpen()
    {
        ListItems(survivalItems, "Craft Survival Items");
    }

    private void RemoveItems()
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }


    private void ListItems(List<ItemObject> items, string text)
    {
        menuText.text = text;
        RemoveItems();
        for (int i = 0; i < items.Count; i++)
        {
            GameObject g = Instantiate(template, parent);
            g.GetComponent<CraftButtonPressed>().craftedItem = items[i];
            g.GetComponentInChildren<TMP_Text>().text = items[i].itemName;
            g.GetComponentInChildren<Image>().sprite = items[i].uiDisplay;
        }
    }
}
