using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Detector_Overlap2D))]
public class DetectorEditor : Editor
{
    private Detector_Overlap2D adjustOverlap2D;

    // Overlap 所需要定義的全域變數
    private SerializedProperty Radius;
    private SerializedProperty BoxSize;
    private SerializedProperty Angle;
    private SerializedProperty Origin;
    private SerializedProperty Diagonal;
    private SerializedProperty DetectSelectedIndex;
    private SerializedProperty AreaSelectedIndex;

    private string[] detectOptions = new string[] { "Single", "Mutiple" };
    private string[] areaOptions = new string[] { "Circular", "Box", "Rectangular", "Point"};

    private void OnEnable() {
        adjustOverlap2D = (Detector_Overlap2D)target;

        Radius = serializedObject.FindProperty("Radius");
        BoxSize = serializedObject.FindProperty("BoxSize");
        Angle = serializedObject.FindProperty("Angle");
        Origin = serializedObject.FindProperty("Origin");
        Diagonal = serializedObject.FindProperty("Diagonal");
        DetectSelectedIndex = serializedObject.FindProperty("DetectSelectedIndex");
        AreaSelectedIndex = serializedObject.FindProperty("AreaSelectedIndex");
        
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        if(adjustOverlap2D == null) {
            return;
        }
        
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        DetectSelectedIndex.intValue = EditorGUILayout.Popup("Detect Mode", DetectSelectedIndex.intValue, detectOptions);
        AreaSelectedIndex.intValue = EditorGUILayout.Popup("Area Mode", AreaSelectedIndex.intValue, areaOptions);

        //Debug.Log("OnInspectorGUI: "+detectSelectedIndex.ToString()+areaSelectedIndex.ToString());
        UpdateOptionsOfInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateOptionsOfInspector() {
        switch(DetectSelectedIndex.intValue.ToString()+AreaSelectedIndex.intValue.ToString()) 
        {
            case "00":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);
               
                break;
            case "01":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);
                
                break;
            case "02":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
            case "10":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);
                
                break;
            case "11":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);
                
                break;
            case "12":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
        }
    }

    private void OnSceneGUI() {
        if(adjustOverlap2D == null) {
            return;
        }

        // 開始畫出檢測區域
        Handles.color = adjustOverlap2D.HandlesColor;
        switch(AreaSelectedIndex.intValue)
        {
            case 0:
                Handles.DrawWireDisc(adjustOverlap2D.transform.position, Vector3.back, Radius.floatValue);
                break;
            case 1:
                // 计算旋转矩阵
                //Handles.matrix  = Matrix4x4.TRS(adjustOverlap2D.transform.position, Quaternion.Euler(0, 0, Angle.floatValue), Vector3.one);
                // 绘制旋转后的线框立方体
                Handles.DrawWireCube(adjustOverlap2D.transform.position, BoxSize.vector2Value);
                break;
            case 2:
                Handles.DrawWireCube((Origin.vector2Value + Diagonal.vector2Value) / 2f, new Vector2(Mathf.Abs(Diagonal.vector2Value.x - Origin.vector2Value.x), Mathf.Abs(Diagonal.vector2Value.y - Origin.vector2Value.y)));
                break;
            case 3:
                Handles.Label(adjustOverlap2D.transform.position, adjustOverlap2D.transform.position.ToString());
                Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), adjustOverlap2D.transform.position, Quaternion.identity, 0.05f, EventType.Repaint);
                break;
            
        }
    }
}

[CustomEditor(typeof(HitBox_Overlap2D))]
public class HitboxEditor : Editor {
    private HitBox_Overlap2D adjustOverlap2D;

    // Overlap 所需要定義的全域變數
    private SerializedProperty Radius;
    private SerializedProperty BoxSize;
    private SerializedProperty Angle;
    private SerializedProperty Origin;
    private SerializedProperty Diagonal;
    private SerializedProperty DetectSelectedIndex;
    private SerializedProperty AreaSelectedIndex;
    
    private string[] detectOptions = new string[] { "Single", "Mutiple" };
    private string[] areaOptions = new string[] { "Circular", "Box", "Rectangular", "Point"};

