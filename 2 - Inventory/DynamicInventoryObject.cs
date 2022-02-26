using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInventoryObject : MonoBehaviour
{
    public ItemObject itemThatOpensInv;
    public Vector2 canvasPosition;
    [System.NonSerialized]
    public InventoryObject dynamicInv;
    public ItemDatabaseObject dataBase;
    public GameObject[] storageSlots;
    [SerializeField]
    private Inventory Container = new Inventory();
    public InventorySlot[] GetSlots => Container.Slots;
    // Start is called before the first frame update
    void Start()
    {
        dynamicInv = new InventoryObject();
        dynamicInv.type = InterfaceType.Inventory;
        dynamicInv.database = dataBase;
       //for (int i = 0; i < dynamicInv.GetSlots.Length; i++)
       //{
       //    dynamicInv.GetSlots[i].onBeforeUpdated += OnItemRemoved;
       //    dynamicInv.GetSlots[i].onAfterUpdated += OnItemPlaced;
       //}
    }

    public void OnItemPlaced(InventorySlot slot)
    {
        var itemObject = slot.GetItemObject();
        if (itemObject == null) return;
    }

    public void OnItemRemoved(InventorySlot slot)
    {
        var itemObject = slot.GetItemObject();
        if (itemObject == null) return;
    }
}
