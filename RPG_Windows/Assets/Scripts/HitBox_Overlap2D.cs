using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            m_attacker.SetOverlapDetected(new Collider2D[0]);
            return;
        }

        if(OverlapDetected != null && OverlapDetected.Length > 0) {
            m_attacker.SetOverlapDetected(OverlapDetected);
            counterElapsed++;
        }
        else m_attacker.SetOverlapDetected(new Collider2D[0]);

        if(counterElapsed > 0) UpdateTimer();
        if(delaytimeElapsed >= OnHitCheckDelay) ResetTimer();
    }
    
    protected void UpdateTimer() {
        delaytimeElapsed += Time.deltaTime;
    }

    protected void ResetTimer() {
        delaytimeElapsed = 0f;
    }
}
