using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine_Avatar : BaseStateMachine_Charactor
{
    protected Avatar currentAvatar;

    public abstract void OnEnter(Avatar avatar);
    public abstract override void OnUpdate();
    public abstract override void OnFixedUpdate();
    public abstract override void OnExit();
}
