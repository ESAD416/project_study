using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_HorizontalOscillation : Movement_Enemy
{
    public bool moveRight;

    protected override void FixedUpdate() {
        if(!m_enemy.CurrentBaseState.Equals(BaseStateMachine_Enemy.BaseState.Dead)) {
            if(moveRight) SetMovement(Vector3.right);
            else SetMovement(Vector3.left);
            // if(m_enemy.CurrentEnemyState.State.Equals(EnemyStateMachine.EnemyState.Patrol)) {
                
            // } 
            // else if(m_enemy.CurrentEnemyState.State.Equals(EnemyStateMachine.EnemyState.Chase)) {
            //     // TODO: chase logic

            //     // if(movement.Equals(Vector3.zero)) {
            //     //     moveRight = AnimeUtils.isRightForHorizontalAnimation(movementAfterAttack);
            //     // }
            //     //moveRight = AnimeUtils.isRightForHorizontalAnimation(Movement);
            // }
            
            if(m_enemySprtRenderer!= null) m_enemySprtRenderer.flipX = moveRight;
        }

        base.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D trig)
	{
		if (trig.gameObject.tag == "Enemy_Turn"){

			if (moveRight){
				moveRight = false;
			}
			else{
				moveRight = true;
			}	
		}
	}
}
