using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Indicator_CurveLine : MonoBehaviour
{
    public Enemy enemy;
    public Vector3 endPos;
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
        Vector3 startPos = enemy.transform.position;
        Vector3 mid = (startPos + endPos) * 0.5f+(Vector3.up * height);

        var temp = new Vector3[3];
        temp[0] = startPos;
        temp[1] = mid;
        temp[2] = endPos;

        InitialState = temp;
    }

    private void ApplyBezierCurvePath() {
        Line.positionCount = SubDivisions;

        int index = 0;
        Vector3[] segmrnts = Curve.GetSegments(SubDivisions);
        for(int j = 0; j < segmrnts.Length; j++) {
            Line.SetPosition(index, segmrnts[j]);
            index++;
        }
    }

}
