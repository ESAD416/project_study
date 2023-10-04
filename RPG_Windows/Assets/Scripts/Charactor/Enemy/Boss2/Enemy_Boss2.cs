using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss2 : Enemy
{

    protected override void Awake() {
        base.Awake();
        patrolState = new EnemyPatrolState();
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
