using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossStateMachine
{
    public enum BossState {
        BeforeStart, DuringBattle, AnimePerformance, BattleFinish
    }

    protected BossState m_stage;
    public BossState Stage => m_stage;

    protected Enemy currentEnemy;

    public abstract void OnEnter();
    public abstract void OnEnter(Enemy enemy);
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}
