using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Tilemaps;

public class Movement_Lamniat : Movement_Player<BoxCollider2D>
{
    [Header("Movement_Lamniat 物件")]
    [SerializeField] protected DynamicJump_Lamniat m_LamiatJump;
    [SerializeField] protected Combat_Lamniat m_LamiatCombat;

    //[Header("Movement_Lamniat 參數")]
    private bool m_isHoldInteraction = false;
    public bool IsHoldInteraction => this.m_isHoldInteraction;

    public bool SetFirstMoveWhileJumping = false;
    protected Vector2 m_firstMoveVelocityWhileJumping;

    protected override void OnEnable() {
    }

    protected override void OnDisable() {
    }

    // Start is called before the first frame update
    protected override void Start() 
    {
        #region InputSystem事件設定

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Move.performed += content => {
            //Debug.Log("Lamniat_Land.Move.started");
            var inputVecter2 = content.ReadValue<Vector2>();
            m_facingDir = inputVecter2;
            m_movement = inputVecter2;
            //m_avatar.SetStatus(Charactor.CharactorStatus.Move);
            if(m_previousMovement == Vector2.zero) m_previousMovement = inputVecter2;
            //Debug.Log("m_LamiatJump.IsJumping :"+m_LamiatJump.IsJumping+", m_LamiatJump.IsAttacking :"+m_LamiatCombat.IsAttacking);
            if(!m_LamiatJump.IsJumping && !m_LamiatCombat.IsAttacking) m_player.SetCurrentBaseState(m_player.Move);
            if(m_playerSprtRenderer!= null) {
                var faceLeft = m_playerSprtRenderer.flipX;
                m_playerSprtRenderer.flipX = AnimeUtils.isLeftForHorizontalAnimation(Movement, faceLeft);
            }
        };

        PlayerInputManager.instance.InputCtrl.Lamniat_Land.Move.canceled += content => {
            m_movement = Vector2.zero;
            if(m_previousMovement != Vector2.zero) m_previousMovement = Vector2.zero;
            if(m_LamiatJump.IsJumping) m_player.SetCurrentBaseState(m_player.Jump);
            else if(m_LamiatCombat.IsAttacking) m_player.SetCurrentBaseState(m_player.Attack);
            else m_player.SetCurrentBaseState(m_player.Idle);
        };

        #endregion
    }

    protected override void Update()
    {
        //m_previousMovement = m_movement;

        // 實作加速效果
        // --正在移動
        if(IsMoving)
        {
            if (m_LamiatJump.IsJumping) 
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

                
                if (m_LamiatJump.JumpCounter > m_LamiatJump.HeightIncreaseCount)
                {
                    // 實現減速(通常只有下往上跳會觸發)
                    Tilemap currentTilemap = ColliderManager.instance.GetCurrentTilemapByAvatarHeight(m_player.CurrentHeight);
                    if(TileUtils.HasTileAtPlayerPosition(currentTilemap, m_player.BodyCollider.bounds)) 
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
            if (m_moveingTimeElapsed > 0.1f && !m_LamiatJump.IsJumping)
            {
                m_LamiatJump.CanJump = true;
            }

        } 
        // 正在跳躍
        else if (m_LamiatJump.IsJumping)
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
            m_LamiatJump.CanJump = false;

            // 修正角色站的位置
            FixStandCorners(); 
        }

        //Debug.Log("m_moveVelocity before adjust: "+m_moveVelocity);
        //Debug.Log("ClampMagnitude OnHeightObjCollisionExit: "+m_LamiatJump.OnHeightObjCollisionExit+", canMove: "+CanMove);
        if (CanMove && !m_LamiatJump.OnHeightObjCollisionExit) 
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

            if(m_LamiatJump.OnHeightObjCollisionExit) m_LamiatJump.OnHeightObjCollisionExit = false;
        }

        //Debug.Log("m_moveVelocity after adjust: "+m_moveVelocity);

        base.Update();
    }

    protected override void FixedUpdate() 
    {
        //m_avatarRdbd.velocity = m_moveVelocity;
        m_playerRdbd.MovePosition(m_playerRdbd.position + m_moveVelocity * Time.fixedDeltaTime);
    }
    

    protected void FixStandCorners()
    {
        // 地面的時候將四個角的位置標記起來
        // 哪一個角有碰到，就讓給collider一個offset往該角移動
        // 只碰到一個角，就往斜向位移
        // 碰到兩個角，就往X或Y方向移動
        // 四個角都碰到，offset就會相加並互相抵銷
        // 跟跳躍的數值差在，位移速度會比較快一點

        Tilemap currentTilemap = ColliderManager.instance.GetCurrentTilemapByAvatarHeight(m_player.CurrentHeight);
        Vector3Int body_bottom_left = currentTilemap.WorldToCell(new Vector3(m_player.BodyCollider.bounds.min.x, m_player.BodyCollider.bounds.min.y));
        Vector3Int body_top_right = currentTilemap.WorldToCell(new Vector3(m_player.BodyCollider.bounds.max.x, m_player.BodyCollider.bounds.max.y));
        Vector3Int body_bottom_right = currentTilemap.WorldToCell(new Vector3(m_player.BodyCollider.bounds.max.x, m_player.BodyCollider.bounds.min.y));
        Vector3Int body_top_left = currentTilemap.WorldToCell(new Vector3(m_player.BodyCollider.bounds.min.x, m_player.BodyCollider.bounds.max.y));

        Vector3 offsetPosition = Vector3.zero;
        
        if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_left))
        {
            offsetPosition += new Vector3(-0.05f * m_player.BodyCollider.size.x, -0.05f * m_player.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_bottom_right))
        {
            offsetPosition += new Vector3(0.05f * m_player.BodyCollider.size.x, -0.05f * m_player.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_top_left))
        {
            offsetPosition += new Vector3(-0.05f * m_player.BodyCollider.size.x, 0.05f * m_player.BodyCollider.size.y);
        }
        if (TileUtils.HasTileAtPosition(currentTilemap, body_top_right))
        {
            offsetPosition += new Vector3(0.05f * m_player.BodyCollider.size.x, 0.05f * m_player.BodyCollider.size.y);
        }

        if (offsetPosition != Vector3.zero)
        {
            Debug.Log("FixStandCorners offsetPosition: " + offsetPosition);
            offsetPosition.x = Mathf.Clamp(offsetPosition.x, -0.25f * m_player.BodyCollider.size.x, 0.25f * m_player.BodyCollider.size.x);
            offsetPosition.y = Mathf.Clamp(offsetPosition.y, -0.25f * m_player.BodyCollider.size.y, 0.25f * m_player.BodyCollider.size.y);
            Vector3 fixCornersPosition = transform.position + offsetPosition;

            transform.position = fixCornersPosition;
        }
    }
    
}
