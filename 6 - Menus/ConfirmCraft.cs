using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmCraft : MonoBehaviour
{

    public InventoryObject inventory;
    public InventoryObject hotBar;
    public ItemObject currentItemToCraft;
    public TMP_InputField amountToCraftInput;

    private GameObject craftInfoPanel;
    private GameObject confirmPanel;

    private Transform confirmHolder;
    private Transform infoHolder;
    private int amountToCraft;
    void Start()
    {
        craftInfoPanel = GameObject.FindGameObjectWithTag("CraftingParent");
        confirmPanel = GameObject.FindGameObjectWithTag("ConfirmCraft");
        confirmHolder = confirmPanel.transform.GetChild(0);
        infoHolder = craftInfoPanel.transform.GetChild(0);

        confirmHolder.gameObject.SetActive(false);
        infoHolder.gameObject.SetActive(false);
        amountToCraft = 1;
        amountToCraftInput.text = "1";
    }

    public void CraftButtonPressed()
    {
        if (currentItemToCraft.oneItem)
        {
            if (inventory.RemoveItemAndAmount(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft))
            {
                if (currentItemToCraft.stackable)
                {
                    if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), amountToCraft);
                    else inventory.AddItem(new Item(currentItemToCraft), amountToCraft);
                }
                else
                {
                    for (int i = 0; i < amountToCraft; i++)
                    {
                        if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), 1);
                        else inventory.AddItem(new Item(currentItemToCraft), 1);
                    }
                }
            }
            else if(hotBar.RemoveItemAndAmount(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft))
            {
                if (currentItemToCraft.stackable)
                {
                    if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), amountToCraft);
                    else inventory.AddItem(new Item(currentItemToCraft), amountToCraft);
                }
                else
                {
                    for (int i = 0; i < amountToCraft; i++)
                    {
                        if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), 1);
                        else inventory.AddItem(new Item(currentItemToCraft), 1);
                    }
                }
            }
        }
        if (currentItemToCraft.twoItem)
        {
            if (inventory.RemoveItemAndAmount(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft) && 
                inventory.RemoveItemAndAmount(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft))
            {
                if (currentItemToCraft.stackable)
                {
                    if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), amountToCraft);
                    else inventory.AddItem(new Item(currentItemToCraft), amountToCraft);
                }
                else
                {
                    for (int i = 0; i < amountToCraft; i++)
                    {
                        if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), 1);
                        else inventory.AddItem(new Item(currentItemToCraft), 1);
                    }
                }
            }
            else if (hotBar.RemoveItemAndAmount(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft) &&
                    hotBar.RemoveItemAndAmount(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft))
            {
                if (currentItemToCraft.stackable)
                {
                    if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), amountToCraft);
                    else inventory.AddItem(new Item(currentItemToCraft), amountToCraft);
                }
                else
                {
                    for (int i = 0; i < amountToCraft; i++)
                    {
                        if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), 1);
                        else inventory.AddItem(new Item(currentItemToCraft), 1);
                    }
                }
            }
        }
        if (currentItemToCraft.threeItem)
        {
            if (inventory.RemoveItemAndAmount(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft) &&
                inventory.RemoveItemAndAmount(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft) &&
                inventory.RemoveItemAndAmount(currentItemToCraft.itemThree, currentItemToCraft.itemThreeAmount * amountToCraft))
            {
                if (currentItemToCraft.stackable)
                {
                    if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), amountToCraft);
                    else inventory.AddItem(new Item(currentItemToCraft), amountToCraft);
                }
                else
                {
                    for (int i = 0; i < amountToCraft; i++)
                    {
                        if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), 1);
                        else inventory.AddItem(new Item(currentItemToCraft), 1);
                    }
                }
            }
            else if (hotBar.RemoveItemAndAmount(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft) &&
                     hotBar.RemoveItemAndAmount(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft) &&
                     hotBar.RemoveItemAndAmount(currentItemToCraft.itemThree, currentItemToCraft.itemThreeAmount * amountToCraft))
            {
                if (currentItemToCraft.stackable)
                {
                    if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), amountToCraft);
                    else inventory.AddItem(new Item(currentItemToCraft), amountToCraft);
                }
                else
                {
                    for(int i = 0; i < amountToCraft; i++)
                    {
                        if (hotBar.GetEmptySlot() != null) hotBar.AddItem(new Item(currentItemToCraft), 1);
                        else inventory.AddItem(new Item(currentItemToCraft), 1);
                    }
                }
            }
        }

        confirmHolder.gameObject.SetActive(false);
        infoHolder.gameObject.SetActive(false);
        amountToCraft = 1;
        amountToCraftInput.text = "1";
    }

    public void AmountChanged()
    {
        amountToCraft = Int32.Parse(amountToCraftInput.text);
        if (amountToCraft != 0)
        {
            Transform oneItemPanel = infoHolder.transform.Find("OneItem");
            Transform twoItemPanel = infoHolder.transform.Find("TwoItem");
            Transform threeItemPanel = infoHolder.transform.Find("ThreeItem");


            #region OneItem
            if (currentItemToCraft.oneItem)
            {
                //panel = Instantiate(infoPanel, mousePos, Quaternion.identity);
                //panel.transform.parent = parent.transform;

                oneItemPanel.gameObject.SetActive(true);
                twoItemPanel.gameObject.SetActive(false);
                threeItemPanel.gameObject.SetActive(false);


                oneItemPanel.transform.Find("Image").GetComponent<Image>().sprite = currentItemToCraft.itemOne.uiDisplay;
                oneItemPanel.transform.Find("Text").GetComponent<TMP_Text>().text = currentItemToCraft.itemOne.itemName + ": " + currentItemToCraft.itemOneAmount * amountToCraft;

                if (inventory.IsItemAndAmountInInv(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft) || hotBar.IsItemAndAmountInInv(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft))
                {
                    oneItemPanel.transform.Find("Text").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    oneItemPanel.transform.Find("Text").GetComponent<TMP_Text>().color = Color.red;
                }

            }
            #endregion

            #region Two Item
            if (currentItemToCraft.twoItem)
            {
                //panel = Instantiate(infoPanel, mousePos, Quaternion.identity);
                //panel.transform.parent = parent.transform;

                oneItemPanel.gameObject.SetActive(false);
                twoItemPanel.gameObject.SetActive(true);
                threeItemPanel.gameObject.SetActive(false);

                twoItemPanel.transform.Find("Image1").GetComponent<Image>().sprite = currentItemToCraft.itemOne.uiDisplay;
                twoItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().text = currentItemToCraft.itemOne.itemName + ": " + currentItemToCraft.itemOneAmount * amountToCraft;

                if (inventory.IsItemAndAmountInInv(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft) || hotBar.IsItemAndAmountInInv(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft))
                {
                    twoItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    twoItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.red;
                }


                twoItemPanel.transform.Find("Image2").GetComponent<Image>().sprite = currentItemToCraft.itemTwo.uiDisplay;
                twoItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().text = currentItemToCraft.itemTwo.itemName + ": " + currentItemToCraft.itemTwoAmount * amountToCraft;

                if (inventory.IsItemAndAmountInInv(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft) || hotBar.IsItemAndAmountInInv(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft))
                {
                    twoItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    twoItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().color = Color.red;
                }

            }
            #endregion

            #region Three Item

            if (currentItemToCraft.threeItem)
            {
                //panel = Instantiate(infoPanel, mousePos, Quaternion.identity);
                //panel.transform.parent = parent.transform;

                oneItemPanel.gameObject.SetActive(false);
                twoItemPanel.gameObject.SetActive(false);
                threeItemPanel.gameObject.SetActive(true);

                threeItemPanel.transform.Find("Image1").GetComponent<Image>().sprite = currentItemToCraft.itemOne.uiDisplay;
                threeItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().text = currentItemToCraft.itemOne.itemName + ": " + currentItemToCraft.itemOneAmount * amountToCraft;

                if (inventory.IsItemAndAmountInInv(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft) || hotBar.IsItemAndAmountInInv(currentItemToCraft.itemOne, currentItemToCraft.itemOneAmount * amountToCraft))
                {
                    threeItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    threeItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.red;
                }

                threeItemPanel.transform.Find("Image2").GetComponent<Image>().sprite = currentItemToCraft.itemTwo.uiDisplay;
                threeItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().text = currentItemToCraft.itemTwo.itemName + ": " + currentItemToCraft.itemTwoAmount * amountToCraft;

                if (inventory.IsItemAndAmountInInv(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft) || hotBar.IsItemAndAmountInInv(currentItemToCraft.itemTwo, currentItemToCraft.itemTwoAmount * amountToCraft))
                {
                    threeItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    threeItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().color = Color.red;
                }

                threeItemPanel.transform.Find("Image3").GetComponent<Image>().sprite = currentItemToCraft.itemThree.uiDisplay;
                threeItemPanel.transform.Find("Text4").GetComponent<TMP_Text>().text = currentItemToCraft.itemThree.itemName + ": " + currentItemToCraft.itemThreeAmount * amountToCraft;

                if (inventory.IsItemAndAmountInInv(currentItemToCraft.itemThree, currentItemToCraft.itemThreeAmount * amountToCraft) || hotBar.IsItemAndAmountInInv(currentItemToCraft.itemThree, currentItemToCraft.itemThreeAmount * amountToCraft))
                {
                    threeItemPanel.transform.Find("Text4").GetComponent<TMP_Text>().color = Color.green;
                }
                else
                {
                    threeItemPanel.transform.Find("Text4").GetComponent<TMP_Text>().color = Color.red;
                }

            }
            #endregion
        }
    }
}
