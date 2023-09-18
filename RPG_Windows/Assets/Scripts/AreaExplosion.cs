using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExplosion : MonoBehaviour
{
    public float explosionRadius = 5f;
    [SerializeField] private LayerMask targetLayer;

    [Header("Gizmo Parameters")]
    public Color gizmoColor = Color.black;
    public bool showGizmos = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Explosion() {
        var collider = Physics2D.OverlapCircle(transform.position, explosionRadius, targetLayer);
        bool targetDetected = collider != null;
        if(targetDetected) {
            Debug.Log("target Detected");
        }
    }

    private void OnDrawGizmos() {
        if(showGizmos) {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
