using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [Header("基本參數")]
    [SerializeField] protected float m_damage;
    /// <summary>
    /// 傷害數字
    /// </summary>
    public float Damage => this.m_damage;
    [SerializeField] protected float m_damageByOverTime;
    /// <summary>
    /// 傷害數字
    /// </summary>
    public float DamageByOverTime => this.m_damageByOverTime;
    [SerializeField] private bool isDOT = false;
    public bool IsDOT => this.isDOT;

    [Header("傷害記數參數")]
    [SerializeField] private int m_initialCounter = 1;
    public int InitialCounter => this.m_initialCounter;
    [SerializeField] protected float m_OnDamageDelay = 0.3f;


    private int damageCounterElapsed;

    private void OnEnable() {
        damageCounterElapsed = m_initialCounter;
    }

    public void OnDamage(HealthSystem healthSystem) {
        healthSystem.Decrease(m_damage);
    }

    public void OnDamageOverTime(HealthSystem healthSystem) {
        //TODO 持續傷害
    }

}
