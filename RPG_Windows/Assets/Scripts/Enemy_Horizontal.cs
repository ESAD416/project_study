using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Horizontal : Enemy_Abstract
{
    private SpriteRenderer m_SprtRenderer;

    [Header("Detector Parameters")]
    public bool moveRight;

    private Transform detectorL;
    private Transform detectorR;

    [SerializeField] private Transform leftDetector;
    [SerializeField] private Transform rightDetector;

    [SerializeField] private Transform leftAttackDetector;
    [SerializeField] private Transform rightAttackDetector;

    protected override void Start() {
        //attackClipTime = AnimeUtils.GetAnimateClipTime(m_Animator, "Attack_Down");
        
        detectorL = leftDetector;
        detectorR = rightDetector;
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
            detectorL.gameObject.SetActive(!moveRight);
            detectorR.gameObject.SetActive(moveRight);
        } else {
            detectorL.gameObject.SetActive(false);
            detectorR.gameObject.SetActive(false);
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

    public void ChangeDetector() {
        var dL =  detectorL.GetComponent<AIDetector>();
        var dR =  detectorR.GetComponent<AIDetector>();

        bool partrolDetecting = (dL != null && dR != null);
        if(partrolDetecting) {
            detectorL = leftAttackDetector;
            detectorR = rightAttackDetector;
        } else {
            detectorL = leftDetector;
            detectorR = rightDetector;
        }
    }

    
}
