using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjects : MonoBehaviour
{
    [SerializeField] private int takinhHitCount = 2;

    public void TakeHit() {
        if(takinhHitCount > 0) {
            takinhHitCount--;
            Debug.Log("TakeHit takinhHitCount: "+takinhHitCount);
        }


        if(takinhHitCount == 0) {
            Debug.Log("Destroy BreakableObjects");
            
            // TODO Breaking Animation

            GetComponent<LootableFeedback>().InstantiateLoot();
            Destroy(gameObject);
        }
    }
}
