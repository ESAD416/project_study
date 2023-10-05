using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Boss2 : BaseStateMachine_Enemy
{
    public MoveState_Boss2(Enemy enemy) 
    {
        this.currentEnemy = enemy;
        this.m_bState = BaseState.Move;
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
