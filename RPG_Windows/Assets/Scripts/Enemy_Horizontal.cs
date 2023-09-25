using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Horizontal : Enemy_Abstract
{
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
        attackClipTime = 1f;

        detectorL = leftDetector ?? null;
        detectorR = rightDetector ?? null;
        m_SprtRenderer = GetComponentInChildren<SpriteRenderer>();
        base.Start();
    }

    protected override void Update() {
        //Debug.Log("Update movement: "+movement);
        base.Update();
        UpdateDetector();
        // Debug.Log("Enemy_Horizontal movement" + movement);
        // Debug.Log("Enemy_Horizontal moveRight" + moveRight);
        // Debug.Log("Enemy_Horizontal movement" + movement);
        // Debug.Log("Enemy_Horizontal moveRight" + moveRight);
    }

    protected override void FixedUpdate() {
        if(!isDead) {
            // Debug.Log("FixedUpdate isAttacking: "+isAttacking);
            // Debug.Log("FixedUpdate movement: "+movement);
            // Debug.Log("FixedUpdate movementAfterAttack: "+movementAfterAttack);
            if(isAttacking) {
                //Debug.Log("attacking");
                if(isMoving) {
                    movementAfterAttack = Movement;
                    //Debug.Log("movementAfterAttack: "+movementAfterAttack);
                }
                SetMovement(Vector3.zero);
            } else {
                if(isPatroling) {
                    if(moveRight) {
                        SetMovement(Vector3.right);
                    } else {
                        SetMovement(Vector3.left);
                    }
                } else if(isChasing) {
                    // if(movement.Equals(Vector3.zero)) {
                    //     moveRight = AnimeUtils.isRightForHorizontalAnimation(movementAfterAttack);
                    // }
                    moveRight = AnimeUtils.isRightForHorizontalAnimation(Movement);
                }

                if(detectorL != null) detectorL.gameObject.SetActive(!moveRight);
                if(detectorR != null) detectorR.gameObject.SetActive(moveRight);
            }
            
            if(m_SprtRenderer!= null) m_SprtRenderer.flipX = moveRight;
        } else {
            if(detectorL != null) detectorL.gameObject.SetActive(false);
            if(detectorR != null) detectorR.gameObject.SetActive(false);
        }

        base.FixedUpdate();
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

    public void UpdateDetector() {
        if(isChasing) {
            detectorL = leftAttackDetector ?? null;
            detectorR = rightAttackDetector ?? null;
        } else {
            detectorL = leftDetector ?? null;
            detectorR = rightDetector ?? null;
        }
    }
    
    public override void OnAttack() {
        //Debug.Log("Enemy_Horizontal onAttack: ");
        if(isMoving) {
            SetFacingDir(Movement);
        }

        if(detectorL != null) detectorL.gameObject.SetActive(false);
        if(detectorR != null) detectorR.gameObject.SetActive(false);
        attackRoutine = StartCoroutine(Attack());
    }

    public override void FinishAttack() {
        //Debug.Log("Enemy_Horizontal FinishAttack start");
        if(attackRoutine != null) {
            StopCoroutine(attackRoutine);
        }

        isAttacking = false;
        m_Animator.SetBool("attack", isAttacking);

        SetMovement(movementAfterAttack);
        movementAfterAttack = Vector3.zero;
        //Debug.Log("Enemy_Horizontal FinishAttack end");
    }
}
