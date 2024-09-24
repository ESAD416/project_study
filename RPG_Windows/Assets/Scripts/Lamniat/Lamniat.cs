using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static JumpMechanismUtils;

public class Lamniat : MonoBehaviour
{
    [Header("")]
    [SerializeField] protected Rigidbody2D m_Rigidbody;                                     // 角色物理剛體
    public Rigidbody2D Rigidbody => this.m_Rigidbody;
    [SerializeField] protected BoxCollider2D m_bodyCollider;                                            // 角色碰撞控制
    public BoxCollider2D BodyCollider => this.m_bodyCollider;
    [SerializeField] protected SpriteRenderer m_SprtRenderer;                               // 角色圖片精靈
    public SpriteRenderer SprtRenderer => this.m_SprtRenderer;
    public void SetSpriteRenderer(SpriteRenderer sprtR) => this.m_SprtRenderer = sprtR;
    [SerializeField] protected Animator m_Animator;                                         // 角色動畫控制器
    public Animator Animator => this.m_Animator;


    [Header("")]
    public Transform CenterObj;                                                             
    public Transform ButtomObj;                                                             
    public CharactorData InfoStorage;                                                       // 角色相關資訊存取
    [SerializeField] protected Vector3 m_Center;                                            // 角色中心
    public Vector3 Center => this.m_Center;
    [SerializeField] protected Vector3 m_Buttom;                                            // 角色底部
    public Vector3 Buttom => this.m_Buttom;
    [SerializeField] protected Vector3 m_Coordinate;                                        // 角色橫縱高座標
    public Vector3 Coordinate => this.m_Coordinate;
    public void SetCoordinate(Vector3 coordinate) => this.m_Coordinate = coordinate;


    [Header("")]
    [SerializeField] protected int m_currHeight = 0;
    public int CurrentHeight => m_currHeight;
    public void SetCurrentHeight(int height) => this.m_currHeight = height;
    [SerializeField] protected int m_lastHeight = 0;
    public int LastHeight => m_lastHeight;
    public void SetLastHeight(int height) => this.m_lastHeight = height;
    
    [Header("")]
    [SerializeField] protected LamniatState m_lamniatStateController;                       // 角色狀態控制器
    public LamniatState StateController => this.m_lamniatStateController;
    [SerializeField] protected Movement_Lamniat m_lamniatMovement;                          // 可操作角色的移動控制
    public Movement_Lamniat LamniatMovement =>this.m_lamniatMovement ;


    protected bool m_onStairs;
    public bool OnStairs => this.m_onStairs;
    public void SetOnStairs(bool onStairs) => this.m_onStairs = onStairs;

    [Header("")]
    [SerializeField] private Vector3 m_raycastStart;
    public Vector3 RaycastStart => this.m_raycastStart;
    public void SetRaycastStart(Vector3 raycastStart) => this.m_raycastStart = raycastStart;
    private RaycastHit2D m_raycastHitJumpTrigger;
    public RaycastHit2D RaycastHitJumpTrigger => this.m_raycastHitJumpTrigger;
    public void SetRaycastHitJumpTrigger(RaycastHit2D raycastHitJumpTrigger) => this.m_raycastHitJumpTrigger = raycastHitJumpTrigger;
    public int RaycastJumpTriggerLayerMask;


    protected void Awake() {
        m_Coordinate = transform.position;
        //Debug.Log("controlTarget.InputCtrl asset: "+m_inputControls.asset);
    }
    protected void OnEnable() 
    {
        m_currHeight = InfoStorage.initialHeight;
        m_lastHeight = InfoStorage.initialHeight;
        transform.position = new Vector3(InfoStorage.initialPos.x, InfoStorage.initialPos.y);
    }

    protected void Start() {

        // 射線的除外遮罩
        // int ignoreRaycastLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        // int mapRangeLayerMask = 1 << LayerMask.NameToLayer("MapRange");
        // int hittableLayerMask = 1 << LayerMask.NameToLayer("Hittable");
        // int visableLayerMask = 1 << LayerMask.NameToLayer("Visable");
        // raycastJumpTriggerLayerMask = ~(ignoreRaycastLayerMask | mapRangeLayerMask | hittableLayerMask |visableLayerMask);
        RaycastJumpTriggerLayerMask = 1 << LayerMask.NameToLayer("HeightObj");
        
    }


    protected void Update()
    {
        if(m_onStairs) m_SprtRenderer.sortingLayerID = SortingLayer.NameToID("Character");
        else m_SprtRenderer.sortingLayerID = SortingLayer.NameToID("Default");

        m_Center = CenterObj?.position ?? Vector3.zero;
        m_Buttom = ButtomObj?.position ?? Vector3.zero;
        //Debug.Log("m_currentBaseState: "+m_currentBaseState.State);

        m_raycastStart = (Vector2)transform.position + m_bodyCollider.offset ;  // 射线的起点
        m_raycastHitJumpTrigger = Physics2D.Raycast(m_raycastStart, m_lamniatMovement.Movement.normalized, 1f, RaycastJumpTriggerLayerMask);
        Color color = m_raycastHitJumpTrigger.collider != null ? Color.red : Color.green;
        Debug.DrawLine(m_raycastStart, (Vector2)m_raycastStart + m_lamniatMovement.Movement.normalized*1f, color);
        if(m_raycastHitJumpTrigger.collider != null ) Debug.Log("m_raycastHit: "+m_raycastHitJumpTrigger.collider.name);

        float newZPosition = -m_currHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

    }

}
