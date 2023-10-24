using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [Header("基本參數")]
    [SerializeField] protected int m_damage;
    /// <summary>
    /// 傷害數字
    /// </summary>
    public float Damage => m_damage;
    [SerializeField] protected float m_attackRate;
    public float AttackRate => m_attackRate;

    
    [SerializeField] protected Collider2D[] m_OnHit;
    public Collider2D[] OnHit => m_OnHit;
    public void SetOverlapDetected(Collider2D[] m_Overlap) {
        this.m_OnHit = m_Overlap;
    } 

    // Update is called once per frame
    protected virtual void Update()
    {
        bool targetAttacked = m_OnHit != null && m_OnHit.Length > 0;
        if (targetAttacked) {
            Debug.Log("targetAttacked");
            foreach (Collider2D col in m_OnHit) {
                if (col.GetComponent<HitSystem>() != null) {
                    col.GetComponent<HitSystem>().TakeHiProcess(this);
                }
            }
        }
    }
}
