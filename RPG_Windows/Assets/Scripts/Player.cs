using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using static JumpMechanismUtils;

public class Player : Charactor
{
    [Header("Player Parameters")]
    [SerializeField] private Transform raycastPoint;
    public Vector2 rayCastEndPos;
    public string onStairs;
    public string stair_start;
    public string stair_end ;
    private bool OnCollisioning = false;
    
    [Header("Input Settings")]
    public PlayerInput playerInput;

    
    protected override void Start() {
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + new Vector2(0, -1) * 0.35f;   // 預設射線終點
        base.Start();
        attackClipTime = AnimeUtils.GetAnimateClipTime(m_Animator, "Attack_Down");
        transform.position = infoStorage.initialPos;
    }

    protected override void Update()
    {
        if(!string.IsNullOrEmpty(onStairs)) {
            HeightSettleOnStair(onStairs);
        }
        else if(!groundDelaying) {
            if(!isJumping) {
                //DetectedToJump();
                jumpState = DetectedWhetherNeedToJump();
                switch(jumpState) {
                    case JumpState.Ground:
                        break;
                    case JumpState.JumpDown:
                        TriggerToJumpDown();
                        break;
                    case JumpState.JumpUp:
                        TriggerToJumpUp();
                        break;
                }
            } else if(!jumpHitColli) {
                DetectedWhileJump();
            }
        }
        //Debug.Log("GetCoordinate: "+GetCoordinate());
        base.Update();
    }

    protected override void FixedUpdate() {
        if(isJumping && !isAttacking) {
            transform.position = GetWorldPosByCoordinate(m_Coordinate) - new Vector3(0, 1.7f);   // 預設中心點是(x, y+1.7)
            HandleJumpingProcess(jumpState);
        }

        base.FixedUpdate();
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

    #region 碰撞偵測

    private void OnCollisionEnter2D(Collision2D other) {
        //Debug.Log("OnCollisionEnter2D");
        OnCollisioning = true;
    }

    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("OnCollisionStay2D");
        OnCollisioning = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        //Debug.Log("OnCollisionExit2D");
        OnCollisioning = false;
    }

    #endregion
    
    private void SetRaycastPoint() {
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
    }

