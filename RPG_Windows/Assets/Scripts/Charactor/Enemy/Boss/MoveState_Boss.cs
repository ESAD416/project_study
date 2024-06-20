using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState_Boss : BaseStateMachine_Enemy
{
    public MoveState_Boss(Enemy boss2) 
    {
        this.currentEnemy = boss2;
        this.m_bState = Constant.BaseState.Move;
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
        AnimeUtils.ActivateAnimatorLayer(this.currentEnemy.Animator, "MoveLayer");
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
