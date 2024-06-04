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


    protected void OnCollisionEnter2D(Collision2D otherCollision) {
        if(otherCollision.collider.CompareTag(TargetTagName) && (TargetLayerMask & 1 << otherCollision.gameObject.layer) > 0) {
            Debug.Log("Target OnCollisionEnter2D targetTagName: "+otherCollision.collider.tag+", TargetLayerMask: "+LayerMask.LayerToName(otherCollision.gameObject.layer));
            OnTargetEnterColliderEvent?.Invoke();
        
        }

    }

    protected void OnCollisionExit2D(Collision2D otherCollision) {
        if(otherCollision.collider.CompareTag(TargetTagName) && (TargetLayerMask & 1 << otherCollision.gameObject.layer) > 0) {
            Debug.Log("Target OnCollisionExit2D targetTagName: "+otherCollision.collider.tag+", TargetLayerMask: "+LayerMask.LayerToName(otherCollision.gameObject.layer));
            OnTargetEnterColliderEvent?.Invoke();
                
        }
    }

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.CompareTag(TargetTagName) && (TargetLayerMask & 1 << otherCollider.gameObject.layer) > 0) {
            Debug.Log("Target OnTriggerEnter2D targetTagName: "+otherCollider.tag+", TargetLayerMask: "+LayerMask.LayerToName(otherCollider.gameObject.layer));

            OnTargetEnterTriggerEvent?.Invoke();
        }
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {
        if(otherCollider.CompareTag(TargetTagName) && (TargetLayerMask & 1 << otherCollider.gameObject.layer) > 0) {
            Debug.Log("Target OnTriggerExit2D targetTagName: "+otherCollider.tag+", TargetLayerMask: "+LayerMask.LayerToName(otherCollider.gameObject.layer));
            
            OnTargetEnterTriggerEvent?.Invoke();
        }
    }
}
