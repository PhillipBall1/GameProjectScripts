using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(ItemObject))]
[CanEditMultipleObjects]
public class ItemEditor : Editor
{
    private bool dataBase;
    private bool inGameLook;
    private bool editItem;
    private bool description;
    private bool data;

    bool change1;
    bool change2;
    bool change3;
    bool change4;
    bool change5;

    override public void OnInspectorGUI()
    {
        serializedObject.Update();

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 14;
        guiStyle.normal.textColor = Color.white;
        guiStyle.richText = true;

        GUIStyle buttonStyle = new GUIStyle();
        buttonStyle.border = EditorStyles.toolbarButton.border;
        buttonStyle.alignment = EditorStyles.toolbarButton.alignment;
        buttonStyle.imagePosition = EditorStyles.toolbarButton.imagePosition;
        buttonStyle.fontSize = 16;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.normal.textColor = Color.white;

        GUILayout.Space(20);

        if (GUILayout.Button("Main", EditorStyles.toolbarButton))
        {
            dataBase = !dataBase;
        }
        if (dataBase)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDataBase"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("despawnTime"));
        }

        GUILayout.Space(20);

        if (GUILayout.Button("In Game Look", EditorStyles.toolbarButton))
        {
            inGameLook = !inGameLook;
        }
        if (inGameLook)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uiDisplay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("characterDisplay"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("floorDisplay"));
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Edit Item", EditorStyles.toolbarButton))
        {
            editItem = !editItem;
        }
        if (editItem)
        {
            EditorGUILayout.LabelField("<b>Main Bools</b>", guiStyle);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stackable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("equippedToHands"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("consumable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("deployable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("armor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("craftable"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("grade"));
            if (serializedObject.FindProperty("stackable").boolValue)
            {
                EditorGUILayout.LabelField("<b>Change Stack Size</b>", guiStyle);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxStackSize"));
            }

            if (serializedObject.FindProperty("equippedToHands").boolValue)
            {
                EditorGUILayout.LabelField("Change Attributes", guiStyle);

                if (!change1) EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponItem"));

                if(!change2) EditorGUILayout.PropertyField(serializedObject.FindProperty("toolItem"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("canBlock"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));


                if (serializedObject.FindProperty("toolItem").boolValue)
                {
                    change1 = true;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponDamage"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeDamage"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("woodDamage"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("toolSwingSpeed"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("toolLength"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("toolTimeBetweenSwingAndHit"));
                    if(!change4) EditorGUILayout.PropertyField(serializedObject.FindProperty("swingDown"));
                    if(!change3) EditorGUILayout.PropertyField(serializedObject.FindProperty("swingRight"));

                    if (serializedObject.FindProperty("swingDown").boolValue)
                    {
                        change3 = true;
                    }
                    else
                    {
                        change3 = false;
                    }
                    if (serializedObject.FindProperty("swingRight").boolValue)
                    {
                        change4 = true;
                    }
                    else
                    {
                        change4 = false;
                    }
                }
                else
                {
                    change1 = false;
                }

                if (serializedObject.FindProperty("weaponItem").boolValue)
                {
                    
                    change2 = true;
                    GUILayout.Space(20);
                    EditorGUILayout.LabelField("<b>Change Gun Attributes</b>", guiStyle);
                    GUILayout.Space(5);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("semiAutomatic"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("singleReload"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponDamage"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("criticalStrikeMultiplier"));

                    EditorGUILayout.LabelField("Ammo Type", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ammoType"));

                    EditorGUILayout.LabelField("Speed Stats", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fireSpeed"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadSpeed"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("aimSpeed"));

                    EditorGUILayout.LabelField("Range/Force", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gunRange"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("impactForce"));

                    EditorGUILayout.LabelField("Ammo", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("currentAmmoAmount"));

                    EditorGUILayout.LabelField("Recoil", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("recoilYStep"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("recoilXPattern"));

                    EditorGUILayout.LabelField("Audio Clips", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gunShot"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gunClick"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("gunReload"));
                }
                else
                {
                    change2 = false;
                }
            }

            if (serializedObject.FindProperty("consumable").boolValue)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("<b>Change Consumable Attributes</b>", guiStyle);
                GUILayout.Space(5);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("foodReturn"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("waterReturn"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("healthReturn"));
            }

            if (serializedObject.FindProperty("deployable").boolValue)
            {
                EditorGUILayout.LabelField("Preview BP", guiStyle);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("deployableBP"));
            }

            if (serializedObject.FindProperty("armor").boolValue)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("<b>Change Armor Attributes</b>", guiStyle);
                GUILayout.Space(5);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeProtection"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("projectileProtection"));
            }
            

            if (serializedObject.FindProperty("craftable").boolValue)
            {
                GUILayout.Space(20);
                EditorGUILayout.LabelField("<b>Change Crafting Attributes</b>", guiStyle);
                GUILayout.Space(5);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("oneItem"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("twoItem"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("threeItem"));

                if (serializedObject.FindProperty("oneItem").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemOne"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemOneAmount"));
                }
                if (serializedObject.FindProperty("twoItem").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemOne"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemOneAmount"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemTwo"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemTwoAmount"));
                }
                if (serializedObject.FindProperty("threeItem").boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemOne"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemOneAmount"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemTwo"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemTwoAmount"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemThree"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemThreeAmount"));
                }
            }
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Description", EditorStyles.toolbarButton))
        {
            description = !description;
        }
        if (description)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Data", EditorStyles.toolbarButton))
        {
            data = !data;
        }
        if (data)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("data"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}

