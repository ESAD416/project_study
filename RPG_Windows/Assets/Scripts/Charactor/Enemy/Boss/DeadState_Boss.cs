using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Boss : BaseStateMachine_Enemy
{
    public DeadState_Boss(Enemy boss2)
    { 
        this.currentEnemy = boss2;
        this.m_bState = Constant.BaseState.Dead;
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
