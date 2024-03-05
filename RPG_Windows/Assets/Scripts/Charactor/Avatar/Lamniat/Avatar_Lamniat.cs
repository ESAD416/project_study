using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static JumpMechanismUtils;

public class Avatar_Lamniat : Avatar
{
    protected override void Awake() {
        base.Awake();
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
        
        m_inputControls.Lamniat_Land.Enable();
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        // Debug.Log("m_currentBaseState: "+m_currentBaseState.State);
        float newZPosition = m_currHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
    }

}
