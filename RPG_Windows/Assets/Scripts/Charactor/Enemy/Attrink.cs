using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attrink : Enemy<BoxCollider2D>
{
    protected override void Update() {
        base.Update();
        
        if(m_Animator != null) m_Animator.SetFloat("moveSpeed", EnemyCurrentMovement.MoveSpeed);
    }
}
