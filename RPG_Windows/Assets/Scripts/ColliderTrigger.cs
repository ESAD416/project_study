using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerEnterColliderEvent, OnPlayerExitColliderEvent;
    public UnityEvent OnPlayerEnterTriggerEvent, OnPlayerExitTriggerEvent;


    protected void OnCollisionEnter2D(Collision2D otherCollider) {
        Debug.Log("ColliderTrigger OnCollisionEnter2D");
        OnPlayerEnterColliderEvent?.Invoke();
    }

    protected void OnCollisionExit2D(Collision2D other) {
        Debug.Log("ColliderTrigger OnCollisionEnter2D");
        OnPlayerEnterColliderEvent?.Invoke();
    }

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        // if(otherCollider.gameObject.tag == "Player")
        Debug.Log("ColliderTrigger OnTriggerEnter2D");
        OnPlayerEnterTriggerEvent?.Invoke();
        // Player player = otherCollider.GetComponent<Player>();
        // if(player != null) {
        //     // Player inside trigger area
        //     Debug.Log("ColliderTrigger player OnTriggerEnter2D");
            
        // }
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {

    }
}
