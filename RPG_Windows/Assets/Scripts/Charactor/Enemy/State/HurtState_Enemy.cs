using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Enemy : BaseStateMachine_Enemy
{
    public HurtState_Enemy(Enemy enemy) {
        this.currentEnemy = enemy;
        this.m_bState = Constant.BaseState.Hurt;
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
