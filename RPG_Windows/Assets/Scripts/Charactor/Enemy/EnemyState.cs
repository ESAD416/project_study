using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState {

    protected Enemy currentEnemy;

    public abstract void OnEnter(Enemy enemy);
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();
}
