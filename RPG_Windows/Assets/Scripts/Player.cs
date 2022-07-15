using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class Player : Charactor
{
    [Header("Input Settings")]
    public PlayerInput playerInput;

    [SerializeField] private Transform raycastPoint;
    public Vector2 rayCastEndPos;


    public string onStairs;
    public string stair_start;
    public string stair_end ;

    
    protected override void Start() {
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + new Vector2(0, -1) * 0.35f;   // 預設射線終點
        base.Start();
    }

    protected override void Update()
    {
        if(!string.IsNullOrEmpty(onStairs)) {
            HeightSettleOnStair(onStairs);
        }
        else if(!isJumping) {
            DetectedToJump();
        }
        //Debug.Log("GetCoordinate: "+GetCoordinate());
        base.Update();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputVecter2 = value.ReadValue<Vector2>();
        bool noInputMovement = inputVecter2.x == 0 && inputVecter2.y == 0;
        if(noInputMovement && movement.x != 0 || movement.y != 0) {
            facingDir = movement;
        }

        movement = new Vector3(inputVecter2.x, inputVecter2.y);
        // Debug.Log("movement x: "+movement.x);
        // Debug.Log("movement y: "+movement.y);
        // Debug.Log("movement normalized x: "+movement.normalized.x);
        // Debug.Log("movement normalized y: "+movement.normalized.y); 
    }

    public void OnAttack(InputAction.CallbackContext value) {
        if(value.started) {
            if(isMoving) {
                facingDir = movement;
            }
            attackRoutine = StartCoroutine(Attack());
        }
    }

    private void DetectedToJump() {
        if(isMoving) {
            if(movement.x == 0 && movement.y > 0) {
                // Up
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Up");
            } else if(movement.x == 0 && movement.y < 0) {
                // Down 
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Down");
            } else if(movement.x < 0 && movement.y == 0) {
                // Left
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Left");
            } else if(movement.x > 0 && movement.y == 0) {
                // Right
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Right");
            } else if(movement.x > 0 && movement.y > 0) {
                // UpRight
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_UpRight");
            } else if(movement.x < 0 && movement.y > 0) {
                // UpLeft
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_UpLeft");
            } else if(movement.x > 0 && movement.y < 0) {
                // DownRight
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_DownRight");
            } else if(movement.x < 0 && movement.y < 0) {
                // DownLeft
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_DownLeft");
            }

            Vector2 distance = new Vector2(movement.x, movement.y) * 0.35f;
            rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.blue);

            // 偵測跳躍Edge ver.3
            RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));
            if(hits.Length > 0) {
                var heightManager = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager; 
                float altitudeVariation = 0f;
                bool jumpUp = false;
                bool jumpDown = false;

                if(hits.Length >= 1) {
                    Debug.Log("hits.Length > 1");
                    foreach(RaycastHit2D hit in hits) {
                        Debug.Log("hits collider name: "+hit.collider.name);
                        var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                        if(heightObj != null) {
                            float correspondHeight = heightObj.GetCorrespondHeight();
                            float selfHeight = heightObj.GetSelfHeight();
                            altitudeVariation = Math.Abs(currHeight - correspondHeight) ;
                            var angle = Vector2.Angle((Vector2)raycastPoint.position - rayCastEndPos, hit.normal);
                            angle = 90.0f - Mathf.Abs(angle);
                            Debug.Log("linecast angle:"+angle);

                            if(currHeight < correspondHeight && altitudeVariation > 0 && altitudeVariation <= 1) {
                                // jumpUp
                                if(angle >= 60f && 180f - angle >= 60f) {
                                    jumpUp = true;
                                }
                            } else if(currHeight >= correspondHeight) {
                                // jumpDown
                                jumpDown = true;
                            }
                        }
                    }
                }

                if(jumpUp || jumpDown) {
                    if(!isJumping) {
                        takeOffPos = m_Center;
                        isJumping = true;
                    }
                }
            }
        } 
    }

    private void HeightSettleOnStair(string stairName) {
        var stairTriggers = GameObject.FindObjectsOfType(typeof(StairsTrigger)) as StairsTrigger[];
        StairsTrigger currStair = null;
        foreach(StairsTrigger stair in stairTriggers) {
            if(stair.gameObject.name == onStairs) {
                currStair = stair;
                break;
            }
        }

        if(currStair != null) {
            currHeight = currStair.SetPlayerHeightOnStair();
            Debug.Log("SetPlayerHeightOnStair player height: "+currHeight);

            transform.position = new Vector3(transform.position.x, transform.position.y, currHeight);
        }

    }

    private IEnumerator Attack() {
        //Debug.Log("attack start");
        isAttacking = true;
        m_Animator.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(attackClipTime);  // hardcasted casted time for debugged
        StopAttack();
    }

    private IEnumerator Jump() {
        isJumping = true;
        yield return new WaitForSeconds(jumpClipTime);  // hardcasted casted time for debugged
        StopJump();
    }

}
