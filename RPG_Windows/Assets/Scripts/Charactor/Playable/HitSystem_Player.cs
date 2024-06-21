using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitSystem_Player<T> : HitSystem where T : Collider2D
{
    [Header("HitSystem_Player 基本物件")]
    [SerializeField] protected Player<T> m_targetPlayer;

    protected Animator m_targetAnimator;
    protected BoxCollider2D m_targetHitBoxCollider;

    protected override void Start() {
        base.Start();
        m_targetHitBoxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Update() 
    {
        base.Update();
        transform.position = m_targetPlayer.transform.position;
    }
}
