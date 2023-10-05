using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Boss2 : BaseStateMachine_Enemy
{
    public IdleState_Boss2(Enemy enemy) 
    {
        this.currentEnemy = enemy;
        this.m_bState = BaseState.Idle;
    }

    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnEnter(Enemy enemy)
    {
        this.currentEnemy = enemy;
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
