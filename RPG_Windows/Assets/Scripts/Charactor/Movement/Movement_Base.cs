using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement_Base : MonoBehaviour
{
    #region 參數

    [Header("Movement 參數")]
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
    
    /// <summary>
    /// 角色移動歷經時間
    /// </summary>
    protected float m_moveingTimeElapsed = 0f;

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
    /// <summary>
    /// 上一禎角色移動向量
    /// </summary>
    protected Vector2 m_previousMovement = Vector2.zero;
    #endregion

    #region Vector2 面向方向
    [SerializeField] protected Vector2 m_facingDir = Vector2.right;
    /// <summary>
    /// 角色面向方向
    /// </summary>
    public Vector2 FacingDir => m_facingDir;
    /// <summary>
    /// 更改角色面向方向
    /// </summary>
    public void SetFacingDir(Vector2 vector2) => this.m_facingDir = vector2;
    #endregion

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

    #region bool 移動判斷
    /// <summary>
    /// 角色目前是否為移動中
    /// </summary>
    public bool IsMoving {
        get {
            return this.m_movement != Vector2.zero;
        }
    }
    /// <summary>
    /// 角色目前是否可以移動
    /// </summary>
    public bool CanMove = true;

    #endregion

    #endregion

}
