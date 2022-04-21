using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsGateway : LevelTrigger
{

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
            if(gameObject.name == "StairTopColider") {
                Debug.Log("StairTopColider");
            }
            if(gameObject.name == "StairDownColider") {
                Debug.Log("StairDownColider");
            }
        }
        base.OnTriggerEnter2D(otherCollider);
    }

    protected override void OnTriggerExit2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
        }
    }
}
