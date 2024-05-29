using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitBox_Overlap2D : Detector_Overlap2D
{
    [SerializeField] protected Attack m_attacker;
    public Attack Attacker => this.m_attacker;
    public void SetAttacker(Attack attacker) => this.m_attacker = attacker;

    [SerializeField] private int OnHitCheckCount = 1;
    public void SetOnHitCheckCount(int count) => this.OnHitCheckCount = count;
    [SerializeField] private float OnHitCheckDelay = 0.5f;
    public void SetOnHitCheckDelay(float delay) => this.OnHitCheckDelay = delay;

    private int counterElapsed;
    private float delaytimeElapsed;

    private void OnEnable() {
        counterElapsed = 0;
        delaytimeElapsed = 0f;
    }

    protected override void Update() {
        base.Update();
        if(counterElapsed >= OnHitCheckCount) {
            // m_attacker.SetOverlapDetected(new Collider2D[0]);
            Debug.Log("attack already hit");
            return;
        }

        if(OverlapDetected != null && OverlapDetected.Length > 0 && counterElapsed < OnHitCheckCount) {
            //m_attacker.SetOverlapDetected(OverlapDetected);
            SetOnHit();
            counterElapsed++;
            Debug.Log("attack set hit");
        }

        if(counterElapsed > 0) UpdateTimer();
        if(delaytimeElapsed >= OnHitCheckDelay) ResetTimer();
    }

    public void SetOnHit() {
        foreach (Collider2D col in OverlapDetected) {
            if (col.GetComponentInParent<HitSystem>() != null) {
                col.GetComponentInParent<HitSystem>().TakeHiProcess(m_attacker.DamageSystem, transform);
            }
        }
    }
    
    protected void UpdateTimer() {
        delaytimeElapsed += Time.deltaTime;
    }

    protected void ResetTimer() {
        delaytimeElapsed = 0f;
    }
}
