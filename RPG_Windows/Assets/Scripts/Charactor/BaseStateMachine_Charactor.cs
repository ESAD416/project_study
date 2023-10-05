using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine_Charactor
{
    public enum BaseState {
        Idle, Move, Attack, Dead,
    }
    protected BaseState m_bState;
    public BaseState State => m_bState;

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}
