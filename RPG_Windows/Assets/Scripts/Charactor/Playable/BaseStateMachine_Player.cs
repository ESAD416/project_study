using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine_Player : BaseStateMachine
{
    protected Player currentPlayer;

    public abstract void OnEnter(Player player);
    public abstract override void OnUpdate();
    public abstract override void OnFixedUpdate();
    public abstract override void OnExit();
}