    private JumpState DetectedWhetherNeedToJump() {
        if(isMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(movement.x, movement.y) * 0.35f;
            rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.blue);

            return JumpMechanismUtils.DetectedJumpState(raycastPoint.position, rayCastEndPos, distance, currHeight, isMoving, OnCollisioning);
        }
        return JumpState.Ground;
    }

    private void DetectedToJump() {
        if(isMoving) {
            SetRaycastPoint();

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
                    Debug.Log("DetectedToJump hits.Length > 1");
                    foreach(RaycastHit2D hit in hits) {
                        Debug.Log("DetectedToJump hits collider name: "+hit.collider.name);
                        var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                        if(heightObj != null) {
                            float correspondHeight = heightObj.GetCorrespondHeight();
                            float selfHeight = heightObj.GetSelfHeight();
                            altitudeVariation = Math.Abs(currHeight - correspondHeight) ;
                            var angle = Vector2.Angle((Vector2)raycastPoint.position - rayCastEndPos, hit.normal);
                            angle = 90.0f - Mathf.Abs(angle);
                            Debug.Log("DetectedToJump linecast angle:"+angle);

                            if(currHeight < correspondHeight && altitudeVariation > 0 && altitudeVariation <= 1) {
                                // jumpUp
                                if(angle >= 60f && 180f - angle >= 60f) {
                                    jumpUp = true;
                                }
                            }  else if(currHeight >= correspondHeight) {
                            //else if(currHeight >= correspondHeight  && altitudeVariation > 0 && altitudeVariation <= 1) {
                                // jumpDown
                                jumpDown = true;
                            }
                        }
                    }
                }

                if(jumpUp || jumpDown) {
                    if(!isJumping) {
                        takeOffCoord = m_Coordinate;
                        takeOffDir = facingDir;
                        isJumping = true;
                        Debug.Log("takeOffPos: "+takeOffCoord);
                    }
                }
            }
        } 
    }

    private void DetectedWhileJump() {
        if(isMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(movement.x, movement.y) * 0.35f;
            rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.blue);

            RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));
            if(hits.Length > 0) {
                var heightManager = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;
                if(hits.Length >= 1) {
                    foreach(RaycastHit2D hit in hits) {
                        Debug.Log("DetectedWhileJump hits collider name: "+hit.collider.name);
                        var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                        if(heightObj != null) {
                            float correspondHeight = heightObj.GetCorrespondHeight();
                            float selfHeight = heightObj.GetSelfHeight();

                            if(currHeight < selfHeight && jumpOffset <= 0) {
                                Debug.Log("DetectedWhileJump correspondHeight: "+correspondHeight);
                                Debug.Log("DetectedWhileJump selfHeight: "+selfHeight);
                                Debug.Log("DetectedWhileJump jumpOffset: "+jumpOffset);
                                jumpHitColli = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private void TriggerToJumpDown() {
        SetRaycastPoint();

        Vector2 distance = new Vector2(movement.x, movement.y) * -0.35f;
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
        //Debug.Log("castEndPos: "+rayCastEndPos);
        Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.blue);

        RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));
        if(hits.Length >= 1) {
            Debug.Log("TriggerToJumpDown hits.Length > 1");
            foreach(RaycastHit2D hit in hits) {
                Debug.Log("TriggerToJumpDown hits collider name: "+hit.collider.name);
                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    if(!jumpDelaying) {
                        jumpDelayRoutine = StartCoroutine(JumpDownDelay());
                    }
                }
            }
        }
    }

    private void TriggerToJumpUp() {
        SetRaycastPoint();

        Vector2 distance = new Vector2(movement.x, movement.y) * 0.35f;
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
        //Debug.Log("castEndPos: "+rayCastEndPos);
        Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.blue);

        RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));
        if(hits.Length >= 1) {
            Debug.Log("TriggerToJumpUp hits.Length > 1");
            foreach(RaycastHit2D hit in hits) {
                Debug.Log("TriggerToJumpUp hits collider name: "+hit.collider.name);
                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    var angle = Vector2.Angle((Vector2)raycastPoint.position - rayCastEndPos, hit.normal);
                    angle = 90.0f - Mathf.Abs(angle);
                    Debug.Log("DetectedJumpState linecast angle:"+angle);
                    if(angle >= 60f && 180f - angle >= 60f && OnCollisioning) {
                        if(!jumpDelaying) {
                            Vector2 faceDirAtJump = facingDir;
                            jumpDelayRoutine = StartCoroutine(JumpUpDelay(faceDirAtJump));
                        }
                    }
                }
            }
        }
    }

    #region 跳躍控制
    private void HandleJumpingProcess(JumpState state) {
        Debug.Log("currHeight start: "+currHeight);
        Debug.Log("lastHeight start: "+lastHeight);
        Debug.Log("jumpOffset start: "+jumpOffset);
        Debug.Log("jumpIncrement end: "+jumpIncrement);
        float goalheight = currHeight + jumpOffset;

        Debug.Log("goalheight: "+goalheight);

        // if(facingDir == takeOffDir) {
        //     moveSpeed = 5f;
        // } else {
        //     moveSpeed = 14f;
        // }

        if(jumpOffset >= 0) {
            lastHeight = currHeight;
            currHeight = goalheight;
            jumpIncrement += jumpOffset;
            jumpOffset += (g / 2); 
        } 
        else {
            var hm = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;

            float groundCheckHeight = Mathf.Floor(currHeight);

            Debug.Log("Center: "+m_Center);
            Debug.Log("Coordinate: "+m_Coordinate);
            Debug.Log("transform_pos: "+transform.position);
            Vector3 shadowCoordinate = new Vector3(m_Coordinate.x, m_Coordinate.y, groundCheckHeight);
            Debug.Log("shadowCoordinate: "+shadowCoordinate);
            Vector3 shadowWorldPos = new Vector3(shadowCoordinate.x, shadowCoordinate.y + shadowCoordinate.z);

            if(hm.GroundableChecked(shadowWorldPos, groundCheckHeight)) {
            // if(hm.GroundableChecked(m_Coordinate)) {
                Debug.Log("Groundable true");
                if(goalheight <= groundCheckHeight) {
                    if(hm.NotGroundableChecked(m_Center, groundCheckHeight) || hm.NotGroundableChecked(shadowWorldPos, groundCheckHeight)) {
                        // NotGroundable(ex: 岩壁)判定
                        Debug.Log("NotGroundable true");
                        bool hasCeiling = hm.CeilingChecked(m_Center, groundCheckHeight); 
                        if(hasCeiling) {
                            lastHeight = currHeight;
                            currHeight = goalheight;
                            // currHeight = groundCheckHeight - 0.01f;
                            jumpOffset += g;
                        }
                        
                    } else {
                        Debug.Log("NotGroundable false");
                        lastHeight = currHeight;
                        currHeight = groundCheckHeight;
                        FinishJump();
                        groundDelayRoutine = StartCoroutine(GroundDelay());
                    }                    
                } else {
                    lastHeight = currHeight;
                    currHeight = goalheight;
                    jumpOffset += g;
                }
            } else {
                Debug.Log("Groundable false");
                lastHeight = currHeight;
                currHeight = goalheight;
                jumpOffset += g; 
            }
        }

        if(jumpOffset <= minjumpOffSet) {
            jumpOffset = minjumpOffSet;
        }
        
        Debug.Log("currHeight end: "+currHeight);
        Debug.Log("lastHeight end: "+lastHeight);
        Debug.Log("jumpOffset end: "+jumpOffset);
        Debug.Log("jumpIncrement end: "+jumpIncrement);
    }

    protected IEnumerator GroundDelay() {
        groundDelaying = true;
        yield return new WaitForSeconds(groundDelay);  // hardcasted casted time for debugged
        //FinishJump();
        groundDelaying = false;
    }

    protected IEnumerator JumpDownDelay() {
        jumpDelaying = true;
        yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
        jumpDelaying = false;
        
        if(!isJumping) {
            takeOffCoord = m_Coordinate;
            takeOffDir = facingDir;
            isJumping = true;
            Debug.Log("takeOffPos: "+takeOffCoord);
        }
    }

    protected IEnumerator JumpUpDelay(Vector2 faceDir) {
        jumpDelaying = true;
        // if button is change before delay time
        if(facingDir != faceDir) {
            jumpDelaying = false;
            yield break;
        }

        yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
        jumpDelaying = false;
        
        if(!isJumping) {
            takeOffCoord = m_Coordinate;
            takeOffDir = facingDir;
            isJumping = true;
            Debug.Log("takeOffPos: "+takeOffCoord);
        }
    }

    public void FinishJump() {
        if(groundDelayRoutine != null) {
            StopCoroutine(groundDelayRoutine);
        }

        Debug.Log("StopJump currHeight: "+currHeight);
        Debug.Log("StopJump takeOffPos: "+takeOffCoord);

        isJumping = false;
        jumpHitColli = false;
        groundDelaying = false;
        jumpOffset = 0.3f;
        lastHeight = currHeight;
        moveSpeed = 14f;

        transform.position = new Vector3(transform.position.x, transform.position.y, currHeight);
        takeOffCoord = Vector3.zero;
    }
    #endregion

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

}
