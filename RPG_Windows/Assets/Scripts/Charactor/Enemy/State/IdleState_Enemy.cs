using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Enemy : BaseStateMachine_Enemy
{
    public IdleState_Enemy(Enemy enemy) {
        this.currentEnemy = enemy;
        this.m_bState = BaseState.Attack;
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
