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

    protected override void Start() {
        base.Start();
        m_targetHitBoxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Update() 
    {
        base.Update();
        transform.position = m_target.transform.position;
    }
}
