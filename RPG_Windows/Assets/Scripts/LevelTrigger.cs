using System;
using UnityEngine;

public abstract class LevelTrigger : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        Debug.Log("LevelTrigger Start");
    }

    protected virtual void OnTriggerEnter2D(Collider2D otherCollider) {
        Debug.Log("OnTriggerEnter");
        if(otherCollider.gameObject.layer == 7) {   // layer7 : Stairs
            Debug.Log("Enter Stairs");
            // Physics2D.IgnoreLayerCollision(6, 7);
            // Debug.Log("Enter Stairs 2");
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D otherCollider) {
        Debug.Log("OnTriggerExit");
        if(otherCollider.gameObject.layer == 7) {   // layer7 : Stairs
            Debug.Log("Exit Stairs");
            // Physics2D.IgnoreLayerCollision(6, 7);
            // Debug.Log("Enter Stairs 2");
        }
    }
}
