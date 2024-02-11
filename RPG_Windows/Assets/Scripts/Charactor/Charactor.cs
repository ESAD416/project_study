using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static JumpMechanismUtils;

public abstract class Charactor : MonoBehaviour
{
    
    #region 角色物件
    [Header("角色基本物件")]
    [SerializeField] protected Rigidbody2D m_Rigidbody;
    /// <summary>
    /// 角色物理剛體
    /// </summary>
    public Rigidbody2D Rigidbody => this.m_Rigidbody;

    /// <summary>
    /// 角色碰撞控制
    /// </summary>
    [SerializeField] protected Collider2D m_bodyCollider; // 預設使用的是BoxCollider2D
    public Collider2D BodyCollider => this.m_bodyCollider;
    
    [SerializeField] protected SpriteRenderer m_SprtRenderer;
    /// <summary>
    /// 角色圖片精靈
    /// </summary>
    public SpriteRenderer SprtRenderer => this.m_SprtRenderer;
    public void SetSpriteRenderer(SpriteRenderer sprtR) => this.m_SprtRenderer = sprtR;
    
    [SerializeField] protected Animator m_Animator;
    /// <summary>
    /// 角色動畫控制器
    /// </summary>
    public Animator Animator => this.m_Animator;
    /// <summary>
    /// 角色中心Transform
    /// </summary>
    public Transform m_CenterObj;
    /// <summary>
    /// 角色底部Transform
    /// </summary>
    public Transform m_ButtomObj;
    /// <summary>
    /// 角色相關資訊存取
    /// </summary>
    public CharactorData m_InfoStorage;

    #endregion
    
    #region 角色物件
    [Header("角色基本參數")]
    [SerializeField] protected Vector3 m_Center;
    /// <summary>
    /// 角色中心
    /// </summary>
    public Vector3 Center => this.m_Center;
    
    [SerializeField] protected Vector3 m_Buttom;
    /// <summary>
    /// 角色底部
    /// </summary>
    public Vector3 Buttom => this.m_Buttom;
    
    [SerializeField] protected Vector3 m_Coordinate;
    /// <summary>
    /// 角色橫縱高座標
    /// </summary>
    public Vector3 Coordinate => this.m_Coordinate;
    public void SetCoordinate(Vector3 coordinate) => this.m_Coordinate = coordinate;

    #endregion

    #region 移動參數
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

    #region 跳躍參數
    [Header("")]
    [SerializeField] protected float m_currHeight = 0f;
    public float CurrentHeight => m_currHeight;
    public void SetCurrentHeight(float height) => this.m_currHeight = height;
    
    [SerializeField] protected float m_lastHeight = 0f;
    public float LastHeight => m_lastHeight;
    public void SetLastHeight(float height) => this.m_lastHeight = height;
    
    #endregion
    
    protected virtual void Awake() {
        m_Coordinate = transform.position;
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
        //Debug.Log("takeHitRoutine == null: "+(takeHitRoutine == null));
        m_Center = m_CenterObj?.position ?? Vector3.zero;
        m_Buttom = m_ButtomObj?.position ?? Vector3.zero;

        //UpdateCoordinate();
    }

    protected virtual void FixedUpdate() {
        //Debug.Log("cantMove: "+cantMove);
        // if(!cantMove) {
        //     if(isJumping && jumpState == JumpState.JumpUp) {
        //         MoveWhileJump();
        //     } else {
        //         Move();
        //     }
        // }
    }

    // #region 位移控制
    // public void UpdateCoordinate() {
    //     Vector3 result = Vector3.zero;
    //     result.x = m_Center.x;
    //     result.z = m_currHeight;
    //     result.y = m_Center.y - m_lastHeight;

    //     m_Coordinate = result;
    // }

    // public Vector3 GetWorldPosByCoordinate(Vector3 coordinate) {
    //     //Debug.Log("GetWorldPosByCoordinate coordinate" + coordinate);
    //     Vector3 result = Vector3.zero;
    //     result.x = coordinate.x;
    //     result.y = coordinate.y + coordinate.z;
    //     result.z = coordinate.z;

    //     //Debug.Log("GetWorldPosByCoordinate result" + result);
    //     return result;
    // }

    // #endregion
}
