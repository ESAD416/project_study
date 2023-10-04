using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_HorizontalOscillation : Movement_Enemy
{
    //public Collider2D m_EnemyCollider;
    public bool moveRight;
    

    protected override void FixedUpdate() {
        if(!m_enemy.CharStatus.Equals(Charactor.CharactorStatus.Dead)) {
            if(m_enemy.EneStatus.Equals(Enemy.EnemyStatus.Patrol)) {
                if(moveRight) SetMovement(Vector3.right);
                else SetMovement(Vector3.left);
            } 
            else if(m_enemy.EneStatus.Equals(Enemy.EnemyStatus.Chase)) {
                // TODO: chase logic

                // if(movement.Equals(Vector3.zero)) {
                //     moveRight = AnimeUtils.isRightForHorizontalAnimation(movementAfterAttack);
                // }
                //moveRight = AnimeUtils.isRightForHorizontalAnimation(Movement);
            }
            
            if(m_enemySprtRenderer!= null) m_enemySprtRenderer.flipX = moveRight;
        }

        base.FixedUpdate();
    }

    private void OnTriggerEnter2D(Collider2D trig)
	{
        Debug.Log("OnTriggerEnter2D");
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
