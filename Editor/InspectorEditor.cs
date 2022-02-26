using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//[CustomEditor(typeof(ItemObject))]
//[CanEditMultipleObjects]
//public class InspectorEditor : Editor
//{
//    
//    override public void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        var itemObject = target as ItemObject;
//        GUIStyle guiStyle = new GUIStyle();
//        guiStyle.fontSize = 22;
//        guiStyle.normal.textColor = Color.white;
//        guiStyle.richText = true;
//
//
//        GUILayout.Space(20);
//        EditorGUILayout.LabelField("<b>Name/Data</b>", guiStyle);
//        GUILayout.Space(5);
//        itemObject.name = GUILayout.TextField(itemObject.name, 25);
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDataBase"));
//
//
//        GUILayout.Space(20);
//        EditorGUILayout.LabelField("<b>In Game Look</b>", guiStyle);
//        GUILayout.Space(5);
//        itemObject.uiDisplay = (Sprite)EditorGUILayout.ObjectField("Inventory Sprite", itemObject.uiDisplay, typeof(Sprite), true);
//        itemObject.characterDisplay = (GameObject)EditorGUILayout.ObjectField("Display on character", itemObject.characterDisplay, typeof(GameObject), true);
//        itemObject.floorDisplay = (GameObject)EditorGUILayout.ObjectField("Display on the ground", itemObject.floorDisplay, typeof(GameObject), true);
//
//
//        GUILayout.Space(20);
//        EditorGUILayout.LabelField("<b>Core Booleans</b>", guiStyle);
//        GUILayout.Space(5);
//        itemObject.stackable = GUILayout.Toggle(itemObject.stackable, "More than 1 in same slot?");
//        itemObject.equiptabale = GUILayout.Toggle(itemObject.equiptabale, "Equiped onto the character?");
//        itemObject.useable = GUILayout.Toggle(itemObject.useable, "Consumed (food, water)?");
//        itemObject.deployable = GUILayout.Toggle(itemObject.deployable, "Placed by character?");
//        itemObject.armor = GUILayout.Toggle(itemObject.armor, "Worn by character?");
//        itemObject.craftable = GUILayout.Toggle(itemObject.craftable, "Crafted by character?");
//        
//
//
//        GUILayout.Space(20);
//        EditorGUILayout.LabelField("<b>Edit Item</b>", guiStyle);
//        GUILayout.Space(5);
//        itemObject.grade = (Grade)EditorGUILayout.EnumPopup("Class of Item: ", itemObject.grade);
//        if (itemObject.stackable)
//        {
//            EditorGUILayout.LabelField("Change Stacksize", EditorStyles.boldLabel);
//            itemObject.maxStackSize = EditorGUILayout.IntField("Stack Size", itemObject.maxStackSize);
//        }
//        else
//        {
//            itemObject.maxStackSize = 1;
//        }
//
//        if (itemObject.equiptabale)
//        {
//            EditorGUILayout.LabelField("Change Weapon/Tool Attributes", EditorStyles.boldLabel);
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponItem"));
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("toolItem"));
//            itemObject.canBlock = GUILayout.Toggle(itemObject.canBlock, "Can Item Block?");
//            itemObject.weaponDamage = EditorGUILayout.IntField("Damage to AI/Players", itemObject.weaponDamage);
//            itemObject.nodeDamage = EditorGUILayout.IntField("Damage to Nodes", itemObject.nodeDamage);
//            itemObject.woodDamage = EditorGUILayout.IntField("Damage to Trees", itemObject.woodDamage);
//            itemObject.criticalStrikeMultiplier = EditorGUILayout.IntField("Critical Damage", itemObject.criticalStrikeMultiplier);
//            EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponAnimator"));
//            if (itemObject.canBlock)
//            {
//                itemObject.type = ItemType.Weapon;
//            }
//        }
//
//        if (itemObject.useable)
//        {
//            GUILayout.Space(20);
//            EditorGUILayout.LabelField("<b>Change Consumable Attributes</b>", guiStyle);
//            GUILayout.Space(5);
//            itemObject.foodReturn = EditorGUILayout.IntField("Food to Replenish", itemObject.foodReturn);
//            itemObject.waterReturn = EditorGUILayout.IntField("Water to Replenish", itemObject.waterReturn);
//            itemObject.healthReturn = EditorGUILayout.IntField("Health to Replenish", itemObject.healthReturn);
//        }
//
//        if (itemObject.deployable)
//        {
//            EditorGUILayout.LabelField("Change BP", EditorStyles.boldLabel);
//            itemObject.deployableBP = (GameObject)EditorGUILayout.ObjectField("", itemObject.deployableBP, typeof(GameObject), true);
//        }
//
//        if (itemObject.armor)
//        {
//            GUILayout.Space(20);
//            EditorGUILayout.LabelField("<b>Change Armor Attributes</b>", guiStyle);
//            GUILayout.Space(5);
//            itemObject.type = (ItemType)EditorGUILayout.EnumPopup("Armor Type", itemObject.type);
//            itemObject.meleeProtection = EditorGUILayout.IntField("Melee Protection", itemObject.meleeProtection);
//            itemObject.projectileProtection = EditorGUILayout.IntField("Projectile Protection", itemObject.projectileProtection);
//        }
//        if (itemObject.weaponItem)
//        {
//            GUILayout.Space(20);
//            EditorGUILayout.LabelField("<b>Change Gun Attributes</b>", guiStyle);
//            GUILayout.Space(5);
//            itemObject.semiAutomatic = GUILayout.Toggle(itemObject.semiAutomatic, "Semi Automatic?");
//            itemObject.singleReload = GUILayout.Toggle(itemObject.singleReload, "Singe Reload?");
//            EditorGUILayout.LabelField("Ammo Type", EditorStyles.boldLabel);
//            itemObject.ammoType = (ItemObject)EditorGUILayout.ObjectField("Ammo Type", itemObject.ammoType, typeof(ItemObject), true);
//            EditorGUILayout.LabelField("Speed Stats", EditorStyles.boldLabel);
//            itemObject.fireSpeed = EditorGUILayout.FloatField("Fire Speed", itemObject.fireSpeed);
//            itemObject.reloadSpeed = EditorGUILayout.FloatField("Reload Speed", itemObject.reloadSpeed);
//            itemObject.aimSpeed = EditorGUILayout.FloatField("Aim Speed", itemObject.aimSpeed);
//            EditorGUILayout.LabelField("Range/Force", EditorStyles.boldLabel);
//            itemObject.gunRange = EditorGUILayout.FloatField("Firing Range", itemObject.gunRange);
//            itemObject.impactForce = EditorGUILayout.FloatField("Impact Force", itemObject.impactForce);
//            EditorGUILayout.LabelField("Ammo", EditorStyles.boldLabel);
//            itemObject.maxAmmo = EditorGUILayout.IntField("Max Ammo", itemObject.maxAmmo);
//            itemObject.currentAmmoAmount = EditorGUILayout.IntField("Start Ammo", itemObject.currentAmmoAmount);
//            EditorGUILayout.LabelField("Recoil", EditorStyles.boldLabel);
//            itemObject.verticalRecoil = EditorGUILayout.FloatField("Vertical Recoil", itemObject.verticalRecoil);
//            itemObject.durationOfRecoil = EditorGUILayout.FloatField("Recoil Duration", itemObject.durationOfRecoil);
//            EditorGUILayout.LabelField("Audio Clips", EditorStyles.boldLabel);
//            itemObject.gunShot = (AudioClip)EditorGUILayout.ObjectField("Gun Shot", itemObject.gunShot, typeof(AudioClip), true);
//            itemObject.gunClick = (AudioClip)EditorGUILayout.ObjectField("Gun Click", itemObject.gunClick, typeof(AudioClip), true);
//            itemObject.gunReload = (AudioClip)EditorGUILayout.ObjectField("Gun Reload", itemObject.gunReload, typeof(AudioClip), true);
//        }
//
//        if (itemObject.craftable)
//        {
//            GUILayout.Space(20);
//            EditorGUILayout.LabelField("<b>Change Crafting Attributes</b>", guiStyle);
//            GUILayout.Space(5);
//            itemObject.oneItem = GUILayout.Toggle(itemObject.oneItem, "One Item Craft?");
//            itemObject.twoItem = GUILayout.Toggle(itemObject.twoItem, "Two Item Craft?");
//            itemObject.threeItem = GUILayout.Toggle(itemObject.threeItem, "Three Item Craft?");
//
//            if (itemObject.oneItem)
//            {
//                itemObject.itemOne = (ItemObject)EditorGUILayout.ObjectField("Craft Item", itemObject.itemOne, typeof(ItemObject), true);
//                itemObject.itemOneAmount = EditorGUILayout.IntField("Craft Item Amount", itemObject.itemOneAmount);
//            }
//            if (itemObject.twoItem)
//            {
//                itemObject.itemOne = (ItemObject)EditorGUILayout.ObjectField("First Craft Item", itemObject.itemOne, typeof(ItemObject), true);
//                itemObject.itemOneAmount = EditorGUILayout.IntField("First Craft Item Amount", itemObject.itemOneAmount);
//                itemObject.itemTwo = (ItemObject)EditorGUILayout.ObjectField("Second Craft Item", itemObject.itemTwo, typeof(ItemObject), true);
//                itemObject.itemTwoAmount = EditorGUILayout.IntField("Second Craft Item Amount", itemObject.itemTwoAmount);
//            }
//            if (itemObject.threeItem)
//            {
//                itemObject.itemOne = (ItemObject)EditorGUILayout.ObjectField("First Craft Item", itemObject.itemOne, typeof(ItemObject), true);
//                itemObject.itemOneAmount = EditorGUILayout.IntField("First Craft Item Amount", itemObject.itemOneAmount);
//                itemObject.itemTwo = (ItemObject)EditorGUILayout.ObjectField("Second Craft Item", itemObject.itemTwo, typeof(ItemObject), true);
//                itemObject.itemTwoAmount = EditorGUILayout.IntField("Second Craft Item Amount", itemObject.itemTwoAmount);
//                itemObject.itemThree = (ItemObject)EditorGUILayout.ObjectField("Third Craft Item", itemObject.itemThree, typeof(ItemObject), true);
//                itemObject.itemThreeAmount = EditorGUILayout.IntField("Third Craft Item Amount", itemObject.itemThreeAmount);
//            }
//        }
//
//        GUILayout.Space(40);
//        EditorGUILayout.LabelField("<b>Description</b>", guiStyle);
//        GUILayout.Space(5);
//        itemObject.description = GUILayout.TextArea(itemObject.description, GUILayout.Height(40));
//        GUILayout.Space(10);
//        EditorGUILayout.LabelField("<b>Data</b>", guiStyle);
//        GUILayout.Space(5);
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("data"));
//        serializedObject.ApplyModifiedProperties();
//    }
//}
