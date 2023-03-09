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
        Player player = otherCollider.GetComponent<Player>();
        if(player != null) {
            // Player inside trigger area
            Debug.Log("player inside trigger area");
            OnPlayerEnterTrigger?.Invoke();
        }
    }
}
