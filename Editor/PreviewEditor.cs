using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Preview))]
public class PreviewEditor : Editor
{
    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 22;
        guiStyle.normal.textColor = Color.white;
        guiStyle.richText = true;

        GUIStyle guiStyle2 = new GUIStyle();
        guiStyle2.fontSize = 16;
        guiStyle2.normal.textColor = Color.white;
        guiStyle2.richText = true;

        //--------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(20);
        EditorGUILayout.LabelField("<b>Prefab To Spawn</b>", guiStyle);
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));

        GUILayout.Space(20);
        EditorGUILayout.LabelField("<b>Main Bools</b>", guiStyle);
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("flatRequired"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isFreeDeployable"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("snaps"));

        if (serializedObject.FindProperty("snaps").boolValue)
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("What does it snap to?", guiStyle2);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("snappingToWalls"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("snapsToFoundations"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("snapsToCeilings"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("snapsToDoorWays"));
        }
        if (serializedObject.FindProperty("flatRequired").boolValue)
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Transforms under corner points", guiStyle2);
            GUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck1"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck2"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck3"));

        }
        if (serializedObject.FindProperty("isFreeDeployable").boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck"));
        }


        serializedObject.ApplyModifiedProperties();
    }
}