    private void OnEnable() {
        adjustOverlap2D = (HitBox_Overlap2D)target;

        Radius = serializedObject.FindProperty("Radius");
        BoxSize = serializedObject.FindProperty("BoxSize");
        Angle = serializedObject.FindProperty("Angle");
        Origin = serializedObject.FindProperty("Origin");
        Diagonal = serializedObject.FindProperty("Diagonal");
        DetectSelectedIndex = serializedObject.FindProperty("DetectSelectedIndex");
        AreaSelectedIndex = serializedObject.FindProperty("AreaSelectedIndex");
        
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        if(adjustOverlap2D == null) {
            return;
        }
        
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        DetectSelectedIndex.intValue = EditorGUILayout.Popup("Detect Mode", DetectSelectedIndex.intValue, detectOptions);
        AreaSelectedIndex.intValue = EditorGUILayout.Popup("Area Mode", AreaSelectedIndex.intValue, areaOptions);

        //Debug.Log("OnInspectorGUI: "+detectSelectedIndex.ToString()+areaSelectedIndex.ToString());
        UpdateOptionsOfInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateOptionsOfInspector() {
        switch(DetectSelectedIndex.intValue.ToString()+AreaSelectedIndex.intValue.ToString()) 
        {
            case "00":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);

                break;
            case "01":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);

                break;
            case "02":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
            case "10":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);
                
                break;
            case "11":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);
                
                break;
            case "12":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
        }
    }

    private void OnSceneGUI() {
        if(adjustOverlap2D == null) {
            return;
        }

        // 開始畫出檢測區域
        Handles.color = adjustOverlap2D.HandlesColor;
        switch(AreaSelectedIndex.intValue) 
        {
            case 0:
                Handles.DrawWireDisc(adjustOverlap2D.transform.position, Vector3.back, Radius.floatValue);
                break;
            case 1:
                // 计算旋转矩阵
                //Handles.matrix  = Matrix4x4.TRS(adjustOverlap2D.transform.position, Quaternion.Euler(0, 0, Angle.floatValue), Vector3.one);
                // 绘制旋转后的线框立方体
                Handles.DrawWireCube(adjustOverlap2D.transform.position, BoxSize.vector2Value);
                break;
            case 2:
                Handles.DrawWireCube((Origin.vector2Value + Diagonal.vector2Value) / 2f, new Vector2(Mathf.Abs(Diagonal.vector2Value.x - Origin.vector2Value.x), Mathf.Abs(Diagonal.vector2Value.y - Origin.vector2Value.y)));
                break;
            case 3:
                Handles.Label(adjustOverlap2D.transform.position, adjustOverlap2D.transform.position.ToString());
                Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), adjustOverlap2D.transform.position, Quaternion.identity, 0.05f, EventType.Repaint);
                break;
            
        }
    }
}


[CustomEditor(typeof(Detector_EnemyPatrol))]
public class EnemyPatrolDetectorEditor : Editor {
    private Detector_EnemyPatrol adjustOverlap2D;

    // Overlap 所需要定義的全域變數
    private SerializedProperty Radius;
    private SerializedProperty BoxSize;
    private SerializedProperty Angle;
    private SerializedProperty Origin;
    private SerializedProperty Diagonal;
    private SerializedProperty DetectSelectedIndex;
    private SerializedProperty AreaSelectedIndex;
    
    private string[] detectOptions = new string[] { "Single", "Mutiple" };
    private string[] areaOptions = new string[] { "Circular", "Box", "Rectangular", "Point"};

    private void OnEnable() {
        adjustOverlap2D = (Detector_EnemyPatrol)target;

        Radius = serializedObject.FindProperty("Radius");
        BoxSize = serializedObject.FindProperty("BoxSize");
        Angle = serializedObject.FindProperty("Angle");
        Origin = serializedObject.FindProperty("Origin");
        Diagonal = serializedObject.FindProperty("Diagonal");
        DetectSelectedIndex = serializedObject.FindProperty("DetectSelectedIndex");
        AreaSelectedIndex = serializedObject.FindProperty("AreaSelectedIndex");
        
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        if(adjustOverlap2D == null) {
            return;
        }
        
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        DetectSelectedIndex.intValue = EditorGUILayout.Popup("Detect Mode", DetectSelectedIndex.intValue, detectOptions);
        AreaSelectedIndex.intValue = EditorGUILayout.Popup("Area Mode", AreaSelectedIndex.intValue, areaOptions);

        //Debug.Log("OnInspectorGUI: "+detectSelectedIndex.ToString()+areaSelectedIndex.ToString());
        UpdateOptionsOfInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateOptionsOfInspector() {
        switch(DetectSelectedIndex.intValue.ToString()+AreaSelectedIndex.intValue.ToString()) 
        {
            case "00":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);

                break;
            case "01":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);

                break;
            case "02":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
            case "10":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);
                
