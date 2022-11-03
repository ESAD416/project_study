using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIAttackDector : MonoBehaviour
{
    public Enemy_Abstract enemyAI;

    [Header("Detector Parameters")]
    [Range(.1f, 1)]
    public float detectRadius;

    [SerializeField] private LayerMask targetLayer;
    public bool targetDetected;

    [Header("Gizmo Parameters")]
    public Color gizmoColor = Color.black;
    public bool showGizmos = true;

    // Update is called once per frame
    void Update()
    {
        var collider = Physics2D.OverlapCircle(transform.position, detectRadius, targetLayer);
        targetDetected = collider != null;
        if(targetDetected) {
            enemyAI.OnAttack();
        }
    } 

    private void OnDrawGizmos() {
        if(showGizmos) {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}