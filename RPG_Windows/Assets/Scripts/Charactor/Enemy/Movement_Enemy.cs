using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Enemy : Movement_Base
{
    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public new bool IsMoving {
        get {
            return m_movement != Vector2.zero && m_moveSpeed > 0f;
        }
    }

    [Header("Movement_Enemy 基本參數")]
    [SerializeField] protected Vector3 defaultMovement = Vector3.left;


    #region 基本物件

    [Header("Movement_Enemy 基本物件")]
    [SerializeField] protected Enemy<Collider2D> m_enemy;
    [SerializeField] protected Detector_EnemyPursuing m_detector_chaser;
    protected Rigidbody2D m_enemyRdbd;
    protected SpriteRenderer m_enemySprtRenderer;
    protected Animator m_enemyAnimator;

    #endregion

    protected override void Awake() {
        m_enemyRdbd = m_enemy.Rigidbody;
        m_enemySprtRenderer = m_enemy.SprtRenderer;
        m_enemyAnimator = m_enemy.Animator;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetDefaultMovement();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(m_enemyAnimator != null) m_enemyAnimator.SetFloat("moveSpeed", m_moveSpeed);
    }

    protected override void FixedUpdate() {
        if(m_enemy.CurrentBaseState.Equals(Constant.CharactorState.Dead) || 
           m_enemy.CurrentBaseState.Equals(Constant.CharactorState.Attack)) {
            SetMovement(Vector3.zero);
        }
        
        base.FixedUpdate();
        //Move();

        // if(!cantMove) {
        //     if(isJumping && jumpState == JumpState.JumpUp) {
        //         MoveWhileJump();
        //     } else {
        //Move();
        //     }
        // }
    }

    public void Move() {
        m_enemyRdbd.velocity = m_movement.normalized * MoveSpeed;
    }

    public void SetDefaultMovement() {
        SetMovement(defaultMovement);
    }
}
