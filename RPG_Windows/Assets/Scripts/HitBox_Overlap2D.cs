using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Overlap2D : Detector_Overlap2D
{
    public Attack Attacker;

    protected override void Update() {
        base.Update();

        // if(OverlapDetected != null && OverlapDetected.Length > 0) {
        //     Attacker.SetOverlapDetected(OverlapDetected);
        // }
        // else Attacker.SetOverlapDetected(new Collider2D[0]);
    }
}
