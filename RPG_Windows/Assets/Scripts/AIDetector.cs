using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetector : MonoBehaviour
{
    [Header("Detector Parameters")]
    [Range(.1f, 1)]
    public float detectRadius;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask visibilityLayer;
    public bool targetDetected;
    public bool targetVisable;
    
    private Transform target = null;
    public Transform Target {
        get => target;
        set {
            target = value;
            targetVisable = false;
        }
    }

    [Header("Gizmo Parameters")]
    public Color gizmoColor = Color.blue;
    public bool showGizmos = true;

    // Update is called once per frame
    void Update()
    {
        var collider = Physics2D.OverlapCircle(transform.position, detectRadius, targetLayer);
        targetDetected = collider != null;
        if(targetDetected)
            Debug.Log("Player Detected");
    }
    
    private void OnDrawGizmos() {
        if(showGizmos) {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }

    private void DectectTarget() {
        if(Target == null) {

        } else if(Target != null) {
            
        }
    }
}
