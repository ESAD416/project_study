using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Horizontal : Enemy_Abstract
{
    private SpriteRenderer m_SprtRenderer;

    [Header("Detector Parameters")]
    public bool moveRight;
    [SerializeField] private Transform leftDetector;
    [SerializeField] private Transform rightDetector;

    protected override void Start() {
        attackClipTime = AnimeUtils.GetAnimateClipTime(m_Animator, "Attack_Down");
        m_SprtRenderer = GetComponentInChildren<SpriteRenderer>();
        base.Start();
    }

    protected override void Update() {
        if(!isDead) {
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
            leftDetector.gameObject.SetActive(!moveRight);
            rightDetector.gameObject.SetActive(moveRight);
        } else {
            leftDetector.gameObject.SetActive(false);
            rightDetector.gameObject.SetActive(false);
        }

        base.Update();
        // Debug.Log("Enemy_Horizontal movement" + movement);
        // Debug.Log("Enemy_Horizontal moveRight" + moveRight);
        // Debug.Log("Enemy_Horizontal movement" + movement);
        // Debug.Log("Enemy_Horizontal moveRight" + moveRight);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (moveRight) {
			moveRight = false;
        }
        else{
            moveRight = true;
        }	
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
