using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CraftButtonPressed : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject hotBar;
    public ItemObject craftedItem;


    private GameObject craftInfoPanel;
    private GameObject confirmPanel;

    public void CraftItemClicked()
    {
        craftInfoPanel = GameObject.FindGameObjectWithTag("CraftingParent");
        confirmPanel = GameObject.FindGameObjectWithTag("ConfirmCraft");

        Transform confirmHolder = confirmPanel.transform.GetChild(0);
        confirmHolder.gameObject.SetActive(true);
        Transform infoHolder = craftInfoPanel.transform.GetChild(0);
        infoHolder.gameObject.SetActive(true);

        confirmPanel.GetComponent<ConfirmCraft>().currentItemToCraft = craftedItem;
        Transform craftImage = confirmHolder.transform.Find("CraftItemImage");
        Transform craftText = confirmHolder.transform.Find("CraftItemName");

        craftImage.GetComponent<Image>().sprite = craftedItem.uiDisplay;
        craftText.GetComponent<TMP_Text>().text = craftedItem.itemName;

        Transform oneItemPanel = infoHolder.transform.Find("OneItem");
        Transform twoItemPanel = infoHolder.transform.Find("TwoItem");
        Transform threeItemPanel = infoHolder.transform.Find("ThreeItem");


        #region OneItem
        if (craftedItem.oneItem)
        {
            //panel = Instantiate(infoPanel, mousePos, Quaternion.identity);
            //panel.transform.parent = parent.transform;

            oneItemPanel.gameObject.SetActive(true);
            twoItemPanel.gameObject.SetActive(false);
            threeItemPanel.gameObject.SetActive(false);


            oneItemPanel.transform.Find("Image").GetComponent<Image>().sprite = craftedItem.itemOne.uiDisplay;
            oneItemPanel.transform.Find("Text").GetComponent<TMP_Text>().text = craftedItem.itemOne.itemName + ": " + craftedItem.itemOneAmount;

            if (inventory.IsItemAndAmountInInv(craftedItem.itemOne, craftedItem.itemOneAmount) || hotBar.IsItemAndAmountInInv(craftedItem.itemOne, craftedItem.itemOneAmount))
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
        if (craftedItem.twoItem)
        {
            //panel = Instantiate(infoPanel, mousePos, Quaternion.identity);
            //panel.transform.parent = parent.transform;

            oneItemPanel.gameObject.SetActive(false);
            twoItemPanel.gameObject.SetActive(true);
            threeItemPanel.gameObject.SetActive(false);

            twoItemPanel.transform.Find("Image1").GetComponent<Image>().sprite = craftedItem.itemOne.uiDisplay;
            twoItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().text = craftedItem.itemOne.itemName + ": " + craftedItem.itemOneAmount;

            if (inventory.IsItemAndAmountInInv(craftedItem.itemOne, craftedItem.itemOneAmount) || hotBar.IsItemAndAmountInInv(craftedItem.itemOne, craftedItem.itemOneAmount))
            {
                twoItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.green;
            }
            else
            {
                twoItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.red;
            }


            twoItemPanel.transform.Find("Image2").GetComponent<Image>().sprite = craftedItem.itemTwo.uiDisplay;
            twoItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().text = craftedItem.itemTwo.itemName + ": " + craftedItem.itemTwoAmount;

            if (inventory.IsItemAndAmountInInv(craftedItem.itemTwo, craftedItem.itemTwoAmount) || hotBar.IsItemAndAmountInInv(craftedItem.itemTwo, craftedItem.itemTwoAmount))
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

        if (craftedItem.threeItem)
        {
            //panel = Instantiate(infoPanel, mousePos, Quaternion.identity);
            //panel.transform.parent = parent.transform;

            oneItemPanel.gameObject.SetActive(false);
            twoItemPanel.gameObject.SetActive(false);
            threeItemPanel.gameObject.SetActive(true);

            threeItemPanel.transform.Find("Image1").GetComponent<Image>().sprite = craftedItem.itemOne.uiDisplay;
            threeItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().text = craftedItem.itemOne.itemName + ": " + craftedItem.itemOneAmount;

            if (inventory.IsItemAndAmountInInv(craftedItem.itemOne, craftedItem.itemOneAmount) || hotBar.IsItemAndAmountInInv(craftedItem.itemOne, craftedItem.itemOneAmount))
            {
                threeItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.green;
            }
            else
            {
                threeItemPanel.transform.Find("Text1").GetComponent<TMP_Text>().color = Color.red;
            }

            threeItemPanel.transform.Find("Image2").GetComponent<Image>().sprite = craftedItem.itemTwo.uiDisplay;
            threeItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().text = craftedItem.itemTwo.itemName + ": " + craftedItem.itemTwoAmount;

            if (inventory.IsItemAndAmountInInv(craftedItem.itemTwo, craftedItem.itemTwoAmount) || hotBar.IsItemAndAmountInInv(craftedItem.itemTwo, craftedItem.itemTwoAmount))
            {
                threeItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().color = Color.green;
            }
            else
            {
                threeItemPanel.transform.Find("Text2").GetComponent<TMP_Text>().color = Color.red;
            }

            threeItemPanel.transform.Find("Image3").GetComponent<Image>().sprite = craftedItem.itemThree.uiDisplay;
            threeItemPanel.transform.Find("Text4").GetComponent<TMP_Text>().text = craftedItem.itemThree.itemName + ": " + craftedItem.itemThreeAmount;

            if (inventory.IsItemAndAmountInInv(craftedItem.itemThree, craftedItem.itemThreeAmount) || hotBar.IsItemAndAmountInInv(craftedItem.itemThree, craftedItem.itemThreeAmount))
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
