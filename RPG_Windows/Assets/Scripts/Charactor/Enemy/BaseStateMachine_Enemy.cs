using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine_Enemy: BaseStateMachine_Charactor
{

    protected Enemy currentEnemy;

    public abstract void OnEnter(Enemy enemy);
    public abstract override void OnUpdate();
    public abstract override void OnFixedUpdate();
    public abstract override void OnExit();
}
