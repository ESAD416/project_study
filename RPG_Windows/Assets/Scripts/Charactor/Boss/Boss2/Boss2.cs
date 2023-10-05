using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy
{
    protected BossStateMachine m_beforeStart;
    protected BossStateMachine m_duringBattle;
    protected BossStateMachine m_bossPerformance;
    protected BossStateMachine m_battleFinish;

    protected override void Awake() {
        base.Awake();
        m_idle = new IdleState_Boss2(this);
        m_move = new MoveState_Boss2(this);
        m_dead = new DeadState_Boss2(this);


    }
    protected override void OnEnable() {
        base.OnEnable();
    }
    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }
    protected override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }
    
}
