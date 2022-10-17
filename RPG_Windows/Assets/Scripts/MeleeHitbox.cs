using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    public int meleeDamage = 10;

    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Enemies") {
            Debug.Log("Hit Target !!");
            Debug.Log("Target name: " + otherCollider.name);

            otherCollider.gameObject.GetComponent<Enemy_Horizontal>().TakeHitProcess(meleeDamage, otherCollider.gameObject.transform);
        }
    }
}
