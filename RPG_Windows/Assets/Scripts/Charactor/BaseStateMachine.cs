using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine
{
    protected Constant.BaseState m_bState;
    public Constant.BaseState State => m_bState;

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}
