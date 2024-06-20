using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFinishState_Boss : BossStateMachine
{
    public BattleFinishState_Boss(Enemy boss2)
    {
        this.m_stage = Constant.BossState.BattleFinish;
        this.currentEnemy = boss2;
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
