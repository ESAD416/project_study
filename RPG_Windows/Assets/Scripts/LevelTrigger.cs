using System;
using UnityEngine;

public abstract class LevelTrigger : MonoBehaviour
{
    private int levelLayer;

    protected Player player;

    protected virtual void Start() {
        levelLayer = gameObject.layer;

        player = GameObject.FindObjectOfType(typeof(Player)) as Player;
    }    

    protected virtual void OnTriggerEnter2D(Collider2D otherCollider) {
 
            // Physics2D.IgnoreLayerCollision(6, 7);
            // Debug.Log("Enter Stairs 2");

    }

    protected virtual void OnTriggerExit2D(Collider2D otherCollider) {
            Debug.Log("Exit Stairs");
            // Physics2D.IgnoreLayerCollision(6, 7);
            // Debug.Log("Enter Stairs 2");
    }
}