                break;
            case "11":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);
                
                break;
            case "12":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
        }
    }

    private void OnSceneGUI() {
        if(adjustOverlap2D == null) {
            return;
        }

        // 開始畫出檢測區域
        Handles.color = adjustOverlap2D.HandlesColor;
        switch(AreaSelectedIndex.intValue) 
        {
            case 0:
                Handles.DrawWireDisc(adjustOverlap2D.transform.position, Vector3.back, Radius.floatValue);
                break;
            case 1:
                // 计算旋转矩阵
                //Handles.matrix  = Matrix4x4.TRS(adjustOverlap2D.transform.position, Quaternion.Euler(0, 0, Angle.floatValue), Vector3.one);
                // 绘制旋转后的线框立方体
                Handles.DrawWireCube(adjustOverlap2D.transform.position, BoxSize.vector2Value);
                break;
            case 2:
                Handles.DrawWireCube((Origin.vector2Value + Diagonal.vector2Value) / 2f, new Vector2(Mathf.Abs(Diagonal.vector2Value.x - Origin.vector2Value.x), Mathf.Abs(Diagonal.vector2Value.y - Origin.vector2Value.y)));
                break;
            case 3:
                Handles.Label(adjustOverlap2D.transform.position, adjustOverlap2D.transform.position.ToString());
                Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), adjustOverlap2D.transform.position, Quaternion.identity, 0.05f, EventType.Repaint);
                break;
            
        }
    }
}

[CustomEditor(typeof(Detector_EnemyChase))]
public class EnemyChaseDetectorEditor : Editor {
    private Detector_EnemyChase adjustOverlap2D;

    // Overlap 所需要定義的全域變數
    private SerializedProperty Radius;
    private SerializedProperty BoxSize;
    private SerializedProperty Angle;
    private SerializedProperty Origin;
    private SerializedProperty Diagonal;
    private SerializedProperty DetectSelectedIndex;
    private SerializedProperty AreaSelectedIndex;
    
    private string[] detectOptions = new string[] { "Single", "Mutiple" };
    private string[] areaOptions = new string[] { "Circular", "Box", "Rectangular", "Point"};

    private void OnEnable() {
        adjustOverlap2D = (Detector_EnemyChase)target;

        Radius = serializedObject.FindProperty("Radius");
        BoxSize = serializedObject.FindProperty("BoxSize");
        Angle = serializedObject.FindProperty("Angle");
        Origin = serializedObject.FindProperty("Origin");
        Diagonal = serializedObject.FindProperty("Diagonal");
        DetectSelectedIndex = serializedObject.FindProperty("DetectSelectedIndex");
        AreaSelectedIndex = serializedObject.FindProperty("AreaSelectedIndex");
        
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        
        if(adjustOverlap2D == null) {
            return;
        }
        
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        DetectSelectedIndex.intValue = EditorGUILayout.Popup("Detect Mode", DetectSelectedIndex.intValue, detectOptions);
        AreaSelectedIndex.intValue = EditorGUILayout.Popup("Area Mode", AreaSelectedIndex.intValue, areaOptions);

        //Debug.Log("OnInspectorGUI: "+detectSelectedIndex.ToString()+areaSelectedIndex.ToString());
        UpdateOptionsOfInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void UpdateOptionsOfInspector() {
        switch(DetectSelectedIndex.intValue.ToString()+AreaSelectedIndex.intValue.ToString()) 
        {
            case "00":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);

                break;
            case "01":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);

                break;
            case "02":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
            case "10":
                Radius.floatValue = EditorGUILayout.Slider("Radius", Radius.floatValue, 0.1f, 49f);
                
                break;
            case "11":
                BoxSize.vector2Value = EditorGUILayout.Vector2Field("Box Size", BoxSize.vector2Value);
                Angle.floatValue = EditorGUILayout.FloatField("Angle", Angle.floatValue);
                
                break;
            case "12":
                Origin.vector2Value = EditorGUILayout.Vector2Field("Origin Point", Origin.vector2Value);
                Diagonal.vector2Value = EditorGUILayout.Vector2Field("Diagonal Point", Diagonal.vector2Value);
                
                break;
        }
    }

    private void OnSceneGUI() {
        if(adjustOverlap2D == null) {
            return;
        }

        // 開始畫出檢測區域
        Handles.color = adjustOverlap2D.HandlesColor;
        switch(AreaSelectedIndex.intValue) 
        {
            case 0:
                Handles.DrawWireDisc(adjustOverlap2D.transform.position, Vector3.back, Radius.floatValue);
                break;
            case 1:
                Handles.DrawWireCube(adjustOverlap2D.transform.position, BoxSize.vector2Value);
                break;
            case 2:
                Handles.DrawWireCube((Origin.vector2Value + Diagonal.vector2Value) / 2f, new Vector2(Mathf.Abs(Diagonal.vector2Value.x - Origin.vector2Value.x), Mathf.Abs(Diagonal.vector2Value.y - Origin.vector2Value.y)));
                break;
            case 3:
                Handles.Label(adjustOverlap2D.transform.position, adjustOverlap2D.transform.position.ToString());
                Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), adjustOverlap2D.transform.position, Quaternion.identity, 0.05f, EventType.Repaint);
                break;
            
        }
    }
}