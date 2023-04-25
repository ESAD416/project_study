using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    public UnityAction OnPlayerEnterTrigger;

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        // if(otherCollider.gameObject.tag == "Player")
        Debug.Log("ColliderTrigger OnTriggerEnter2D");
        Player player = otherCollider.GetComponent<Player>();
        if(player != null) {
            // Player inside trigger area
            Debug.Log("ColliderTrigger player OnTriggerEnter2D");
            OnPlayerEnterTrigger?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("ColliderTrigger OnTriggerExit2D");
    }

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log("ColliderTrigger OnTriggerStay2D");
    }
}
