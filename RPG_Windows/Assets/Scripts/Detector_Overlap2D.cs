using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detector_Overlap2D : MonoBehaviour
{
    public Collider2D[] OverlapDetected;
    public string DetectTagName = string.Empty;
    public LayerMask TargetLayer;

    public Color HandlesColor = Color.black;


    // Overlap 所需要定義的全域變數
    [HideInInspector] public float Radius = 0.5f;

    [HideInInspector] public Vector2 BoxSize = new Vector2(1f, 1f);
    [HideInInspector] public float Angle = 0f;

    [HideInInspector] public Vector2 Origin = new Vector2(0f, 0f);
    [HideInInspector] public Vector2 Diagonal = new Vector2(1f, 1f);

    [HideInInspector] public int DetectSelectedIndex = 0;
    [HideInInspector] public int AreaSelectedIndex = 0;

    protected virtual void Update() {
        switch(DetectSelectedIndex.ToString()+AreaSelectedIndex.ToString()) 
        {
            case "00":
                var detected_00 = Physics2D.OverlapCircle(transform.position, Radius, TargetLayer);
                
                if(detected_00 != null) {
                    if(SingleOverlapFilterByTagName(detected_00)) {
                        OverlapDetected = new Collider2D[1];
                        OverlapDetected[0] = detected_00;
                    }
                    else OverlapDetected = new Collider2D[0];
                }
                else OverlapDetected = new Collider2D[0];
                break;
            case "01":
                var detected_01 = Physics2D.OverlapBox(transform.position, BoxSize, Angle, TargetLayer);

                if(detected_01 != null) {
                    if(SingleOverlapFilterByTagName(detected_01)) {
                        OverlapDetected = new Collider2D[1];
                        OverlapDetected[0] = detected_01;
                    }
                    else OverlapDetected = new Collider2D[0];
                }
                else OverlapDetected = new Collider2D[0];
                break;
            case "02":
                var detected_02 = Physics2D.OverlapArea(Origin, Diagonal, TargetLayer);

                if(detected_02 != null) {
                    if(SingleOverlapFilterByTagName(detected_02)) {
                        OverlapDetected = new Collider2D[1];
                        OverlapDetected[0] = detected_02;
                    }
                    else OverlapDetected = new Collider2D[0];
                }
                else OverlapDetected = new Collider2D[0];
                break;
            case "03":
                var detected_03 = Physics2D.OverlapPoint(transform.position, TargetLayer);

                if(detected_03 != null) {
                    if(SingleOverlapFilterByTagName(detected_03)) {
                        OverlapDetected = new Collider2D[1];
                        OverlapDetected[0] = detected_03;
                    }
                    else OverlapDetected = new Collider2D[0];
                }
                else OverlapDetected = new Collider2D[0];
                break;
            case "10":
                var detected_10 = Physics2D.OverlapCircleAll(transform.position, Radius, TargetLayer);

                OverlapDetected = MutipleOverlapFilterByTagName(detected_10);
                break;
            case "11":
                var detected_11 = Physics2D.OverlapBoxAll(transform.position, BoxSize, Angle, TargetLayer);

                OverlapDetected = MutipleOverlapFilterByTagName(detected_11);
                break;
            case "12":
                var detected_12 = Physics2D.OverlapAreaAll(Origin, Diagonal, TargetLayer);

                OverlapDetected = MutipleOverlapFilterByTagName(detected_12);
                break;
            case "13":
                var detected_13 = Physics2D.OverlapPointAll(transform.position, TargetLayer);

                OverlapDetected = MutipleOverlapFilterByTagName(detected_13);
                break;
        }
    }

    private bool SingleOverlapFilterByTagName(Collider2D OverlapResult) {
        if(string.IsNullOrEmpty(DetectTagName)) return true;
        if(OverlapResult.CompareTag(DetectTagName)) return true;

        return false;
    }

    private Collider2D[] MutipleOverlapFilterByTagName(Collider2D[] OverlapResult) {
        if(string.IsNullOrEmpty(DetectTagName)) return OverlapResult;

        List<Collider2D> filter = new List<Collider2D>();
        foreach(Collider2D result in OverlapResult) {
            if(result.CompareTag(DetectTagName)) filter.Add(result);
        }
        if(filter.Any()) return filter.ToArray();

        return OverlapResult;
    }
}
