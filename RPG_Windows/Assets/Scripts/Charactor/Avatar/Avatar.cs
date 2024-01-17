using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using static JumpMechanismUtils;

public class Avatar : Charactor
{
    #region 可操作角色物件

    [Header("Avatar基本物件")]
    [SerializeField] protected Movement_Avatar m_avatarMovement;
    /// <summary>
    /// 可操作角色的移動控制
    /// </summary>
    public Movement_Avatar AvatarMovement =>this.m_avatarMovement;

    [SerializeField] protected Jump_Lamniat m_avatarJump;
    /// <summary>
    /// 可操作角色的跳躍機制
    /// </summary>
    public Jump_Lamniat AvatarJump => this.m_avatarJump;

    protected AvatarInputActionsControls m_inputControls;
    /// <summary>
    /// 可操作角色的使用者輸入
    /// </summary>
    public AvatarInputActionsControls InputCtrl => this.m_inputControls;
    protected bool isHoldInteraction = false;

    #region Raycast物件

    [SerializeField] private List<Transform> m_raycastStartPosition = new List<Transform>();
    public List<Transform> RaycastStartPosition => this.m_raycastStartPosition;
    [SerializeField] private Transform m_raycastStart;
    public Transform RaycastStart => this.m_raycastStart;
    public Vector2 RaycastEnd;

    #endregion


    public string onStairs;
    public string stair_start;
    public string stair_end ;

    #endregion

    #region 可操作角色狀態

    private BaseStateMachine_Avatar m_currentBaseState;
    public BaseStateMachine_Avatar CurrentBaseState => this.m_currentBaseState;
    public void SetCurrentBaseState(BaseStateMachine_Avatar state) {
        this.m_currentBaseState.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }

    protected BaseStateMachine_Avatar m_idle;
    public BaseStateMachine_Avatar Idle => m_idle;
    protected BaseStateMachine_Avatar m_move;
    public BaseStateMachine_Avatar Move => m_move;
    protected BaseStateMachine_Avatar m_dodge;
    public BaseStateMachine_Avatar Dodge => m_dodge;
    protected BaseStateMachine_Avatar m_attack;
    public BaseStateMachine_Avatar Attack => m_attack;
    protected BaseStateMachine_Avatar m_hurt;
    public BaseStateMachine_Avatar Hurt => m_hurt;
    protected BaseStateMachine_Avatar m_dead;
    public BaseStateMachine_Avatar Dead => m_dead;

    #endregion

    protected override void Awake() {
        base.Awake();

        m_inputControls = new AvatarInputActionsControls();
    }

    protected override void OnEnable() {
        base.OnEnable();
        
        m_currHeight = m_InfoStorage.initialHeight;
        RaycastEnd = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + new Vector2(0.35f, 0) ;   // 預設射線終點
        transform.position = new Vector3(m_InfoStorage.initialPos.x, m_InfoStorage.initialPos.y, m_InfoStorage.initialHeight);

        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    {
        m_currentBaseState.OnUpdate();

        base.Update();
        // Debug.Log("m_currentBaseState: "+m_currentBaseState.State);
    }

    protected override void FixedUpdate() {
        m_currentBaseState.OnFixedUpdate();
        // if(isJumping && !CharStatus.Equals(CharactorStatus.Attack)) {
        //     transform.position = GetWorldPosByCoordinate(m_Coordinate) - new Vector3(0, 1.7f);   // 預設中心點是(x, y+1.7)
        //     HandleJumpingProcess(jumpState);
        // }

        base.FixedUpdate();
    }

    protected virtual void OnDisable() {
        m_currentBaseState.OnExit();
    }

    #region 碰撞偵測

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("OnTriggerEnter2D: "+other.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D other) {
        //Debug.Log("OnTriggerStay2D: "+other.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D other) {
        //Debug.Log("OnTriggerStay2D: "+other.gameObject.name);
    }

    private void ChangeColliderToJumpDown() {
        var body = GetComponent<BoxCollider2D>();
        body.isTrigger = true;
        // var jumpTrigger = GetComponent<CircleCollider2D>();
        // if(body != null && jumpTrigger != null) {
        //     jumpTrigger.enabled = true;
        // }
    }

    private void RevertColliderFromJumpDown() {
        var body = GetComponent<BoxCollider2D>();
        body.isTrigger = false;
        //var jumpTrigger = GetComponent<CircleCollider2D>();
        // if(body != null && jumpTrigger != null) {
        //     jumpTrigger.enabled = false;
        // }
    }

    #endregion
    
    public void SetRaycastPoint(string raycastPointName = null) {
        if(raycastPointName != null) {
            //m_raycastStart = GetComponentInChildren<Transform>().Find(raycastPointName);
            m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals(raycastPointName));
        } else {
            if(AvatarMovement.Movement.x == 0 && AvatarMovement.Movement.y > 0) {
                // Up
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Up");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Up"));
            } else if(AvatarMovement.Movement.x == 0 && AvatarMovement.Movement.y < 0) {
                // Down 
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Down");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Down"));
            } else if(AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y == 0) {
                // Left
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Left");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Left"));
            } else if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y == 0) {
                // Right
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Right");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Right"));
            } else if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y > 0) {
                // UpRight
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_UpRight");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_UpRight"));
            } else if(AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y > 0) {
                // UpLeft
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_UpLeft");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_UpLeft"));
            } else if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y < 0) {
                // DownRight
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_DownRight");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_DownRight"));
            } else if(AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y < 0) {
                // DownLeft
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_DownLeft");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_DownLeft"));
            }
        }
    }

    public bool IsObliqueRaycast() {
        if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y > 0 || AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y > 0 ||
           AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y < 0 || AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y < 0) {
            return true;
        }

        return false;
    }

    /*

    private void HeightSettleOnStair(string stairName) {
        var stairTriggers = GameObject.FindObjectsOfType(typeof(StairsTrigger)) as StairsTrigger[];
        StairsTrigger currStair = null;
        foreach(StairsTrigger stair in stairTriggers) {
            if(stair.gameObject.name == onStairs) {
                currStair = stair;
                break;
            }
        }

        if(currStair != null) {
            m_currHeight = currStair.SetPlayerHeightOnStair();
            Debug.Log("SetPlayerHeightOnStair player height: "+m_currHeight);

            transform.position = new Vector3(transform.position.x, transform.position.y, m_currHeight);
        }

    }

    */
}
