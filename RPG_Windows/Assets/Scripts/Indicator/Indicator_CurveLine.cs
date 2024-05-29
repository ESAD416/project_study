using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Indicator_CurveLine : MonoBehaviour
{
    private Vector3 m_StartPos;
    public Vector3 StartPos => this.m_StartPos;
    public void SetStartPos(Vector3 pos) => this.m_StartPos = pos;

    private Vector3 m_EndPos;
    public Vector3 EndPos => this.m_EndPos;
    public void SetEndPos(Vector3 pos) => this.m_EndPos = pos;


    public LineRenderer Line;
    public bool isQuadratic;
    public Vector3[] InitialState;
    public int SubDivisions = 30;

    private BezierCurveModel Curve;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update() {
        UpdateInitialState();
        Curve = new BezierCurveModel(InitialState.ToList());
        ApplyBezierCurvePath();
    }

    private void UpdateInitialState(float height = 10f) {
        Vector3 mid = (m_StartPos + m_EndPos) * 0.5f+(Vector3.up * height);

        var temp = new Vector3[3];
        temp[0] = m_StartPos;
        temp[1] = mid;
        temp[2] = m_EndPos;

        InitialState = temp;
    }

    private void ApplyBezierCurvePath() {
        Line.positionCount = SubDivisions;

        int index = 0;
        Vector3[] segments = Curve.GetSegments(SubDivisions);
        for(int j = 0; j < segments.Length; j++) {
            Line.SetPosition(index, segments[j]);
            index++;
        }
    }

}
