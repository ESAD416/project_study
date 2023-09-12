using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Indicator_CurveLine))]
public class BezierCurveLineRendererEditor : Editor
{
    private Indicator_CurveLine adjustLineRenderer;

    private SerializedProperty Line;
    private SerializedProperty InitialState;
    private SerializedProperty SubDivisions;

    private GUIContent UpdateInitialStateGUIContent = new GUIContent("Set Initial State");
    private GUIContent SmoothButtonGUIContent = new GUIContent("Smoothing Path");
    private GUIContent RestoreDefaultGUIContent = new GUIContent("Restore Default Path");

    private bool ExpandCurve = false;
    private BezierCurveModel Curves;

    private void OnEnable() {
        adjustLineRenderer = (Indicator_CurveLine) target;

        if(adjustLineRenderer.Line == null) {
            adjustLineRenderer.Line = adjustLineRenderer.GetComponent<LineRenderer>();
        }

        Line = serializedObject.FindProperty("Line");
        InitialState = serializedObject.FindProperty("InitialState");
        SubDivisions = serializedObject.FindProperty("SubDivisions");

        EnsureCurvesMatchLineRendererPositions();
    }

    public override void OnInspectorGUI() {
        if(adjustLineRenderer == null) {
            return;
        }
        EnsureCurvesMatchLineRendererPositions();

        EditorGUILayout.PropertyField(Line);
        EditorGUILayout.PropertyField(InitialState);
        //EditorGUILayout.PropertyField(SmoothingLength);
        EditorGUILayout.PropertyField(SubDivisions);

        if(GUILayout.Button(UpdateInitialStateGUIContent)) {
            adjustLineRenderer.InitialState = new Vector3[adjustLineRenderer.Line.positionCount];
            adjustLineRenderer.Line.GetPositions(adjustLineRenderer.InitialState);
            Curves = new BezierCurveModel(adjustLineRenderer.InitialState.ToList());
        }

        EditorGUILayout.BeginHorizontal();
        {
            GUI.enabled = adjustLineRenderer.Line.positionCount >=3;
            if(GUILayout.Button(SmoothButtonGUIContent)) {
                SmoothBezierCurvePath();
            }
    
            bool lineRendererPathEqualsInitialState = adjustLineRenderer.InitialState != null && adjustLineRenderer.Line.positionCount == adjustLineRenderer.InitialState.Length;
            if(lineRendererPathEqualsInitialState) {
                Vector3[] positions = new Vector3[adjustLineRenderer.Line.positionCount];
                adjustLineRenderer.Line.GetPositions(positions);

                lineRendererPathEqualsInitialState = positions.SequenceEqual(adjustLineRenderer.InitialState);
            }

            GUI.enabled = !lineRendererPathEqualsInitialState;
            if(GUILayout.Button(RestoreDefaultGUIContent)) {
                adjustLineRenderer.Line.positionCount = adjustLineRenderer.InitialState.Length;
                adjustLineRenderer.Line.SetPositions(adjustLineRenderer.InitialState);

                Curves = new BezierCurveModel(adjustLineRenderer.InitialState.ToList());
            }
        }
        EditorGUILayout.EndHorizontal();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void SmoothBezierCurvePath() {
        adjustLineRenderer.Line.positionCount = SubDivisions.intValue;
        int index = 0;
        Vector3[] segmrnts = Curves.GetSegments(SubDivisions.intValue);
        for(int j = 0; j < segmrnts.Length; j++) {
            adjustLineRenderer.Line.SetPosition(index, segmrnts[j]);
            index++;
        }

        // Reset values so insspector doesn't freeze if you use lots of smoothing sections
        SubDivisions.intValue = 30;
        serializedObject.ApplyModifiedProperties();
    } 

    private void OnSceneGUI() {
        if(adjustLineRenderer.Line.positionCount < 2) {
            Debug.Log("OnSceneGUI, adjustLineRenderer.Line.positionCount < 2");
            return;
        }
        EnsureCurvesMatchLineRendererPositions();

        DrawSegmentsCurve();
    }

    private void DrawSegmentsCurve() {
        Vector3[] segments = Curves.GetSegments(SubDivisions.intValue);
        for(int j = 0; j < segments.Length - 1; j++) {
            
            Handles.color = Color.red;
            Handles.DrawLine(segments[j], segments[j + 1]);

            float color = (float)j / (float)segments.Length;
            Handles.color = new Color(color, color, color);
            Handles.Label(segments[j], $"S{j}");
            Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), segments[j], Quaternion.identity, 0.05f, EventType.Repaint);
        }

        Handles.color = Color.red;
        Handles.Label(segments[segments.Length - 1], $"P{segments.Length - 1}");
        Handles.DotHandleCap(EditorGUIUtility.GetControlID(FocusType.Passive), segments[segments.Length - 1], Quaternion.identity, 0.05f, EventType.Repaint);

        Handles.DrawLine(segments[segments.Length - 1], Curves.GetEndPosition());
    }

    // Safety Check
    private void EnsureCurvesMatchLineRendererPositions() {
        if(Curves == null) Curves = new BezierCurveModel();
    }
}
