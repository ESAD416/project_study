using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Tilemaps;

public class Movement_Lamniat : Movement_Avatar
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

    // Start is called before the first frame update
    protected override void Start() 
    {
        #region InputSystem事件設定
        m_inputControls = m_avatar.InputCtrl;

        m_inputControls.Lamniat_Land.Move.performed += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            SetFacingDir(inputVecter2);
            SetMovement(inputVecter2);
            //m_avatar.SetStatus(Charactor.CharactorStatus.Move);
            m_avatar.SetCurrentBaseState(m_avatar.Move);

            if(m_avatarSprtRenderer!= null) {
                var faceLeft = m_avatarSprtRenderer.flipX;
                m_avatarSprtRenderer.flipX = AnimeUtils.isLeftForHorizontalAnimation(Movement, faceLeft);
            }
        };

        m_inputControls.Lamniat_Land.Move.canceled += content => {
            SetMovement(Vector2.zero);
            m_avatar.SetCurrentBaseState(m_avatar.Idle);
        };

        m_inputControls.Lamniat_Land.Hold.performed += content => {
            if(content.interaction is HoldInteraction) {
                m_isHoldInteraction = true;
            }
        };

        m_inputControls.Lamniat_Land.Hold.canceled += content => {
            if(content.interaction is HoldInteraction) {
                m_isHoldInteraction = false;
            }
        };

        #endregion
    }

    protected override void Update()
    {
        m_previousMovement = m_movement;

        // 實作加速效果
        // --正在移動
        if(IsMoving)
        {
            if (m_LamiatJump.IsJumping) 
            {
                // 如果在跳躍期間方向改變，方向雖然會更新，但只會減速而不會增加速率
                if (!SetFirstMoveWhileJumping) {
                    m_moveVelocity = Vector2.MoveTowards(m_moveVelocity, m_movement.normalized * m_jumpingMoveSpeed, m_acceleration * Time.deltaTime);
                    m_firstMoveVelocityWhileJumping = m_moveVelocity;
                    SetFirstMoveWhileJumping = true;
                }

                m_moveVelocity = m_firstMoveVelocityWhileJumping;
                Debug.Log("Movement_Lamniat Update IsJumping moveVelocity: " + m_moveVelocity);

                // 在這邊實現減速
                if (m_firstMoveVelocityWhileJumping.x > 0)
                {
                    if (m_movement.normalized.x <= 0)
                    {
                        m_firstMoveVelocityWhileJumping.x = m_firstMoveVelocityWhileJumping.x / 1.1f;
                    }
                    if (m_movement.normalized.y != 0 && m_firstMoveVelocityWhileJumping.y < m_firstMoveVelocityWhileJumping.x)
                    {
                        m_firstMoveVelocityWhileJumping.y = m_firstMoveVelocityWhileJumping.x * 0.8f * m_movement.normalized.y;
                    }

                }
                else if (m_firstMoveVelocityWhileJumping.x < 0)
                {
                    if (m_movement.normalized.x >= 0)
                    {
                        m_firstMoveVelocityWhileJumping.x = m_firstMoveVelocityWhileJumping.x / 1.1f;
                    }
                    if (m_movement.normalized.y != 0 && m_firstMoveVelocityWhileJumping.y < m_firstMoveVelocityWhileJumping.x)
                    {
                        m_firstMoveVelocityWhileJumping.y = m_firstMoveVelocityWhileJumping.x * 0.8f * m_movement.normalized.y;
                    }

                }

                if (m_firstMoveVelocityWhileJumping.y > 0)
                {
                    if (m_movement.normalized.y <= 0)
                    {
                        m_firstMoveVelocityWhileJumping.y = m_firstMoveVelocityWhileJumping.y / 1.1f;
                    }
                    if (m_movement.normalized.x != 0 && m_firstMoveVelocityWhileJumping.x < m_firstMoveVelocityWhileJumping.y)
                    {
                        m_firstMoveVelocityWhileJumping.x = m_firstMoveVelocityWhileJumping.y * 0.8f * m_movement.normalized.x;
                    }
                }
                else if (m_firstMoveVelocityWhileJumping.y < 0)
                {
                    if (m_movement.normalized.y >= 0)
                    {
                        m_firstMoveVelocityWhileJumping.y = m_firstMoveVelocityWhileJumping.y / 1.1f;
                    }
                    if (m_movement.normalized.x != 0 && m_firstMoveVelocityWhileJumping.x < m_firstMoveVelocityWhileJumping.y)
                    {
                        m_firstMoveVelocityWhileJumping.x = m_firstMoveVelocityWhileJumping.y * 0.8f * m_movement.normalized.x;
                    }
                }

                Tilemap currentTilemap = m_HeightManager.GetCurrentTilemapByAvatarHeight(m_avatar.CurrentHeight);
                if (m_LamiatJump.JumpCounter > m_LamiatJump.HeightIncreaseCount && TileUtils.HasTileAtPlayerPosition(currentTilemap, m_avatar.BodyCollider.bounds))
                {
                    m_firstMoveVelocityWhileJumping.x = m_firstMoveVelocityWhileJumping.x / 1.5f;
                    m_firstMoveVelocityWhileJumping.y = m_firstMoveVelocityWhileJumping.y / 1.5f;
                }

            }
            else if (m_movement != m_previousMovement)
            {
                // 如果在地面上方向改变，立即更新方向但保持当前速率
                m_moveVelocity = m_movement.normalized * m_moveVelocity.magnitude;
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
            m_moveingTimeElapsed = 0f;
            if (!IsMoving)
            {
                m_moveVelocity.x = m_moveVelocity.x / 1.05f;
                m_moveVelocity.y = m_moveVelocity.y / 1.05f;
            }

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

        
        if (!m_LamiatCombat.IsAttacking && !m_LamiatJump.OnHeightObjCollisionExit) 
        {
            // 最終設定剛體速度的上限
            m_moveVelocity = Vector2.ClampMagnitude(m_moveVelocity, m_moveSpeed);
        }
        else
        {
            // 碰撞牆壁或是在攻擊狀態下剛體速度歸零
            m_moveVelocity = Vector2.ClampMagnitude(m_moveVelocity*0f, m_moveSpeed*0f);
            if(m_LamiatJump.OnHeightObjCollisionExit) m_LamiatJump.OnHeightObjCollisionExit = false;
        }

        base.Update();
    }

    protected override void FixedUpdate() 
    {
        m_avatarRdbd.MovePosition(m_avatarRdbd.position + m_moveVelocity * Time.fixedDeltaTime);
    }
    
    protected override void OnDisable() {
    }

    
}
