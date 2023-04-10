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

            otherCollider.gameObject.GetComponent<Charactor>().HealthSystem.OnDamage(meleeDamage);
            otherCollider.gameObject.GetComponent<Charactor>().TakeHitProcess(attacker.m_Center);
        }

        if(otherCollider.gameObject.tag == "BreakableObj") {
            otherCollider.gameObject.GetComponent<BreakableObjects>().TakeHit();
        }
    }
}
