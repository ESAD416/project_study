using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bullet : DirectProjectile
{
    protected void Awake() {
        referenceAxis = Vector3.up;
    }

    protected override void DestoryProjectile() {
        Destroy(gameObject);
    }
}
