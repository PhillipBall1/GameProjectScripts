using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System;

[RequireComponent(typeof(EventTrigger))]
public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    private InventoryObject _previousInventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    private GameObject player;
    public PlayerCraft playerC;
    private ItemObject currentItem;
    private int currentItemAmount;

    public Canvas parentCanvas;
    public GameObject infoPanel;

    private GameObject currentPanel;

    private Vector2 movePos;

    public void OnEnable()
    {
        if (gameObject.activeInHierarchy)
        {
            CreateSlots();
            for (int i = 0; i < inventory.GetSlots.Length; i++)
            {
                inventory.GetSlots[i].parent = this;
                inventory.GetSlots[i].onAfterUpdated += OnSlotUpdate;
            }


            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        }
    }

    public abstract void CreateSlots();

    public void UpdateInventoryLinks()
    {
        int i = 0;
        foreach (var key in slotsOnInterface.Keys.ToList())
        {
            slotsOnInterface[key] = inventory.GetSlots[i];
            i++;
        }
    }

    public void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.item.Id <= -1)
        {
            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().sprite = null;
            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
        }
        else
        {
            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().sprite = slot.GetItemObject().uiDisplay;
            slot.slotDisplay.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount == 1 ? string.Empty : slot.amount.ToString("n0");
        }
    }

    public void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(inventory != null)
        {
            if (_previousInventory != inventory)
            {
                UpdateInventoryLinks();
            }
            _previousInventory = inventory;

        }
        Vector2 f = Input.mousePosition;
        Vector2 g = new Vector2(f.x, f.y + 200);
        movePos = g;
        
    }

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (!trigger) { Debug.LogWarning("No EventTrigger component found!"); return; }
        var eventTrigger = new EventTrigger.Entry {eventID = type};
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
        
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
        if (Input.GetMouseButtonDown(2))
        {
            InventorySlot item = slotsOnInterface[obj];
            if (item.GetItemObject().stackable && item.amount > 1)
            {

            }
        }
    }

    public void OnClick(GameObject obj)
    {
        GameObject[] slotPanels = GameObject.FindGameObjectsWithTag("SlotPanelActive");
        foreach (GameObject slotPanel in slotPanels)
        {
            slotPanel.gameObject.SetActive(false);
        }

        obj.transform.GetChild(2).gameObject.SetActive(true);

        OpenInfoPanel(MouseData.interfaceMouseIsOver.slotsOnInterface, obj);
    }

    private GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].GetItemObject().uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    private void DropItem(InventorySlot slot)
    {
        if(currentItem != null)
        {
            inventory.Drop(currentItem, currentItemAmount);
            slot.RemoveItem();
        }
    }

    private void SetItem(InventorySlot slot)
    {
        currentItem = slot.GetItemObject();
        currentItemAmount = slot.amount;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItemBeingDragged);

        if (MouseData.interfaceMouseIsOver == null)
        {
            DropItem(slotsOnInterface[obj]);
            return;
        }
        else
        {
            GameObject[] slotPanels = GameObject.FindGameObjectsWithTag("SlotPanelActive");
            foreach (GameObject slotPanel in slotPanels)
            {
                slotPanel.gameObject.SetActive(false);
            }
            MouseData.slotHoveredOver.transform.GetChild(2).gameObject.SetActive(true);
            if (MouseData.interfaceMouseIsOver.slotsOnInterface != null)
            {
                OpenInfoPanel(MouseData.interfaceMouseIsOver.slotsOnInterface, MouseData.slotHoveredOver);
            }
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            InventorySlot slotThatWasDragged = slotsOnInterface[obj];
            if (mouseHoverSlotData != slotThatWasDragged)
            {
                if (mouseHoverSlotData.GetItemObject() == slotThatWasDragged.GetItemObject())
                {
                    InventorySlot item1 = mouseHoverSlotData;
                    InventorySlot item2 = slotThatWasDragged;
                    InventorySlot item2Slot = new InventorySlot(item2.item, item2.amount);
                    int max = item1.GetItemObject().maxStackSize;
                    Item mainItem = item1.item;
                    int fullAmount = item1.amount + item2Slot.amount;
                    if (fullAmount > max)
                    {
                        int extra = fullAmount - max;
                        item1.UpdateSlot(mainItem, max);
                        item2.UpdateSlot(mainItem, extra);
                    }
                    else
                    {
                        item1.UpdateSlot(mainItem, fullAmount);
                        item2.RemoveItem();
                    }
                }
                else
                {
                    inventory.SwapItems(slotThatWasDragged, mouseHoverSlotData);
                }
            }
        }
    }

    public void OnDrag(GameObject obj)
    {
        SetItem(slotsOnInterface[obj]);
        if (MouseData.tempItemBeingDragged != null)
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    private void OpenInfoPanel(Dictionary<GameObject, InventorySlot> slotsOnInterfaces, GameObject obj)
    {
        InfoPanel g = FindObjectOfType<InfoPanel>();
        if (g)
        {
            Destroy(g.gameObject);
        }

        if (slotsOnInterfaces[obj].GetItemObject() != null && infoPanel != null)
        {
            currentPanel = Instantiate(infoPanel, parentCanvas.transform);

            Transform titleChild = currentPanel.transform.Find("TitleText");
            titleChild.GetComponent<TMP_Text>().text = slotsOnInterfaces[obj].GetItemObject().itemName;

            Transform imageChild = currentPanel.transform.Find("Image");
            imageChild.GetComponent<Image>().sprite = slotsOnInterfaces[obj].GetItemObject().uiDisplay;

            Transform descriptionChild = currentPanel.transform.Find("DescriptionText");
            descriptionChild.GetComponent<TMP_Text>().text = slotsOnInterfaces[obj].GetItemObject().description;

            Transform craftableTitle = currentPanel.transform.Find("CraftableText");
            if (slotsOnInterfaces[obj].GetItemObject().craftable)
            {
                craftableTitle.GetComponent<TMP_Text>().text = "Yes";
            }
            else
            {
                craftableTitle.GetComponent<TMP_Text>().text = "No";
            }

            Transform detailTitle = currentPanel.transform.Find("DetailTitle");
            Transform detailText = currentPanel.transform.Find("DetailText");
            if (slotsOnInterfaces[obj].GetItemObject().equippedToHands)
            {
                detailTitle.GetComponent<TMP_Text>().text = "Damage";
                detailText.gameObject.SetActive(true);
                detailText.GetComponent<TMP_Text>().text = "Trees:  " + slotsOnInterfaces[obj].GetItemObject().woodDamage + "  " +
                                                            "Nodes: " + slotsOnInterfaces[obj].GetItemObject().nodeDamage + "  " +
                                                            "Creatures: " + slotsOnInterfaces[obj].GetItemObject().weaponDamage;
            }
            else if (slotsOnInterfaces[obj].GetItemObject().consumable)
            {
                detailTitle.GetComponent<TMP_Text>().text = "Restoration";
                detailText.GetComponent<TMP_Text>().text = "Food:  " + slotsOnInterfaces[obj].GetItemObject().foodReturn + "  " +
                                                            "Water: " + slotsOnInterfaces[obj].GetItemObject().waterReturn + "  " +
                                                            "Health: " + slotsOnInterfaces[obj].GetItemObject().healthReturn;
            }
            else if (slotsOnInterfaces[obj].GetItemObject().equippedToHands)
            {
                detailTitle.GetComponent<TMP_Text>().text = "Armor";
                detailText.GetComponent<TMP_Text>().text = "Melee:  " + slotsOnInterfaces[obj].GetItemObject().meleeProtection + "  " +
                                                            "Projectile: " + slotsOnInterfaces[obj].GetItemObject().projectileProtection;
            }
            else
            {
                detailTitle.GetComponent<TMP_Text>().text = "";
                detailText.GetComponent<TMP_Text>().text = "";
            }
        }
    }
}
