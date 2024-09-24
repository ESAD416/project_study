using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Tilemaps;

public class Movement_Lamniat : MonoBehaviour
{
    [Header("")]
    [SerializeField] protected float m_moveSpeed = 25.0f;                   // 角色移速
    public float MoveSpeed => this.m_moveSpeed;
    public void SetMoveSpeed(float speed) => this.m_moveSpeed = speed;      // 更改角色移速
    [SerializeField] protected float m_acceleration = 100.0f;               // 角色加速度
    public float Acceleration => this.m_acceleration;
    public void SetAcceleration(float acceleration) => this.m_acceleration = acceleration;  
    [SerializeField] protected float m_jumpingMoveSpeed = 25.0f;            // 角色跳躍時移速
    public float JumpingMoveSpeed => this.m_jumpingMoveSpeed;
    public void SetJumpingMoveSpeed(float speed) => this.m_jumpingMoveSpeed = speed;
    protected float m_moveingTimeElapsed = 0f;                              // 角色移動歷經時間

    [Header("")]
    [SerializeField] protected Vector2 m_movement = Vector2.zero;           // 角色移動向量
    public Vector2 Movement => this.m_movement;
    public void SetMovement(Vector2 vector2) => this.m_movement = vector2; 
    protected Vector2 m_previousMovement = Vector2.zero;                    // 上一禎角色移動向量
    [SerializeField] protected Vector2 m_facingDir = Vector2.right;         // 角色面向方向
    public Vector2 FacingDir => m_facingDir;
    public void SetFacingDir(Vector2 vector2) => this.m_facingDir = vector2;

    protected Vector2 m_moveVelocity;                                       // 角色剛體速度
    public Vector2 MoveVelocity => this.m_moveVelocity;
    public void SetMoveVelocity(Vector2 vector2) => this.m_moveVelocity = vector2;

    public bool IsMoving {                                                  // 角色目前是否為移動中
        get {
            // Player移動由使用者控制，故以接收輸入的移動向量為判斷基準;
            return this.m_movement != Vector2.zero;
        }
    }
    public bool CanMove = true;                                             // 角色目前是否可以移動


    [Header("")]
    [SerializeField] protected Lamniat m_movingTarget;
    [SerializeField] protected LamniatState m_LamniatState;
    [SerializeField] protected LamniatController m_LamniatController;

    protected Rigidbody2D m_targetRdbd;
    protected SpriteRenderer m_targetSprtRenderer;
    protected Animator m_targetAnimator;

    private bool m_isHoldInteraction = false;
    public bool IsHoldInteraction => this.m_isHoldInteraction;

    public bool SetFirstMoveWhileJumping = false;
    protected Vector2 m_firstMoveVelocityWhileJumping;

    protected void Awake() {
        m_targetRdbd = m_movingTarget.Rigidbody;
        m_targetSprtRenderer = m_movingTarget.SprtRenderer;
        m_targetAnimator = m_movingTarget.Animator;
    }

    protected void OnEnable() {
        //CanMove = true;
    }

