using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Avatar : MonoBehaviour
{
    #region 基本物件

    [Header("Movement_Avatar 基本物件")]
    [SerializeField] protected Avatar m_avatar;
    protected Rigidbody2D m_avatarRdbd;
    protected SpriteRenderer m_avatarSprtRenderer;
    protected Animator m_avatarAnimator;
    protected AvatarInputActionsControls m_inputControls;

    #endregion
    
    #region 基本參數

    [Header("Movement_Avatar 基本參數")]
    [SerializeField] protected float m_moveSpeed = 11f;
    /// <summary>
    /// 角色移速
    /// </summary>
    public float MoveSpeed => this.m_moveSpeed;
    /// <summary>
    /// 更改角色移速
    /// </summary>
    public void SetMoveSpeed(float speed) => this.m_moveSpeed = speed;
    
    protected Vector3 m_movement = Vector3.zero;
    /// <summary>
    /// 角色移動向量
    /// </summary>
    public Vector3 Movement => m_movement;
    /// <summary>
    /// 更改角色向量
    /// </summary>
    public void SetMovement(Vector3 vector3) => this.m_movement = vector3;
    [SerializeField] protected Vector3 m_movementAfterTrigger = Vector3.zero;
    /// <summary>
    /// 記錄當下角色攻擊動作時的移動向量並於攻擊動作完成後復原
    /// </summary>
    public Vector3 MovementAfterTrigger => this.m_movementAfterTrigger;
    public void SetMovementAfterTrigger(Vector3 vector3) => this.m_movementAfterTrigger = vector3;


    /// <summary>
    /// 角色面向方向
    /// </summary>
    [SerializeField] protected Vector2 m_facingDir = Vector2.right;
    public Vector2 FacingDir => m_facingDir;
    /// <summary>
    /// 更改角色面向方向
    /// </summary>
    public void SetFacingDir(Vector2 vector2) => this.m_facingDir = vector2;

    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public bool IsMoving {
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

    #endregion

    protected virtual void Awake() 
    {
        m_avatarRdbd = m_avatar.Rigidbody;
        m_avatarSprtRenderer = m_avatar.SprtRenderer;
        m_avatarAnimator = m_avatar.Animator;
    }

    protected virtual void OnEnable() {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        SetAnimateMovementPara();
    }

    protected virtual void FixedUpdate() 
    {
        // if(m_avatar.CharStatus.Equals(Charactor.CharactorStatus.Attack)) {
        //     //Debug.Log("attacking");
        //     SetMovement(Vector3.zero);
        // }

        // if(!cantMove) {
        //     if(isJumping && jumpState == JumpState.JumpUp) {
        //         MoveWhileJump();
        //     } else {
        Move();
        //     }
        // }
    }

    protected virtual void OnDisable() {

    }

    public void Move() 
    {
        //Debug.Log("FixedUpdate movement.normalized: "+movement.normalized+", moveSpeed: "+moveSpeed );
        m_avatarRdbd.velocity = Movement.normalized * MoveSpeed;
        //m_Rigidbody.AddForce(movement.normalized* moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        // transform.Translate(movement*moveSpeed*Time.deltaTime);
    }

    public void MoveWhileJump() 
    {
        //m_targetRdbd.velocity = Movement.normalized * jumpingMovementVariable * MoveSpeed;
    }

    protected void SetAnimateMovementPara() 
    {
        // Debug.Log("movement.x: "+movement.x + "movement.y: "+movement.y);
        // Debug.Log("facingDir.x: "+facingDir.x + "facingDir.y: "+facingDir.y);
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add("movementX", m_movement.x);
        dict.Add("movementY", m_movement.y);
        dict.Add("facingDirX", m_facingDir.x);
        dict.Add("facingDirY", m_facingDir.y);

        if(m_avatarAnimator != null) AnimeUtils.SetAnimateFloatPara(m_avatarAnimator, dict);
    }
}
