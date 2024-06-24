using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    public string TargetTagName = string.Empty;
    public LayerMask TargetLayerMask;
    public UnityEvent OnTargetEnterColliderEvent, OnTargetExitColliderEvent;
    public UnityEvent OnTargetEnterTriggerEvent, OnTargetExitTriggerEvent;

    private bool compareTagNameNeed = false;
    private bool compareLayerMaskNeed = false;

    void Update() {
        compareTagNameNeed = !string.IsNullOrEmpty(TargetTagName);
        compareLayerMaskNeed = TargetLayerMask.value != 0;
    }


    protected void OnCollisionEnter2D(Collision2D otherCollision) {
        if(IsOnCollision(otherCollision)) {
            Debug.Log("Target OnCollisionEnter2D target: "+otherCollision.collider.name+", TargetLayerMask: "+LayerMask.LayerToName(otherCollision.gameObject.layer));
            OnTargetEnterColliderEvent?.Invoke();
        
        }

    }

    protected void OnCollisionExit2D(Collision2D otherCollision) {
        if(IsOnCollision(otherCollision)) {
            Debug.Log("Target OnCollisionExit2D target: "+otherCollision.collider.name+", TargetLayerMask: "+LayerMask.LayerToName(otherCollision.gameObject.layer));
            OnTargetExitColliderEvent?.Invoke();
                
        }
    }

    private bool IsOnCollision(Collision2D otherCollision) {
        bool compareTagName = !compareTagNameNeed ? true : otherCollision.collider.CompareTag(TargetTagName);
        bool compareLayerMask = !compareLayerMaskNeed ? true : (TargetLayerMask & 1 << otherCollision.gameObject.layer) > 0;
        return compareTagName && compareLayerMask; // 同时满足
    }

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        if(IsTrigger(otherCollider)) {
            Debug.Log("Target OnTriggerEnter2D target: "+otherCollider.name+", TargetLayerMask: "+LayerMask.LayerToName(otherCollider.gameObject.layer));

            OnTargetEnterTriggerEvent?.Invoke();
        }
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {
        if(IsTrigger(otherCollider)) {
            Debug.Log("Target OnTriggerExit2D target: "+otherCollider.name+", TargetLayerMask: "+LayerMask.LayerToName(otherCollider.gameObject.layer));
            
            OnTargetExitTriggerEvent?.Invoke();
        }
    }

    private bool IsTrigger(Collider2D otherCollider) {
        bool compareTagName = !compareTagNameNeed ? true : otherCollider.CompareTag(TargetTagName);
        bool compareLayerMask = !compareLayerMaskNeed ? true : (TargetLayerMask & 1 << otherCollider.gameObject.layer) > 0;
        return compareTagName && compareLayerMask; // 同时满足
    }
}
