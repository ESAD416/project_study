using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Boss : BaseStateMachine_Enemy
{
    public IdleState_Boss(Enemy boss2) 
    {
        this.currentEnemy = boss2;
        this.m_bState = BaseState.Idle;
    }

    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnEnter(Enemy boss2)
    {
        this.currentEnemy = boss2;
        OnEnter();
    }

    public override void OnUpdate()
    {
        // OnUpdate
    }

    public override void OnFixedUpdate()
    {
        // OnFixedUpdate
    }

    public override void OnExit()
    {
        // OnExit
    }
}
