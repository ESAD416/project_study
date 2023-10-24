using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Overlap2D : Detector_Overlap2D
{
    [SerializeField] protected Attack m_attacker;
    public Attack Attacker => this.m_attacker;
    public void SetAttacker(Attack attacker) => this.m_attacker = attacker;


    protected override void Update() {
        base.Update();

        if(OverlapDetected != null && OverlapDetected.Length > 0) {
            m_attacker.SetOverlapDetected(OverlapDetected);
        }
        else m_attacker.SetOverlapDetected(new Collider2D[0]);
    }
}
