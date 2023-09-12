using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierCurveModel
{
    public List<Vector3> positions;

    public BezierCurveModel() {
        this.positions = new List<Vector3>();
    }

    public BezierCurveModel(List<Vector3> points, bool autoQuadratic = false, float height = 12f)
    {
        this.positions = points;
    }

    public Vector3 GetStartPosition() {
        return positions.FirstOrDefault();
    }

    public Vector3 GetEndPosition() {
        return positions.LastOrDefault();
    }

    private int CalculateBinomialCoefficient(int order, int index) {
        if (index < 0 || index > order)
        {
            // 如果 index 小於 0 或大於 order，二項式係數為 0
            return 0;
        }
        // 初始化結果為 1
        int result = 1;
        // 計算 C(order, index) 的值
        for (int i = 1; i <= index; i++)
        {
            result *= (order - i + 1);
            result /= i;
        }

        return result;
    }

    public Vector3 GetQuadraticBezierControlPoint(float height) {
        return (GetStartPosition() + GetEndPosition()) * 0.5f+(Vector3.up * height);
    }

    public Vector3 GetBezierPoint(float t) {
        /// <param name="t"> 0 <= t <= 1，0獲得曲線的起點，1獲得曲線的終點</param>
        //t = Mathf.Clamp01(t);
        Vector3 result = Vector3.zero;
        int order = positions.Count - 1;
        for(int i = 0; i < positions.Count; i++) {
            float p = Mathf.Pow(1 - t, order - i) * Mathf.Pow(t, i);
            int bc = CalculateBinomialCoefficient(order, i);
            result += bc * p * positions[i];
        }
        
        return result;
    }

    public Vector3[] GetSegments(int subDivisions) {
        Vector3[] segments = new Vector3[subDivisions];

        float spanTime;
        for(int i = 0; i < subDivisions; i++) {
            spanTime = (float)i / subDivisions;
            segments[i] = GetBezierPoint(spanTime);
        }

        return segments;

    }

}
