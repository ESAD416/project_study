using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private Charactor attacker;

    public int meleeDamage = 10;

    private void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Enemies") {
            Debug.Log("Hit Target !!");
            Debug.Log("Target name: " + otherCollider.name);
            Debug.Log("Attacker name: " + attacker.name);

            otherCollider.gameObject.GetComponent<Enemy_Horizontal>().TakeHitProcess(meleeDamage, attacker.m_Center);
        }
    }
}
