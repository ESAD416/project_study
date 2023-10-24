using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(HitSystem))]
public class HitSystemEditor : Editor
{
    private HitSystem adjustHitSystem;

    private SerializedProperty AvatarOnHitEventBegin;
    private SerializedProperty AvatarOnHitEventEnd;
    private SerializedProperty EnemyOnHitEventBegin;
    private SerializedProperty EnemyOnHitEventEnd;

    private SerializedProperty TargetSelectedIndex;
    private string[] targetOptions = new string[] { "Avatar", "Enemy", "Both" };

    private void OnEnable() {
        adjustHitSystem = (HitSystem) target;

        AvatarOnHitEventBegin = serializedObject.FindProperty("AvatarOnHitEventBegin");
        AvatarOnHitEventEnd = serializedObject.FindProperty("AvatarOnHitEventEnd");
        EnemyOnHitEventBegin = serializedObject.FindProperty("EnemyOnHitEventBegin");
        EnemyOnHitEventEnd = serializedObject.FindProperty("EnemyOnHitEventEnd");
        TargetSelectedIndex = serializedObject.FindProperty("TargetSelectedIndex");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        if(adjustHitSystem == null) {
            return;
        }
        
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        TargetSelectedIndex.intValue = EditorGUILayout.Popup("Hit Target", TargetSelectedIndex.intValue, targetOptions);

        UpdateOptionsOfInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateOptionsOfInspector() {
        switch(TargetSelectedIndex.intValue) 
        {
            case 0:
                // Avatar
                EditorGUILayout.PropertyField(AvatarOnHitEventBegin, new GUIContent("Avatar On Hit Event Begin"));
                EditorGUILayout.PropertyField(AvatarOnHitEventEnd, new GUIContent("Avatar On Hit Event End"));

                //EnemyOnHitEventBegin.Dispose();
                UnityEventUtil.RemoveAllListenersWithPersistent(adjustHitSystem.EnemyOnHitEventBegin);
                UnityEventUtil.RemoveAllListenersWithPersistent(adjustHitSystem.EnemyOnHitEventEnd);

                break;
            case 1:
                // Enemy
                EditorGUILayout.PropertyField(EnemyOnHitEventBegin, new GUIContent("Enemy On Hit Event Begin"));
                EditorGUILayout.PropertyField(EnemyOnHitEventEnd, new GUIContent("Enemy On Hit Event End"));

                UnityEventUtil.RemoveAllListenersWithPersistent(adjustHitSystem.AvatarOnHitEventBegin);
                UnityEventUtil.RemoveAllListenersWithPersistent(adjustHitSystem.AvatarOnHitEventEnd);
                
                Debug.Log("Select Enemy");
                break;
            case 2:
                EditorGUILayout.PropertyField(AvatarOnHitEventBegin, new GUIContent("Avatar On Hit Event Begin"));
                EditorGUILayout.PropertyField(AvatarOnHitEventEnd, new GUIContent("Avatar On Hit Event End"));

                EditorGUILayout.PropertyField(EnemyOnHitEventBegin, new GUIContent("Enemy On Hit Event Begin"));
                EditorGUILayout.PropertyField(EnemyOnHitEventEnd, new GUIContent("Enemy On Hit Event End"));
                break;

        }
    }

    
}
