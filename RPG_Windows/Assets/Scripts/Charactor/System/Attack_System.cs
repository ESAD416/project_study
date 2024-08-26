using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_System : MonoBehaviour
{
    [Header("Attack 物件")]
    [SerializeField] protected DamageSystem m_damageSystem;
    public DamageSystem DamageSystem => this.m_damageSystem;
    [SerializeField] protected Collider2D[] m_OnHit;
    public Collider2D[] OnHit => m_OnHit;
    public void SetOverlapDetected(Collider2D[] m_Overlap) {
        Debug.Log("SetOverlapDetected");
        this.m_OnHit = m_Overlap;
    } 
    

    [Header("Attack 參數")]
    [SerializeField] protected float m_attackRate;
    public float AttackRate => m_attackRate;

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
