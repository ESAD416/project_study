using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStateMachine
{
    public enum BossState {
        BeforeStart, DuringBattle, BossPerformance, BattleFinish
    }

    protected BossState m_BsState;
    public BossState State => m_BsState;

    protected Enemy currentEnemy;

    public abstract void OnEnter(Enemy enemy);
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}
