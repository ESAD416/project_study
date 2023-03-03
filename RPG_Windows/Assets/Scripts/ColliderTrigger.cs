using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public event EventHandler OnPlayerEnterTrigger;

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        // if(otherCollider.gameObject.tag == "Player")
        Player player = otherCollider.GetComponent<Player>();
        if(player != null) {
            // Player inside trigger area
            Debug.Log("player inside trigger area");
            OnPlayerEnterTrigger?.Invoke(this, EventArgs.Empty);
        }
    }
}
