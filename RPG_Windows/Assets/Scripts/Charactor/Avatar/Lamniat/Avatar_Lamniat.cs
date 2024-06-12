using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static JumpMechanismUtils;

public class Avatar_Lamniat : Avatar
{
    [Header("Avatar_Lamniat 基本物件")]
    
    #region Raycast物件
    [SerializeField] private Vector3 m_raycastStart;
    public Vector3 RaycastStart => this.m_raycastStart;
    private RaycastHit2D m_raycastHitJumpTrigger;
    public RaycastHit2D RaycastHitJumpTrigger => this.m_raycastHitJumpTrigger;
    private int raycastJumpTriggerLayerMask;

    #endregion

    protected override void Awake() {
        base.Awake();
        //Debug.Log("controlTarget.InputCtrl asset: "+m_inputControls.asset);
    }
    protected override void OnEnable() 
    {

        m_idle = new IdleState_Lamniat(this);
        m_move = new MoveState_Lamniat(this);
        m_attack = new AttackState_Lamniat(this);
        m_jump = new JumpState_Lamniat(this);
        m_dodge = new DodgeState_Lamniat(this);
        m_hurt = new HurtState_Lamniat(this);
        m_dead = new DeadState_Lamniat(this);
        
        //m_inputControls = new AvatarInputActionsControls();
        //m_inputControls.Lamniat_Land.Enable();
        base.OnEnable();
    }

    protected override void Start() {
        base.Start();

        // 射線的除外遮罩
        // int ignoreRaycastLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        // int mapRangeLayerMask = 1 << LayerMask.NameToLayer("MapRange");
        // int hittableLayerMask = 1 << LayerMask.NameToLayer("Hittable");
        // int visableLayerMask = 1 << LayerMask.NameToLayer("Visable");
        // raycastJumpTriggerLayerMask = ~(ignoreRaycastLayerMask | mapRangeLayerMask | hittableLayerMask |visableLayerMask);
        raycastJumpTriggerLayerMask = 1 << LayerMask.NameToLayer("HeightObj");
        
    }


    protected override void Update()
    {
        base.Update();
        //Debug.Log("m_currentBaseState: "+m_currentBaseState.State);

        m_raycastStart = (Vector2)transform.position + m_bodyCollider.offset ;  // 射线的起点
        m_raycastHitJumpTrigger = Physics2D.Raycast(m_raycastStart, m_avatarMovement.Movement.normalized, 1f, raycastJumpTriggerLayerMask);
        Color color = m_raycastHitJumpTrigger.collider != null ? Color.red : Color.green;
        Debug.DrawLine(m_raycastStart, (Vector2)m_raycastStart + m_avatarMovement.Movement.normalized*1f, color);
        if(m_raycastHitJumpTrigger.collider != null ) Debug.Log("m_raycastHit: "+m_raycastHitJumpTrigger.collider.name);

        float newZPosition = -m_currHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
    }

}
