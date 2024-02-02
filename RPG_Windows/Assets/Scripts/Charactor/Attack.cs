using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("基本參數")]
    [SerializeField] protected float m_attackRate;
    public float AttackRate => m_attackRate;

    [Header("基本物件")]
    [SerializeField] protected DamageSystem m_damageSystem;
    public DamageSystem DamageSystem => this.m_damageSystem;
    [SerializeField] protected Collider2D[] m_OnHit;
    public Collider2D[] OnHit => m_OnHit;
    public void SetOverlapDetected(Collider2D[] m_Overlap) {
        this.m_OnHit = m_Overlap;
    } 

    // Update is called once per frame
    protected virtual void Update()
    {
        bool targetOnHit = m_OnHit != null && m_OnHit.Length > 0;
        if (targetOnHit) {
            Debug.Log("targetAttacked");
            foreach (Collider2D col in m_OnHit) {
                if (col.GetComponentInParent<HitSystem>() != null) {
                    col.GetComponentInParent<HitSystem>().TakeHiProcess(this);
                }
            }
        }
    }
}
