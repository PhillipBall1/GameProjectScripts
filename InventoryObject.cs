using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    [SerializeField]
    private Inventory Container = new Inventory();
    public InventorySlot[] GetSlots => Container.Slots;

    private bool debug = true;

    public bool AddItem(Item item, int amount)
    {
        DynamicInterface[] dynamicInterfaces = FindObjectsOfType<DynamicInterface>();
        List<InventoryObject> invObjects = new List<InventoryObject>();
        foreach (DynamicInterface dynamicInterface in dynamicInterfaces)
        {
            invObjects.Add(dynamicInterface.inventory);
        }


        int excess = -1;
        //First Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.type == InterfaceType.Equipment) continue;

            if (!invObj.FindSlotContainingItemBool(item)) continue;

            if (invObj.CanPlaceInSlot(item, amount)) break;

            InventorySlot lowestSlot = FindLowestSlot(item, invObj);

            if(lowestSlot.amount == database.itemObjects[item.Id].maxStackSize) continue;

            if(debug) Debug.Log("First");

            int addedAmounts = lowestSlot.amount + amount;

            excess = addedAmounts - database.itemObjects[item.Id].maxStackSize;

            lowestSlot.amount = database.itemObjects[item.Id].maxStackSize;

            while (true)
            {
                InventorySlot nextSlot = FindLowestSlot(item, invObj);

                if(nextSlot.amount == database.itemObjects[item.Id].maxStackSize) break; 

                int added = excess + nextSlot.amount;

                if(added > database.itemObjects[item.Id].maxStackSize)
                {
                    excess = added - database.itemObjects[item.Id].maxStackSize;
                    continue;
                }
                else
                {
                    nextSlot.UpdateSlot(item, added);
                }
            }
        }


        if(excess == 0)
        {
            return true;
        }
        if(excess != -1)
        {
            amount = excess;
        }

        //Second Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.type == InterfaceType.Equipment) continue;

            if (!invObj.FindSlotContainingItemBool(item)) continue;

            InventorySlot lowestSlot = FindLowestSlot(item, invObj);

            if (amount + lowestSlot.amount > database.itemObjects[item.Id].maxStackSize) continue;

            if (debug) Debug.Log("Second");

            lowestSlot.UpdateSlot(item, amount + lowestSlot.amount);

            return true;
        }

        //Third Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.type == InterfaceType.Equipment) continue;

            if (invObj.EmptySlotCount <= 0) continue;

            if (debug) Debug.Log("Third");

            GetEmptySlotsWithKey(invObj).UpdateSlot(item, amount);

            return true;
        }

        //Fourth Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.type == InterfaceType.Equipment) continue;

            if (invObj.EmptySlotCount <= 0) continue;

            if (invObj.FindSlotContainingItem(item) == null) continue;

            if (debug) Debug.Log("Fourth");

            Drop(database.itemObjects[item.Id], amount);

            return false;
        }
        return false;
    }

    public bool AddItemSpecificInventoryObject(Item item, int amount, string inventoryObj)
    {
        DynamicInterface[] dynamicInterfaces = FindObjectsOfType<DynamicInterface>();
        List<InventoryObject> invObjects = new List<InventoryObject>();
        foreach (DynamicInterface dynamicInterface in dynamicInterfaces)
        {
            invObjects.Add(dynamicInterface.inventory);
        }


        int excess = -1;
        //First Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.name != inventoryObj) continue;

            if (!invObj.FindSlotContainingItemBool(item)) continue;

            if (invObj.CanPlaceInSlot(item, amount)) break;

            InventorySlot lowestSlot = FindLowestSlot(item, invObj);

            if (lowestSlot.amount == database.itemObjects[item.Id].maxStackSize) continue;

            Debug.Log("First");

            int addedAmounts = lowestSlot.amount + amount;

            excess = addedAmounts - database.itemObjects[item.Id].maxStackSize;

            lowestSlot.amount = database.itemObjects[item.Id].maxStackSize;

            while (excess > 0)
            {
                InventorySlot nextSlot = FindLowestSlot(item, invObj);

                if (nextSlot.amount == database.itemObjects[item.Id].maxStackSize) break;

                int added = excess + nextSlot.amount;

                if (added > database.itemObjects[item.Id].maxStackSize)
                {
                    excess = added - database.itemObjects[item.Id].maxStackSize;
                    continue;
                }
                else
                {
                    nextSlot.UpdateSlot(item, added);
                    excess = 0;
                }
            }
        }

        Debug.Log(excess);

        if (excess == 0)
        {
            return true;
        }
        if (excess != -1)
        {
            amount = excess;
        }

        //Second Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.name != inventoryObj) continue;

            if (!invObj.FindSlotContainingItemBool(item)) continue;

            InventorySlot lowestSlot = FindLowestSlot(item, invObj);

            if (amount + lowestSlot.amount > database.itemObjects[item.Id].maxStackSize) continue;

            Debug.Log("Second");

            lowestSlot.UpdateSlot(item, amount + lowestSlot.amount);

            return true;
        }

        //Third Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.name != inventoryObj) continue;

            if (invObj.EmptySlotCount <= 0) continue;

            Debug.Log("Third");

            GetEmptySlotsWithKey(invObj).UpdateSlot(item, amount);

            return true;
        }

        //Fourth Loop 
        foreach (InventoryObject invObj in invObjects)
        {
            if (invObj.name != inventoryObj) continue;

            if (invObj.EmptySlotCount <= 0) continue;

            if (invObj.FindSlotContainingItem(item) == null) continue;

            Debug.Log("Fourth");

            Drop(database.itemObjects[item.Id], amount);

            return false;
        }
        return false;
    }

    private InventorySlot FindLowestSlot(Item item, InventoryObject key)
    {
        List<InventorySlot> possibleSlots = PossibleSlotsAny(item, key);
        InventorySlot previousSlot = null;
        InventorySlot lowestSlot = null;
        for (int i = 0; i < possibleSlots.Count; i++)
        {
            InventorySlot currentSlot = possibleSlots[i];
            if (previousSlot == null)
            {
                lowestSlot = currentSlot;
            }
            else
            {
                if (currentSlot.amount < previousSlot.amount)
                {
                    lowestSlot = currentSlot;
                }
            }
            previousSlot = currentSlot;
        }
        return lowestSlot;
    }

    public void TransferItem(InventorySlot slot, string nameToGoTo)
    {
        DynamicInterface[] dynamicInterfaces = FindObjectsOfType<DynamicInterface>();
        Dictionary<InventoryObject, DynamicInterface> inventoryObjects = new Dictionary<InventoryObject, DynamicInterface>();
        foreach (DynamicInterface dynamicInterface in dynamicInterfaces)
        {
            inventoryObjects.Add(dynamicInterface.inventory, dynamicInterface);
        }

        foreach (KeyValuePair<InventoryObject, DynamicInterface> invObject in inventoryObjects)
        {
            if(invObject.Value.name == nameToGoTo)
            {
                invObject.Key.AddItem(slot.item, slot.amount);
                slot.RemoveItem();
            }
        }
    }

    public void Drop(ItemObject mainItem, int extra)
    {
        DropPoint dropPoint = FindObjectOfType<DropPoint>();
        Vector3 spawnPos = dropPoint.transform.position; 

        GameObject droppedItem = Instantiate(mainItem.floorDisplay, spawnPos, Quaternion.identity);
        if (extra > 1000)
        {
            droppedItem.transform.localScale = new Vector3(extra / 1300, extra / 1300, extra / 1300);
        }
        droppedItem.GetComponent<GroundItem>().amount = extra;
        droppedItem.GetComponent<GroundItem>().item = mainItem;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindSlotContainingItem(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public bool FindSlotContainingItemBool(Item item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanPlaceInSlot(Item item, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id && amount + GetSlots[i].amount <= database.itemObjects[item.Id].maxStackSize)
            {
                return true;
                //return GetSlots[i];
            }
        }
        return false;
        //return null;
    }

    public List<InventorySlot> PossibleSlots(Item item)
    {
        List<InventorySlot> slotList = new List<InventorySlot>();
        
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id)
            {
                slotList.Add(GetSlots[i]);
            }
        }
        
        return slotList;
    }

    public List<InventorySlot> PossibleSlotsAny(Item item, InventoryObject inv)
    {
        List<InventorySlot> slotList = new List<InventorySlot>();

        for (int i = 0; i < inv.GetSlots.Length; i++)
        {
            if (inv.GetSlots[i].item.Id == item.Id)
            {
                slotList.Add(inv.GetSlots[i]);
            }
        }

        return slotList;
    }

    public InventorySlot FindItemToRemove(Item item, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id && GetSlots[i].amount >= amount)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public InventorySlot FindStackToRemove(Item item, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id && GetSlots[i].amount == amount)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public bool IsItemInInventory(ItemObject item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.data.Id)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsItemAndAmountInInv(ItemObject item, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.data.Id && GetSlots[i].amount >= amount)
            {
                return true;
            }
        }
        return false;
    }

    public bool RemoveItemAndAmount(ItemObject itemToRemove, int amountOfItemToRemove)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == itemToRemove.data.Id && GetSlots[i].amount >= amountOfItemToRemove)
            {
                Item item = new Item(itemToRemove);
                GetSlots[i].UpdateSlot(item, GetSlots[i].amount - amountOfItemToRemove);
                if (GetSlots[i].amount <= 0)
                {
                    GetSlots[i].RemoveItem();
                }
                return true;
            }
        }
        return false;
    }

    public int AmountInSlot(ItemObject item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.data.Id)
            {
                return GetSlots[i].amount;
            }
        }
        return 0;
    }
    
    
    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public InventorySlot GetEmptySlotsWithKey(InventoryObject key)
    {
        for (int i = 0; i < key.GetSlots.Length; i++)
        {
            if (key.GetSlots[i].item.Id <= -1)
            {
                return key.GetSlots[i];
            }
        }
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2)
            return;
        if (item2.CanPlaceInSlot(item1.GetItemObject()) && item1.CanPlaceInSlot(item2.GetItemObject()))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }


    [ContextMenu("Save")]
    public void Save()
    {
        #region Optional Save
        //string saveData = JsonUtility.ToJson(Container, true);
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //bf.Serialize(file, saveData);
        //file.Close();
        #endregion

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
        PlayerPrefs.SetInt("Saved", 1);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            #region Optional Load
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            //JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), Container);
            //file.Close();
            #endregion

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }

}

