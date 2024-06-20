using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement_Avatar : MonoBehaviour
{
    #region 物件

    [Header("Movement_Avatar 物件")]
    [SerializeField] protected Avatar m_avatar;
    protected Rigidbody2D m_avatarRdbd;
    protected SpriteRenderer m_avatarSprtRenderer;
    protected Animator m_avatarAnimator;

    #endregion
    
    #region 參數

    [Header("Movement_Avatar 參數")]
    #region float 移速
    [SerializeField] protected float m_moveSpeed = 25.0f;
    /// <summary>
    /// 角色移速
    /// </summary>
    public float MoveSpeed => this.m_moveSpeed;
    /// <summary>
    /// 更改角色移速
    /// </summary>
    public void SetMoveSpeed(float speed) => this.m_moveSpeed = speed;
    #endregion

    #region float 加速度
    [SerializeField] protected float m_acceleration = 100.0f;
    /// <summary>
    /// 角色加速度
    /// </summary>
    public float Acceleration => this.m_acceleration;
    /// <summary>
    /// 更改角色加速度
    /// </summary>
    public void SetAcceleration(float acceleration) => this.m_acceleration = acceleration;
    #endregion

    #region float 跳躍移速
    [SerializeField] protected float m_jumpingMoveSpeed = 25.0f;
    /// <summary>
    /// 角色跳躍時移速
    /// </summary>
    public float JumpingMoveSpeed => this.m_jumpingMoveSpeed;
    /// <summary>
    /// 更改角色跳躍時移速
    /// </summary>
    public void SetJumpingMoveSpeed(float speed) => this.m_jumpingMoveSpeed = speed;
    #endregion

    #region Vector2 移動向量
    [SerializeField] protected Vector2 m_movement = Vector2.zero;
    /// <summary>
    /// 角色移動向量
    /// </summary>
    public Vector2 Movement => this.m_movement;
    /// <summary>
    /// 更改角色向量
    /// </summary>
    public void SetMovement(Vector2 vector2) => this.m_movement = vector2;
    #endregion
    protected Vector2 m_previousMovement = Vector2.zero;

    [SerializeField] protected Vector2 m_movementAfterTrigger = Vector2.zero;
    /// <summary>
    /// 記錄當下角色其餘動作時的移動向量並於動作完成後復原
    /// </summary>
    public Vector2 MovementAfterTrigger => this.m_movementAfterTrigger;
    public void SetMovementAfterTrigger(Vector2 vector3) => this.m_movementAfterTrigger = vector3;


    #region Vector2 剛體速度
    protected Vector2 m_moveVelocity;
    /// <summary>
    /// 角色剛體速度
    /// </summary>
    public Vector2 MoveVelocity => this.m_moveVelocity;
    /// <summary>
    /// 更改角色剛體速度
    /// </summary>
    public void SetMoveVelocity(Vector2 vector2) => this.m_moveVelocity = vector2;
    #endregion
    protected Vector2 m_firstMoveVelocity;

    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public bool IsMoving {
        get {
            return this.m_movement != Vector2.zero;
        }
    }

    public bool CanMove = true;

    /// <summary>
    /// 角色移動歷經時間
    /// </summary>
    protected float m_moveingTimeElapsed = 0f;


    /// <summary>
    /// 角色面向方向
    /// </summary>
    [SerializeField] protected Vector2 m_facingDir = Vector2.right;
    public Vector2 FacingDir => m_facingDir;
    /// <summary>
    /// 更改角色面向方向
    /// </summary>
    public void SetFacingDir(Vector2 vector2) => this.m_facingDir = vector2;

    

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
        m_avatarRdbd.velocity = Movement.normalized * MoveSpeed;
    }

    protected virtual void OnDisable() {

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

    // protected void FixStandCorners()
    // {
    //     // 地面的時候將四個角的位置標記起來
    //     // 哪一個角有碰到，就讓給collider一個offset往該角移動
    //     // 只碰到一個角，就往斜向位移
    //     // 碰到兩個角，就往X或Y方向移動
    //     // 四個角都碰到，offset就會相加並互相抵銷
    //     // 跟跳躍的數值差在，位移速度會比較快一點

    //     Tilemap currentTilemap = HeightManager.instance.GetCurrentTilemapByAvatarHeight(m_avatar.CurrentHeight);
    //     Vector3Int body_bottom_left = currentTilemap.WorldToCell(new Vector3(m_avatar.BodyCollider.bounds.min.x, m_avatar.BodyCollider.bounds.min.y));
    //     Vector3Int body_top_right = currentTilemap.WorldToCell(new Vector3(m_avatar.BodyCollider.bounds.max.x, m_avatar.BodyCollider.bounds.max.y));
    //     Vector3Int body_bottom_right = currentTilemap.WorldToCell(new Vector3(m_avatar.BodyCollider.bounds.max.x, m_avatar.BodyCollider.bounds.min.y));
    //     Vector3Int body_top_left = currentTilemap.WorldToCell(new Vector3(m_avatar.BodyCollider.bounds.min.x, m_avatar.BodyCollider.bounds.max.y));

    //     Vector3 offsetPosition = Vector3.zero;
        
    //     if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_left))
    //     {
    //         offsetPosition += new Vector3(-0.05f * m_avatar.BodyCollider.size.x, -0.05f * m_avatar.BodyCollider.size.y);
    //     }
    //     if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_right))
    //     {
    //         offsetPosition += new Vector3(0.05f * m_avatar.BodyCollider.size.x, -0.05f * m_avatar.BodyCollider.size.y);
    //     }
    //     if (TileUtils.HasTileAtPosition(currentTilemap, body_top_left))
    //     {
    //         offsetPosition += new Vector3(-0.05f * m_avatar.BodyCollider.size.x, 0.05f * m_avatar.BodyCollider.size.y);
    //     }
    //     if (TileUtils.HasTileAtPosition(currentTilemap, body_top_right))
    //     {
    //         offsetPosition += new Vector3(0.05f * m_avatar.BodyCollider.size.x, 0.05f * m_avatar.BodyCollider.size.y);
    //     }

    //     if (offsetPosition != Vector3.zero)
    //     {
    //         Debug.Log("FixStandCorners offsetPosition: " + offsetPosition);
    //         offsetPosition.x = Mathf.Clamp(offsetPosition.x, -0.25f * m_avatar.BodyCollider.size.x, 0.25f * m_avatar.BodyCollider.size.x);
    //         offsetPosition.y = Mathf.Clamp(offsetPosition.y, -0.25f * m_avatar.BodyCollider.size.y, 0.25f * m_avatar.BodyCollider.size.y);
    //         Vector3 fixCornersPosition = transform.position + offsetPosition;

    //         transform.position = fixCornersPosition;
    //     }
    // }
}
