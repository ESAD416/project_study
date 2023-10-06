using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateMachine
{
    public enum EnemyState {
        Patrol, Chase, 
    }
    protected EnemyState m_eState;
    public EnemyState State => m_eState;

    protected Enemy currentEnemy;

    public abstract void OnEnter();
    public abstract void OnEnter(Enemy enemy);
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}
