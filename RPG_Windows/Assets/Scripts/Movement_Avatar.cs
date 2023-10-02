using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Avatar : MonoBehaviour
{
    [SerializeField] protected Avatar m_avatar;
    [SerializeField] protected Rigidbody2D m_targetRdbd;
    [SerializeField] protected SpriteRenderer m_targetSprtRenderer;
    [SerializeField] protected Animator m_targetAnimator;
    

    [Header("Movement Parameters")]
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
    /// <summary>
    /// 角色目前是否能移動
    /// </summary>
    // public bool cantMove {
    //     get {
    //         bool jumpingUpButNotFinish = isJumping && jumpState == JumpState.JumpUp && jumpIncrement < 1f;
    //         return jumpingUpButNotFinish || jumpHitColli || (isTakingHit && !hyperArmor);
    //     }
    // }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAnimateMovementPara(Movement, FacingDir);
    }

    protected virtual void FixedUpdate() {
        if(m_avatar.Status.Equals(Charactor.CharactorStatus.Attack)) {
            //Debug.Log("attacking");
            SetMovement(Vector3.zero);
        }

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

    public void MoveWhileJump() {
        //m_targetRdbd.velocity = Movement.normalized * jumpingMovementVariable * MoveSpeed;
    }

    protected void SetAnimateMovementPara(Vector3 movement, Vector2 facingDir) {
        // Debug.Log("movement.x: "+movement.x + "movement.y: "+movement.y);
        // Debug.Log("facingDir.x: "+facingDir.x + "facingDir.y: "+facingDir.y);
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add("movementX", movement.x);
        dict.Add("movementY", movement.y);
        dict.Add("facingDirX", facingDir.x);
        dict.Add("facingDirY", facingDir.y);

        if(m_targetAnimator != null) AnimeUtils.SetAnimateFloatPara(m_targetAnimator, dict);
    }
}
