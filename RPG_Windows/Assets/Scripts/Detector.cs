using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [Header("Detector Parameters")]
    [SerializeField] private LayerMask targetLayer;
    public bool targetDetected;
    [Range(.1f, 1)]
    public float detectRadius;

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
}
