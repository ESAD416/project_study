using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Boss2 : BaseStateMachine_Enemy
{
    public DeadState_Boss2(Enemy enemy)
    { 
        this.currentEnemy = enemy;
        this.m_bState = BaseState.Dead;
    }

    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnEnter(Enemy enemy)
    {
        throw new System.NotImplementedException();
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
