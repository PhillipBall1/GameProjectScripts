using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCraft : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject hotBar;

    [NonSerialized]
    public bool canCraft;
  

    public void RemoveOneItemAndAmount(ItemObject craftItem, int craftAmount)
    {
        if (inventory.IsItemAndAmountInInv(craftItem, craftAmount))
        {
            if(inventory.RemoveItemAndAmount(craftItem, craftAmount)) canCraft = true;
        } 
        else if (hotBar.IsItemAndAmountInInv(craftItem, craftAmount))
        {
            if (hotBar.RemoveItemAndAmount(craftItem, craftAmount)) canCraft = true;
        }
    }
    public void RemoveTwoItemsAndAmounts(ItemObject craftItemOne, ItemObject craftItemTwo, int craftAmountOne, int craftAmountTwo)
    {
        if (inventory.IsItemAndAmountInInv(craftItemOne, craftAmountOne) && inventory.IsItemAndAmountInInv(craftItemTwo, craftAmountTwo))
        {
            if (inventory.RemoveItemAndAmount(craftItemOne, craftAmountOne) && inventory.RemoveItemAndAmount(craftItemTwo, craftAmountTwo)) canCraft = true;
        }
        else if (hotBar.IsItemAndAmountInInv(craftItemOne, craftAmountOne) && hotBar.IsItemAndAmountInInv(craftItemTwo, craftAmountTwo))
        {
            if (hotBar.RemoveItemAndAmount(craftItemOne, craftAmountOne) && hotBar.RemoveItemAndAmount(craftItemTwo, craftAmountTwo)) canCraft = true;
        }
        else
        {
            canCraft = false;
            Debug.Log("Not In Inventory");
        }
    }

    public void RemoveThreeItemsAndAmounts(ItemObject craftItemOne, ItemObject craftItemTwo,ItemObject craftItemThree, int craftAmountOne, int craftAmountTwo, int craftAmountThree)
    {
        
        if (inventory.IsItemAndAmountInInv(craftItemOne, craftAmountOne) && inventory.IsItemAndAmountInInv(craftItemTwo, craftAmountTwo) && inventory.IsItemAndAmountInInv(craftItemThree, craftAmountThree))
        {
            if (inventory.RemoveItemAndAmount(craftItemOne, craftAmountOne) &&
            inventory.RemoveItemAndAmount(craftItemTwo, craftAmountTwo) &&
            inventory.RemoveItemAndAmount(craftItemThree, craftAmountThree)) canCraft = true;
        }
        else if (hotBar.IsItemAndAmountInInv(craftItemOne, craftAmountOne) && hotBar.IsItemAndAmountInInv(craftItemTwo, craftAmountTwo) && hotBar.IsItemAndAmountInInv(craftItemThree, craftAmountThree))
        {
            if (hotBar.RemoveItemAndAmount(craftItemOne, craftAmountOne) &&
            hotBar.RemoveItemAndAmount(craftItemTwo, craftAmountTwo) &&
            hotBar.RemoveItemAndAmount(craftItemThree, craftAmountThree)) canCraft = true;
        }
        else
        {
            canCraft = false;
            Debug.Log("Missing Items");
        }
    }

}