    // Start is called before the first frame update
    protected void Start() 
    {
        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Move.performed += content => {
            //Debug.Log("Lamniat_Land.Move.started");
            var inputVecter2 = content.ReadValue<Vector2>();
            m_facingDir = inputVecter2;
            m_movement = inputVecter2;
            //m_avatar.SetStatus(Charactor.CharactorStatus.Move);
            if(m_previousMovement == Vector2.zero) m_previousMovement = inputVecter2;
            //Debug.Log("m_LamiatJump.IsJumping :"+m_LamiatJump.IsJumping+", m_LamiatJump.IsAttacking :"+m_LamiatCombat.IsAttacking);
            if(!m_LamniatController.LamniatJump.IsJumping && !m_LamniatController.LamniatCombat.IsAttacking) 
                m_LamniatState.SetCurrentBaseState(m_LamniatState.Move);
            if(m_targetSprtRenderer!= null) {
                var faceLeft = m_targetSprtRenderer.flipX;
                m_targetSprtRenderer.flipX = AnimeUtils.isLeftForHorizontalAnimation(Movement, faceLeft);
            }
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Move.canceled += content => {
            m_movement = Vector2.zero;
            if(m_previousMovement != Vector2.zero) m_previousMovement = Vector2.zero;
            if(m_LamniatController.LamniatJump.IsJumping) m_LamniatState.SetCurrentBaseState(m_LamniatState.Jump);
            else if(m_LamniatController.LamniatCombat.IsAttacking) m_LamniatState.SetCurrentBaseState(m_LamniatState.Attack);
            else m_LamniatState.SetCurrentBaseState(m_LamniatState.Idle);
        };

    }

    protected void Update()
    {
        //m_previousMovement = m_movement;

        // 實作加速效果
        // --正在移動
        if(IsMoving)
        {
            if (m_LamniatController.LamniatJump.IsJumping) 
            {
                if (!SetFirstMoveWhileJumping) {
                    Debug.Log("SetFirstMoveWhileJumping m_movement: "+m_movement);
                    //m_moveVelocity = Vector2.MoveTowards(m_moveVelocity, m_movement.normalized * m_jumpingMoveSpeed, m_acceleration * Time.deltaTime);
                    Debug.Log("SetFirstMoveWhileJumping m_movement: "+m_moveVelocity);
                    m_moveVelocity = m_movement.normalized * m_jumpingMoveSpeed;
                    m_firstMoveVelocityWhileJumping = m_moveVelocity;
                    SetFirstMoveWhileJumping = true;
                }

                m_moveVelocity = m_firstMoveVelocityWhileJumping;

                // --如果在跳躍期間使用者輸入改變，移動方向雖然會更新，但只會為了增加阻尼感做減速，所以不影響整體跳躍移動方向
                // 使用者x輸入為零，或與跳躍x向量正負不同就做減速阻尼
                if ((m_movement.normalized.x <= 0 && m_firstMoveVelocityWhileJumping.x > 0) ||
                    (m_movement.normalized.x >= 0 && m_firstMoveVelocityWhileJumping.x < 0) )
                {
                    m_firstMoveVelocityWhileJumping.x = m_firstMoveVelocityWhileJumping.x / 1.1f;
                }
                // 使用者y輸入不為零，且跳躍y向量絕對值小跳躍x向量絕對值就做減速阻尼
                if (m_movement.normalized.y != 0 && Mathf.Abs(m_firstMoveVelocityWhileJumping.y) < Mathf.Abs(m_firstMoveVelocityWhileJumping.x))
                {
                    m_firstMoveVelocityWhileJumping.y = Mathf.Abs(m_firstMoveVelocityWhileJumping.x) * 0.8f * m_movement.normalized.y;
                }

                // 使用者y輸入為零，或與跳躍y向量正負不同就做減速阻尼
                if ((m_movement.normalized.y <= 0 && m_firstMoveVelocityWhileJumping.y > 0) ||
                    (m_movement.normalized.y >= 0 && m_firstMoveVelocityWhileJumping.y < 0))
                {
                    m_firstMoveVelocityWhileJumping.y = m_firstMoveVelocityWhileJumping.y / 1.1f;
                }
                // 使用者x輸入不為零，且跳躍x向量絕對值小跳躍y向量絕對值就做減速阻尼
                if (m_movement.normalized.x != 0 && Mathf.Abs(m_firstMoveVelocityWhileJumping.x) < Mathf.Abs(m_firstMoveVelocityWhileJumping.y))
                {
                    m_firstMoveVelocityWhileJumping.x = Mathf.Abs(m_firstMoveVelocityWhileJumping.y) * 0.8f * m_movement.normalized.x;
                }

                
                if (m_LamniatController.LamniatJump.JumpCounter > m_LamniatController.LamniatJump.HeightIncreaseCount)
                {
                    // 實現減速(通常只有下往上跳會觸發)
                    Tilemap currentTilemap = ColliderManager.instance.GetCurrentTilemapByAvatarHeight(m_movingTarget.CurrentHeight);
                    if(TileUtils.HasTileAtPlayerPosition(currentTilemap, m_movingTarget.BodyCollider.bounds)) 
                    {
                        m_firstMoveVelocityWhileJumping.x = m_firstMoveVelocityWhileJumping.x / 1.5f;
                        m_firstMoveVelocityWhileJumping.y = m_firstMoveVelocityWhileJumping.y / 1.5f;
                    }
                }

                Debug.Log("Movement_Lamniat Update IsJumping moveVelocity: " + m_moveVelocity);
                Debug.Log("Movement_Lamniat Update IsJumping m_firstMoveVelocityWhileJumping: " + m_firstMoveVelocityWhileJumping);
            }
            else if (m_movement != m_previousMovement)
            {
                // 如果在地面上方向改变，立即更新方向但保持当前速率
                m_moveVelocity = m_movement.normalized * m_moveVelocity.magnitude;
                m_previousMovement = m_movement;
            }
            else
            {
                m_moveVelocity = Vector2.MoveTowards(m_moveVelocity, m_movement.normalized * m_moveSpeed, m_acceleration * Time.deltaTime);
            }

            // 監測移動時間，須在移動狀態下超過0.1秒才可跳躍
            m_moveingTimeElapsed += Time.deltaTime;
            if (m_moveingTimeElapsed > 0.1f && !m_LamniatController.LamniatJump.IsJumping)
            {
                m_LamniatController.LamniatJump.CanJump = true;
            }

        } 
        // 正在跳躍
        else if (m_LamniatController.LamniatJump.IsJumping)
        {
            //Debug.Log("IsJumping not moving moveVelocity start: " + m_moveVelocity);
            m_moveingTimeElapsed = 0f;
            m_moveVelocity.x = m_moveVelocity.x / 1.05f;
            m_moveVelocity.y = m_moveVelocity.y / 1.05f;
            //Debug.Log("IsJumping not moving moveVelocity end: " + m_moveVelocity);
        }
        else
        {
            // 停止移動
            m_moveVelocity = Vector2.zero;

            // 移動時間以及跳躍判定重置
            m_moveingTimeElapsed = 0f;
            m_LamniatController.LamniatJump.CanJump = false;

            // 修正角色站的位置
            FixStandCorners(); 
        }

        //Debug.Log("m_moveVelocity before adjust: "+m_moveVelocity);
        //Debug.Log("ClampMagnitude OnHeightObjCollisionExit: "+m_LamiatJump.OnHeightObjCollisionExit+", canMove: "+CanMove);
        if (CanMove && !m_LamniatController.LamniatJump.OnHeightObjCollisionExit) 
        {
            // 設定剛體移動向量的上限
            //Debug.Log("Movement_Lamniat Update before ClampMagnitude moveVelocity: " + m_moveVelocity);
            m_moveVelocity = Vector2.ClampMagnitude(m_moveVelocity, m_moveSpeed);
            //Debug.Log("Movement_Lamniat Update after ClampMagnitude moveVelocity: " + m_moveVelocity);
        }
        else
        {
            // 碰撞牆壁或是在其他狀態下，設定剛體移動向量為0.1倍
            if(CanMove) m_moveVelocity = Vector2.ClampMagnitude(m_moveVelocity*0.1f, m_moveSpeed*0.1f);
            else m_moveVelocity = Vector2.zero;

            if(m_LamniatController.LamniatJump.OnHeightObjCollisionExit) m_LamniatController.LamniatJump.OnHeightObjCollisionExit = false;
        }

        //Debug.Log("m_moveVelocity after adjust: "+m_moveVelocity);

        SetAnimateMovementPara();

        // m_movingTarget.SetRaycastStart((Vector2)transform.position + m_movingTarget.BodyCollider.offset) ;  // 射线的起点
        // var m_raycastHitJumpTrigger = Physics2D.Raycast(m_movingTarget.RaycastStart, m_movement.normalized, 1f, m_movingTarget.RaycastJumpTriggerLayerMask);
        // m_movingTarget.SetRaycastHitJumpTrigger(m_raycastHitJumpTrigger);

        // Color color = m_raycastHitJumpTrigger.collider != null ? Color.red : Color.green;
        // Debug.DrawLine(m_movingTarget.RaycastStart, (Vector2)m_movingTarget.RaycastStart + m_movement.normalized*1f, color);
        // if(m_raycastHitJumpTrigger.collider != null ) Debug.Log("m_raycastHit: "+m_raycastHitJumpTrigger.collider.name);
    }

    protected void FixedUpdate() 
    {
        //m_avatarRdbd.velocity = m_moveVelocity;
        m_targetRdbd.MovePosition(m_targetRdbd.position + m_moveVelocity * Time.fixedDeltaTime);
    }

    protected void OnDisable() {
        // CanMove = false;
        // m_targetRdbd.velocity = Vector2.zero;
    }
    
    protected void FixStandCorners()
    {
        // 地面的時候將四個角的位置標記起來
        // 哪一個角有碰到，就讓給collider一個offset往該角移動
        // 只碰到一個角，就往斜向位移
        // 碰到兩個角，就往X或Y方向移動
        // 四個角都碰到，offset就會相加並互相抵銷
        // 跟跳躍的數值差在，位移速度會比較快一點

        Tilemap currentTilemap = ColliderManager.instance.GetCurrentTilemapByAvatarHeight(m_movingTarget.CurrentHeight);
        Vector3Int body_bottom_left = currentTilemap.WorldToCell(new Vector3(m_movingTarget.BodyCollider.bounds.min.x, m_movingTarget.BodyCollider.bounds.min.y));
        Vector3Int body_top_right = currentTilemap.WorldToCell(new Vector3(m_movingTarget.BodyCollider.bounds.max.x, m_movingTarget.BodyCollider.bounds.max.y));
        Vector3Int body_bottom_right = currentTilemap.WorldToCell(new Vector3(m_movingTarget.BodyCollider.bounds.max.x, m_movingTarget.BodyCollider.bounds.min.y));
        Vector3Int body_top_left = currentTilemap.WorldToCell(new Vector3(m_movingTarget.BodyCollider.bounds.min.x, m_movingTarget.BodyCollider.bounds.max.y));

        Vector3 offsetPosition = Vector3.zero;
        
        if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_left))
        {
            offsetPosition += new Vector3(-0.05f * m_movingTarget.BodyCollider.size.x, -0.05f * m_movingTarget.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_right))
        {
            offsetPosition += new Vector3(0.05f * m_movingTarget.BodyCollider.size.x, -0.05f * m_movingTarget.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_top_left))
        {
            offsetPosition += new Vector3(-0.05f * m_movingTarget.BodyCollider.size.x, 0.05f * m_movingTarget.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_top_right))
        {
            offsetPosition += new Vector3(0.05f * m_movingTarget.BodyCollider.size.x, 0.05f * m_movingTarget.BodyCollider.size.y);
        }

        if (offsetPosition != Vector3.zero)
        {
            Debug.Log("FixStandCorners offsetPosition: " + offsetPosition);
            offsetPosition.x = Mathf.Clamp(offsetPosition.x, -0.25f * m_movingTarget.BodyCollider.size.x, 0.25f * m_movingTarget.BodyCollider.size.x);
            offsetPosition.y = Mathf.Clamp(offsetPosition.y, -0.25f * m_movingTarget.BodyCollider.size.y, 0.25f * m_movingTarget.BodyCollider.size.y);
            Vector3 fixCornersPosition = transform.position + offsetPosition;

            transform.position = fixCornersPosition;
        }
    }

    protected void SetAnimateMovementPara() 
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        dict.Add("movementX", m_movement.x);
        dict.Add("movementY", m_movement.y);
        dict.Add("facingDirX", m_facingDir.x);
        dict.Add("facingDirY", m_facingDir.y);

        if(m_targetAnimator != null) AnimeUtils.SetAnimateFloatPara(m_targetAnimator, dict);
    }
    
}
