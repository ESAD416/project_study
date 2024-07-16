using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bullet : DirectProjectile
{
    protected void Awake() {
        referenceAxis = Vector3.up;
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        int hittableLayerMask = 1 << LayerMask.NameToLayer("Hittable");
        if (other.CompareTag("Enemy") && (hittableLayerMask & 1 << other.gameObject.layer) > 0)
        {
            Debug.Log("OnTriggerEnter2D Destory Projectile Trigger name: "+other.name);
            //Destroy(gameObject);
            DestoryProjectile();
        }
        // if(other.gameObject.tag == "Player") {
        //     Debug.Log("OnTriggerEnter2D Destory Projectile");
        //     //Destroy(gameObject);
        //     DestoryProjectile();
        // }
    }

    protected override void DestoryProjectile() {
        Destroy(gameObject);
    }
}
