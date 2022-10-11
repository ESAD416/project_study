using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Horizontal : Enemy_Abstract
{
    private SpriteRenderer m_SprtRenderer;
    public bool moveRight;

    protected override void Start() {
        m_SprtRenderer = GetComponentInChildren<SpriteRenderer>();
        base.Start();
    }

    protected override void Update() {
        if(isPatroling) {
            if(moveRight) {
                movement = Vector3.right;
            } else {
                movement = Vector3.left;
            }
        } else if(isChasing) {
            moveRight = AnimeUtils.isRightForHorizontalAnimation(movement);
        }
        
        m_SprtRenderer.flipX = moveRight;
        base.Update();
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
