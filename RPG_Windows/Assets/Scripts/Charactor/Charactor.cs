using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static JumpMechanismUtils;

public interface ICharactor {
    Rigidbody2D Rigidbody { get; }
    SpriteRenderer SprtRenderer { get; }
    Animator Animator { get; }

    Transform CenterObj { get; }

    Vector3 Center { get; }
    Vector3 Buttom { get; }
    Vector3 Coordinate { get; }

    int CurrentHeight { get; }
    int LastHeight { get; }

    public Constant.CharactorState CurrentBaseStateName { get; }
}

public abstract class Charactor<T> : MonoBehaviour, ICharactor where T : Collider2D 
{
    #region 角色物件
    [Header("Charactor 基本物件")]
    [SerializeField] protected Rigidbody2D m_Rigidbody;
    /// <summary>
    /// 角色物理剛體
    /// </summary>
    public Rigidbody2D Rigidbody => this.m_Rigidbody;

    /// <summary>
    /// 角色碰撞控制
    /// </summary>
    [SerializeField] protected T m_bodyCollider; // 泛型Collider2D
    public T BodyCollider => this.m_bodyCollider;
    
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
    public Transform CenterObj;
    Transform ICharactor.CenterObj => this.CenterObj;
    /// <summary>
    /// 角色底部Transform
    /// </summary>
    public Transform ButtomObj;
    /// <summary>
    /// 角色相關資訊存取
    /// </summary>
    public CharactorData InfoStorage;

    #endregion
    
    #region 角色參數
    [Header("Charactor 基本參數")]
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

    #region 高度參數
    [Header("")]
    [SerializeField] protected int m_currHeight = 0;
    public int CurrentHeight => m_currHeight;
    public void SetCurrentHeight(int height) => this.m_currHeight = height;
    
    [SerializeField] protected int m_lastHeight = 0;
    public int LastHeight => m_lastHeight;
    public void SetLastHeight(int height) => this.m_lastHeight = height;
    
    #endregion

    #region 角色狀態

    protected CharactorStateMachine<Charactor<T>, T> m_currentBaseState;
    public CharactorStateMachine<Charactor<T>, T> CurrentBaseState => this.m_currentBaseState;
    public void SetCurrentBaseState(CharactorStateMachine<Charactor<T>, T> state) {
        this.m_currentBaseState?.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }
    [SerializeField] protected Constant.CharactorState m_currBaseStateName;
    public Constant.CharactorState CurrentBaseStateName => this.m_currBaseStateName;

    protected CharactorStateMachine<Charactor<T>, T> m_idle;
    public CharactorStateMachine<Charactor<T>, T> Idle => m_idle;
    protected CharactorStateMachine<Charactor<T>, T> m_move;
    public CharactorStateMachine<Charactor<T>, T> Move => m_move;
    protected CharactorStateMachine<Charactor<T>, T> m_attack;
    public CharactorStateMachine<Charactor<T>, T> Attack => m_attack;
    protected CharactorStateMachine<Charactor<T>, T> m_jump;
    public CharactorStateMachine<Charactor<T>, T> Jump => m_jump;
    protected CharactorStateMachine<Charactor<T>, T> m_hurt;
    public CharactorStateMachine<Charactor<T>, T> Hurt => m_hurt;
    protected CharactorStateMachine<Charactor<T>, T> m_dodge;
    public CharactorStateMachine<Charactor<T>, T> Dodge => m_dodge;
    protected CharactorStateMachine<Charactor<T>, T> m_dead;
    public CharactorStateMachine<Charactor<T>, T> Dead => m_dead;

    #endregion

    protected virtual void Awake() {
        m_Coordinate = transform.position;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Debug.Log("takeHitRoutine == null: "+(takeHitRoutine == null));
        m_Center = CenterObj?.position ?? Vector3.zero;
        m_Buttom = ButtomObj?.position ?? Vector3.zero;

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
