using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{

    public string itemName;
    public int despawnTime;
    public float recoilYStep;
    public float[] recoilXPattern;
    public Sprite uiDisplay;
    public GameObject characterDisplay;
    public GameObject floorDisplay;
    public int maxStackSize = 1;
    public ItemDatabaseObject itemDataBase;

    public bool stackable;
    public bool equippedToHands;
    public bool consumable;
    public bool deployable;
    public bool armor;
    public bool craftable;


    public bool semiAutomatic;
    public bool singleReload;

    public ItemObject ammoType;

    public float toolSwingSpeed;
    [Tooltip("Distance at which tool can hit")]
    public float toolLength;
    [Tooltip("To find this do '1.7 times the animator frame level, which is 0-60' ")]
    public float toolTimeBetweenSwingAndHit;

    [Tooltip("Higher = Faster Firing")]
    public float fireSpeed;
    public float gunRange;
    public int criticalStrikeMultiplier;
    public float impactForce;
    public int maxAmmo;
    public int currentAmmoAmount;
    public float reloadSpeed;
    public float aimSpeed;
    public AudioClip gunShot;
    public AudioClip gunClick;
    public AudioClip gunReload;

    public bool weaponItem;
    public bool toolItem;
    public bool swingDown;
    public bool swingRight;
    public bool canBlock;
    public int weaponDamage;
    public int nodeDamage;
    public int woodDamage;
    public ItemType type;
    public Grade grade;
    public AnimatorOverrideController animator;

    public int foodReturn;
    public int waterReturn;
    public int healthReturn;

    public GameObject deployableBP;

    public int meleeProtection;
    public int projectileProtection;

    public bool oneItem;
    public bool twoItem;
    public bool threeItem;

    public ItemObject itemOne;
    public int itemOneAmount;
    public ItemObject itemTwo;
    public int itemTwoAmount;
    public ItemObject itemThree;
    public int itemThreeAmount;



    [TextArea(15, 20)] public string description;
    public Item data = new Item();

    public List<string> boneNames = new List<string>();

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

    private void OnValidate()
    {
        if (equippedToHands)
        {
            floorDisplay.transform.tag = "Item";
        }
        if (!itemDataBase.itemObjects.Contains(this))
        {
            itemDataBase.itemObjects.Add(this);
        }
        if (craftable)
        {

        }
        
        boneNames.Clear();
        if (characterDisplay == null) 
            return;
        if(!characterDisplay.GetComponent<SkinnedMeshRenderer>())
            return;

        var renderer = characterDisplay.GetComponent<SkinnedMeshRenderer>();
        var bones = renderer.bones;

        foreach (var t in bones)
        {
            boneNames.Add(t.name);
        }
    }

    
}