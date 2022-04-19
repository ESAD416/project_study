using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : LevelTrigger
{
    Vector2 stairDir;

    protected override void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
            Debug.Log("player name: "+player.gameObject.name);
            Debug.Log("player layer: "+player.gameObject.layer);
            Debug.Log("player x: "+player.gameObject.transform.position.x);
            Debug.Log("player y: "+player.gameObject.transform.position.y);
            Debug.Log("player z: "+player.gameObject.transform.position.z);
            float ms = player.GetMoveSpeed();
            stairDir = player.GetFacingDir();
            player.inLevelTrigger = true;
            player.altitudeIncrease = 1f;

        }
        //base.OnTriggerEnter2D(otherCollider);
    }

    protected override void OnTriggerExit2D(Collider2D otherCollider) {
        Debug.Log("OnTriggerExit");
        //base.OnTriggerExit2D(otherCollider);
    }
}
