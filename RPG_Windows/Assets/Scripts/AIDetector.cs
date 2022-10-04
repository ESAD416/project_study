using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIDetector : MonoBehaviour
{
    [Header("Detector Parameters")]
    [Range(.1f, 1)]
    public float detectRadius;
    [SerializeField] private LayerMask targetLayer;
    public bool targetDetected;

    public UnityEvent OnTargetDected;

    [Header("Gizmo Parameters")]
    public Color gizmoColor = Color.blue;
    public bool showGizmos = true;

    // Update is called once per frame
    void Update()
    {
        var collider = Physics2D.OverlapCircle(transform.position, detectRadius, targetLayer);
        targetDetected = collider != null;
        if(targetDetected) {
            Debug.Log("target Detected");
            OnTargetDected?.Invoke();
        }
    }
    
    private void OnDrawGizmos() {
        if(showGizmos) {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}
