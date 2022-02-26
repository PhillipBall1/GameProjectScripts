using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject
{
    public List<ItemObject> itemObjects;

    public void OnValidate()
    {
        for (int i = 0; i < itemObjects.Count; i++)
        {
            if(itemObjects[i] == null)
            {
                itemObjects.RemoveAt(i);
            }
            else
            {
                itemObjects[i].data.Id = i;
            }
        }
    }

}
