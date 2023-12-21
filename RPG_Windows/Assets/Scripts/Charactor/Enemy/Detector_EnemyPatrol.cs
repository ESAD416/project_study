using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Detector_EnemyPatrol : Detector_Overlap2D
{
    [SerializeField] protected Enemy m_enemy;
    public UnityEvent OnTargetDected;

    protected override void Update() {
        base.Update();
        if(OverlapDetected != null && OverlapDetected.Length > 0) {
            OnTargetDected?.Invoke();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = HandlesColor;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
