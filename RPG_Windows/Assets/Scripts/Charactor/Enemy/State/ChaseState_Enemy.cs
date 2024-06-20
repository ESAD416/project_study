using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState_Enemy : EnemyStateMachine
{
    public ChaseState_Enemy(Enemy enemy) {
        this.currentEnemy = enemy;
        this.m_eState = Constant.EnemyState.Chase;
    }

    public override void OnEnter()
    {
        // OnEnter
    }
    public override void OnEnter(Enemy enemy)
    {
        this.currentEnemy = enemy;
        OnEnter();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        // 物理引擎相關的Update
    }

    public override void OnExit()
    {
        // OnExit
    }
}
