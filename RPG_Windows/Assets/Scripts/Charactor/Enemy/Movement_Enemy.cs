using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Enemy : MonoBehaviour
{
    [SerializeField] protected Enemy m_enemy;
    protected Rigidbody2D m_targetRdbd;
    protected SpriteRenderer m_targetSprtRenderer;
    protected Animator m_targetAnimator;

    [Header("Movement Parameters")]
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
    
    protected Vector3 m_movement = Vector3.zero;
    /// <summary>
    /// 角色移動向量
    /// </summary>
    public Vector3 Movement => m_movement;
    /// <summary>
    /// 更改角色向量
    /// </summary>
    public void SetMovement(Vector3 vector3) {
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
            return Movement.x != 0 || Movement.y != 0;
        }
    }

    protected virtual void Awake() {
        m_targetRdbd = m_enemy.Rigidbody;
        m_targetSprtRenderer = m_enemy.SprtRenderer;
        m_targetAnimator = m_enemy.Animator;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate() {
        if(m_enemy.CharStatus.Equals(Charactor.CharactorStatus.Attack)) {
            //Debug.Log("attacking");
            SetMovement(Vector3.zero);
        } else if(m_enemy.CharStatus.Equals(Charactor.CharactorStatus.Dead)) {
            //SetMovement(Vector3.zero);
        }
        // Move();

        // if(!cantMove) {
        //     if(isJumping && jumpState == JumpState.JumpUp) {
        //         MoveWhileJump();
        //     } else {
        Move();
        //     }
        // }
    }

    public void Move() {
        //Debug.Log("FixedUpdate movement.normalized: "+movement.normalized+", moveSpeed: "+moveSpeed );
        m_targetRdbd.velocity = Movement.normalized * MoveSpeed;
        //m_Rigidbody.AddForce(movement.normalized* moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        // transform.Translate(movement*moveSpeed*Time.deltaTime);
    }

    public void SetDefaultMovement() {
        SetMovement(defaultMovement);
    }
}
