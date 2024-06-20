using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeStartState_Boss : BossStateMachine
{

    public BeforeStartState_Boss(Enemy boss2)
    {
        this.m_stage = Constant.BossState.BeforeStart;
        this.currentEnemy = boss2;
    }

    public override void OnEnter()
    {
        // OnEnter
        this.currentEnemy.EnemyMovement.SetMoveSpeed(0f);
        this.currentEnemy.SetCurrentBaseState(this.currentEnemy.Idle);

        var hitSystem = this.currentEnemy.GetComponent<HitSystem_Enemy>();
        if(hitSystem) hitSystem.IsIgnoreHit = true;
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
