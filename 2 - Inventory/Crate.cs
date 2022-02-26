using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private InventoryObject crateInv;
    public ItemDatabaseObject dataBase;
    // Start is called before the first frame update
    void Start()
    {
        crateInv = new InventoryObject();
        crateInv.type = InterfaceType.Inventory;
        crateInv.database = dataBase;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
