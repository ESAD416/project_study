using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitSystem_Avatar : HitSystem
{
    [Header("HitSystem_Avatar 基本物件")]
    [SerializeField] protected Avatar m_target;

    protected Animator m_targetAnimator;
    protected BoxCollider2D m_targetHitBoxCollider;

    protected virtual void Awake() {
        m_targetAnimator = m_target.Animator;
    }

    protected virtual void Start() {
        m_targetHitBoxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() 
    {
        transform.position = m_target.transform.position;
    }

    protected void SetHurtTrigger()
    {
        m_targetAnimator?.SetTrigger("hurt");
    }
}
