using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
[CustomEditor(typeof(GlobalAIController))]
[CanEditMultipleObjects]
public class AIEditor : Editor
{
    
    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 22;
        guiStyle.normal.textColor = Color.white;
        guiStyle.richText = true;

        //--------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(20);
        EditorGUILayout.LabelField("<b>Base</b>", guiStyle);
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("aiName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animalType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("health"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));


        GUILayout.Space(20);
        EditorGUILayout.LabelField("<b>Audio Clips</b>", guiStyle);
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("specialAudioClip"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hurtAudioClip"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackAudioClip"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("deadAudioClip"));


        //--------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(20);
        EditorGUILayout.LabelField("<b>If Animations Exist</b>", guiStyle);
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("willGetHurt"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("willGetBlocked"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hasSpecial"));
        //--------------------------------------------------------------------------------------------------------------------
        GUILayout.Space(20);
        EditorGUILayout.LabelField("<b>Values</b>", guiStyle);
        GUILayout.Space(5);

        if (serializedObject.FindProperty("animalType").enumValueIndex == 2)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("multiplyBy"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceToRun"));
        }

        if (serializedObject.FindProperty("animalType").enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackAnimLength"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceToChase"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceToAttack"));
        }
        if (serializedObject.FindProperty("animalType").enumValueIndex == 1)
        {

        }
        serializedObject.ApplyModifiedProperties();
    }
}

#endif
