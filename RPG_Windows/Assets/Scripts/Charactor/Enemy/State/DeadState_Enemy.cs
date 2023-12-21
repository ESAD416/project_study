using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState_Enemy : BaseStateMachine_Enemy
{
    public DeadState_Enemy(Enemy enemy) {
        this.currentEnemy = enemy;
        this.m_bState = BaseState.Dead;
    }

    public override void OnEnter()
    {
    }
    public override void OnEnter(Enemy enemy)
    {
        this.currentEnemy = enemy;
        OnEnter();
    }


    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
