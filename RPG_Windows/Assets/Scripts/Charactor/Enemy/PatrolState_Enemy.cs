using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState_Enemy : EnemyStateMachine
{
    public PatrolState_Enemy(Enemy enemy) {
        this.currentEnemy = enemy;
        this.m_eState = EnemyState.Patrol;
    }

    public override void OnEnter(Enemy enemy)
    {
        this.currentEnemy = enemy;
    }

    public override void OnUpdate()
    {
        // 發現Player(Avatar)就轉移至ChaseState
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
