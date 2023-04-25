using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumericSpring2D
{
    private Vector2 m_TargetCoordinate;
    private Vector2 m_CurrentCoordinate;
    private Vector2 m_CurrentChange;

    private float m_fDampingRatio;
    private float m_fFrequency;
    private float m_fSensitivity;

    public NumericSpring2D(Vector3 originCoordinate, float fFrequency, float fDumpingRatio = 1.0f, float fSensitivity = 0.01f) {
        // 物件的初始位置、彈簧頻率、阻尼係數(預設為 1)、靈敏度(預設為 0.01)
        this.m_TargetCoordinate = originCoordinate;
        this.m_CurrentCoordinate = originCoordinate;
        this.m_CurrentChange = new Vector3();
 
        m_fFrequency = fFrequency;
        m_fDampingRatio = fDumpingRatio;
        m_fSensitivity = fSensitivity;
    }

    public void SetTargetCoordinate(Vector3 TargetCoordinate) {
        m_TargetCoordinate = TargetCoordinate;
    }

    public Vector3 DampingCoordinateUpdate(float fDeltaTime) {
        float ww = m_fFrequency * m_fFrequency;
        float wwt = ww * fDeltaTime;
        float wwtt = wwt * fDeltaTime;
        float f = 1.0f + 2.0f * fDeltaTime * m_fDampingRatio * m_fFrequency;
        float detInv = 1.0f / (f + wwtt);
 
        m_CurrentCoordinate = (m_CurrentCoordinate * f + wwtt * m_TargetCoordinate + fDeltaTime * m_CurrentChange) * detInv;
        m_CurrentChange = (m_CurrentChange + wwt * (m_TargetCoordinate - m_CurrentCoordinate)) * detInv;
 
        if (Vector3.Magnitude(m_CurrentCoordinate - m_TargetCoordinate) < m_fSensitivity) {
            m_CurrentCoordinate = m_TargetCoordinate;
        }
        return m_CurrentCoordinate;
    }
}
