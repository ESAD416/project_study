using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FireBall : DirectProjectile
{
    protected void Awake() {
        referenceAxis = Vector3.down;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player") {
            Debug.Log("OnTriggerEnter2D Destory Projectile");
            //Destroy(gameObject);
            base.DestoryProjectile();
        }
    }

    
}
