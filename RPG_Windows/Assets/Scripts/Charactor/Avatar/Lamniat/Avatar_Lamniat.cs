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

        m_idle = new IdleState_Lamniat(this);
        m_move = new MoveState_Lamniat(this);
        m_attack = new AttackState_Lamniat(this);
        m_dead = new DeadState_Lamniat(this);
    }

    protected override void OnEnable() {
        base.OnEnable();
        
        m_inputControls.Lamniat_Land.Enable();
    }

}
