using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : LevelTrigger
{
    public float altitudeVariation = 1f;

    Vector2 startPos;
    Vector2 endPos;

    protected override void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
            if(!player.inLevelTrigger) {
                Debug.Log("player not inLevelTrigger");
                startPos = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y);
                Debug.Log("startPos: "+startPos);
            }

            Physics2D.IgnoreLayerCollision(6, 8);
        }
        //base.OnTriggerEnter2D(otherCollider);
    }

    protected override void OnTriggerExit2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
            if(!player.inLevelTrigger) {
                Debug.Log("player enter in LevelTrigger");
                player.inLevelTrigger = true;
            } else {
                Debug.Log("player prepare to leave LevelTrigger");
                endPos = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y);
                Debug.Log("endPos: "+endPos);

                // Physics2D.IgnoreLayerCollision(6, 8, false);
                player.inLevelTrigger = false;
            }
        }
        //base.OnTriggerExit2D(otherCollider);
    }

    private void OnCollisionStay2D(Collision2D other) {
        Debug.Log("OnCollisionStay2D");
    }
}
