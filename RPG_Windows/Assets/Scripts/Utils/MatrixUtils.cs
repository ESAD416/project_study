using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatrixUtils
{
    // 以下的方法帶入的float參數都是弧度，如果要用角度來計算需事先將其轉變為弧度
    // Ex: 角度30度，弧度 = 30 * Mathf.Deg2Rad;
    // Unity的旋轉預設是逆時針方向(跟數學的向限一樣)，如果想計算出順時針方向，將角度/弧度設為負值即可

    [ContextMenu("Get Result")]
    public static void GetResult() {
        Matrix4x4 m = RotateZ(180);
        Debug.Log("Matrix4x4:\n"+m);
    }

    public static Matrix4x4 RotateX(float aAngleRad)     // 以X為軸心旋轉
    {
        Matrix4x4 m = Matrix4x4.identity;     //  1   0   0   0 
        m.m11 = m.m22 = Mathf.Cos(aAngleRad); //  0  cos -sin 0
        m.m21 = Mathf.Sin(aAngleRad);         //  0  sin  cos 0
        m.m12 = -m.m21;                       //  0   0   0   1
        return m;
    }
    
    public static Matrix4x4 RotateY(float aAngleRad)     // 以Y為軸心旋轉
    {
        Matrix4x4 m = Matrix4x4.identity;     // cos  0  sin  0
        m.m00 = m.m22 = Mathf.Cos(aAngleRad); //  0   1   0   0
        m.m02 = Mathf.Sin(aAngleRad);         //-sin  0  cos  0
        m.m20 = -m.m02;                       //  0   0   0   1
        return m;
    }

    public static Matrix4x4 RotateZ(float aAngleRad)     // 以Z為軸心旋轉
    {
        Matrix4x4 m = Matrix4x4.identity;     // cos -sin 0   0
        m.m00 = m.m11 = Mathf.Cos(aAngleRad); // sin  cos 0   0
        m.m10 = Mathf.Sin(aAngleRad);         //  0   0   1   0
        m.m01 = -m.m10;                       //  0   0   0   1
        return m;
    }

    

    public static Matrix4x4 MatrixForCalcPosByRotateZ(float aAngleRad)     // 計算原圖座標用，以Z為軸心的旋轉矩陣，
    {
        Matrix4x4 m = Matrix4x4.identity;     // cos sin  0   0
        m.m00 = m.m11 = Mathf.Cos(aAngleRad); //-sin cos  0   0
        m.m01 = Mathf.Sin(aAngleRad);         //  0   0   1   0
        m.m10 = -m.m01;                       //  0   0   0   1
        return m;
    }     
}
