using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    //public int MAX_ITEMS;
    [SerializeField]
    private Inventory Container = new Inventory();
    public InventorySlot[] GetSlots => Container.Slots;

    public bool AddItem(Item item, int amount)
    {
        ItemObject mainItem = database.itemObjects[item.Id];
        InventorySlot slot = FindUnmaxedSlot(item, mainItem.maxStackSize);
        if (!mainItem.stackable || slot == null)
        {
            if(EmptySlotCount > 0)
            {
                GetEmptySlot().UpdateSlot(item, amount);
                return true;
            }
            if (EmptySlotCount <= 0)
            {
                Drop(mainItem, 1);
            }
        }
        if (mainItem.stackable)
        {
            bool check;
            if(slot == null)
            {
                check = true;
            }
            else
            {
                slot.AddAmount(amount);
                if(slot.amount > mainItem.maxStackSize)
                {
                    int extra = slot.amount - mainItem.maxStackSize;
                    slot.amount = mainItem.maxStackSize;
                    slot.UpdateSlot(slot.item, slot.amount);
                    if(EmptySlotCount <= 0)
                    {
                        Drop(mainItem, extra);
                    }
                    else 
                    {
                        GetEmptySlot().UpdateSlot(item, extra);
                        return true;
                    }
                }
                return true;
            }

            if (check)
            {
                InventorySlot newSlot = FindSlotContainingItem(item);
                newSlot.AddAmount(amount);
                int extra = newSlot.amount - mainItem.maxStackSize;
                if (EmptySlotCount <= 0)
                {
                    Drop(mainItem, extra);
                }
                else
                {
                    InventorySlot check2 = FindUnmaxedSlot(item, mainItem.maxStackSize);

                    if(check2 == null)
                    {
                        GetEmptySlot().UpdateSlot(item, extra);
                        return true;
                    }
                    else
                    {
                        check2.AddAmount(amount);
                    }
                }
            }
        }
        return false;
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

    public InventorySlot FindUnmaxedSlot(Item item, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == item.Id && GetSlots[i].amount < amount)
            {
                return GetSlots[i];
            }
        }
        return null;
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

