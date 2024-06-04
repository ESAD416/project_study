using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Enemy : MonoBehaviour
{
    #region 基本物件

    [Header("Movement_Enemy 基本物件")]
    [SerializeField] protected Enemy m_enemy;
    [SerializeField] protected Detector_EnemyChase m_detector_chaser;
    protected Rigidbody2D m_enemyRdbd;
    protected SpriteRenderer m_enemySprtRenderer;
    protected Animator m_enemyAnimator;

    #endregion

    #region 基本參數

    [Header("Movement_Enemy 基本參數")]
    [SerializeField] protected Vector3 defaultMovement = Vector3.left;
    [SerializeField] protected float m_moveSpeed = 11f;
    /// <summary>
    /// 角色移速
    /// </summary>
    public float MoveSpeed => m_moveSpeed;
    /// <summary>
    /// 更改角色移速
    /// </summary>
    public void SetMoveSpeed(float speed) {
        this.m_moveSpeed = speed;
    }
    
    [SerializeField] protected Vector3 m_movement = Vector3.zero;
    /// <summary>
    /// 角色移動向量
    /// </summary>
    public Vector3 Movement => m_movement;
    /// <summary>
    /// 更改角色向量
    /// </summary>
    public void SetMovement(Vector3 vector3) {
        // Debug.Log("SetMovement: "+vector3);
        this.m_movement = vector3;
    }
    /// <summary>
    /// 角色面向方向
    /// </summary>
    [SerializeField] protected Vector2 m_facingDir = Vector2.down;
    public Vector2 FacingDir => m_facingDir;
    /// <summary>
    /// 更改角色面向方向
    /// </summary>
    public void SetFacingDir(Vector2 vector2) {
        this.m_facingDir = vector2;
    }
    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public bool isMoving {
        get {
            return (m_movement.x != 0 || m_movement.y != 0) && m_moveSpeed > 0f;
        }
    }

    public bool CanMove = true;

    # endregion

    protected virtual void Awake() {
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

    protected virtual void FixedUpdate() {
        if(m_enemy.CurrentBaseState.Equals(BaseStateMachine_Enemy.BaseState.Dead) || 
           m_enemy.CurrentBaseState.Equals(BaseStateMachine_Enemy.BaseState.Attack)) {
            SetMovement(Vector3.zero);
        }
        
        Move();

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
