using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerEnterTriggerEvent;

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
}
