using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_FireBall : Projectile
{
    protected void Awake() {
        referenceAxis = Vector3.down;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
