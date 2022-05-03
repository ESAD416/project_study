using System;
using UnityEngine;

public class LevelCollision : MonoBehaviour
{
    public float altitude = 0;
    private int levelLayer;

    public bool playerOnLevel = false;

    protected virtual void Start() {
        levelLayer = gameObject.layer;
    }    

    protected virtual void OnTriggerEnter2D(Collider2D otherCollider) {
        // Physics2D.IgnoreLayerCollision(6, 7);
        Debug.Log("LevelTrigger Enter");

    }

    protected virtual void OnTriggerExit2D(Collider2D otherCollider) {
        Debug.Log("LevelTrigger Exit");
        // Physics2D.IgnoreLayerCollision(6, 7);
        // Debug.Log("Enter Stairs 2");
    }
}
