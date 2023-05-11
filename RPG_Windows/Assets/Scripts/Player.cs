using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static JumpMechanismUtils;

public class Player : Charactor
{
    [Header("Player Parameters")]
    [SerializeField] private Transform raycastPoint;
    public Vector2 rayCastEndPos;
    public string onStairs;
    public string stair_start;
    public string stair_end ;

    [Header(" Parameters")]
    private bool prepareToJump = false;
    private bool OnCollisioning = false;
    private bool processingPushback = false;
    [SerializeField] private ColliderTrigger jumpPoint;
    private bool jumpPointTrigger = false;
    
    [Header("Input Settings")]
    [SerializeField] private InputActionReference movementActionReference;
    private bool isHoldInteraction = false;
    private Vector2 holdActionInput = Vector2.zero;
    
    protected override void Start() {
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + new Vector2(0, -1) * 0.35f;   // 預設射線終點
        base.Start();
        attackClipTime = AnimeUtils.GetAnimateClipTime(m_Animator, "Attack_Down");
        transform.position = infoStorage.initialPos;

        movementActionReference.action.started += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            if(content.interaction is HoldInteraction) {
                //Debug.Log("HoldInteraction started");
                //Debug.Log("inputVecter2: "+inputVecter2);
            }
        };

        movementActionReference.action.performed += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            if(content.interaction is HoldInteraction) {
                //Debug.Log("HoldInteraction performed");
                //Debug.Log("inputVecter2: "+inputVecter2);
                holdActionInput = inputVecter2;
                isHoldInteraction = true;
            }
        };
        
        movementActionReference.action.canceled += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            if(content.interaction is HoldInteraction) {
                //Debug.Log("HoldInteraction canceled");
                //Debug.Log("inputVecter2: "+inputVecter2);
                holdActionInput = Vector2.zero;
                isHoldInteraction = false;
            }
        };

        jumpPoint.OnPlayerEnterTrigger += () => {
            Debug.Log("ColliderTrigger jumpPointTrigger = true");
            jumpPointTrigger = true;
        };
    }

    protected override void Update()
    {
        if(!string.IsNullOrEmpty(onStairs)) {
            HeightSettleOnStair(onStairs);
        }
        else if(!groundDelaying) {
            if(!isJumping) {
                //DetectedToJump();
                if(!prepareToJump) {
                    jumpState = DetectedWhetherNeedToJump();
                }
                
                switch(jumpState) {
                    case JumpState.Ground:
                        break;
                    case JumpState.JumpDown:
                        TriggerToJumpDown();
                        // if(!processingPushback) {
                        //     TriggerToJumpDown();
                        // } else {
                        //     m_Rigidbody.AddForce((-new Vector2(movement.x, movement.y))* moveSpeed, ForceMode2D.Force);
                        // }
                        break;
                    case JumpState.JumpUp:
                        TriggerToJumpUp();
                        break;
                }
            } 
            // else if(!jumpHitColli) {
            //     DetectedWhileJump();
            // }
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
        //Debug.Log("inputVecter2: "+inputVecter2);
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
        //Debug.Log("OnCollisionEnter2D: "+other.gameObject.name);
        OnCollisioning = true;
    }

    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("OnCollisionStay2D: "+other.gameObject.name);
        OnCollisioning = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        //Debug.Log("OnCollisionExit2D: "+other.gameObject.name);
        OnCollisioning = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("OnTriggerEnter2D: "+other.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log("OnTriggerStay2D: "+other.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("OnTriggerStay2D: "+other.gameObject.name);
    }

    private void ChangeColliderToJumpDown() {
        var body = GetComponent<BoxCollider2D>();
        body.isTrigger = true;
        // var jumpTrigger = GetComponent<CircleCollider2D>();
        // if(body != null && jumpTrigger != null) {
        //     jumpTrigger.enabled = true;
        // }
    }

    private void RevertColliderFromJumpDown() {
        var body = GetComponent<BoxCollider2D>();
        body.isTrigger = false;
        //var jumpTrigger = GetComponent<CircleCollider2D>();
        // if(body != null && jumpTrigger != null) {
        //     jumpTrigger.enabled = false;
        // }
    }

    #endregion
    
    private void SetRaycastPoint(string raycastPointName = null) {
        if(raycastPointName != null) {
            raycastPoint = GetComponentInChildren<Transform>().Find(raycastPointName);
        } else {
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
    }

    private JumpState DetectedWhetherNeedToJump() {
        if(isMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(movement.x, movement.y) * 0.5f;
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
        Debug.Log("TriggerToJumpDown start");
        prepareToJump = true;
        ChangeColliderToJumpDown();
        SetRaycastPoint("RaycastPoint_Down");

        Vector2 distance = new Vector2(movement.x, movement.y) * 0.35f;
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
        //Debug.Log("castEndPos: "+rayCastEndPos);
        Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.red);

        RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));
        if(hits.Length >= 1) {
            Debug.Log("TriggerToJumpDown hits.Length > 1");
            foreach(RaycastHit2D hit in hits) {
                Debug.Log("TriggerToJumpDown hits collider name: "+hit.collider.name);
                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    if(jumpPointTrigger) {
                        Debug.Log("TriggerToJumpDown jumpPointTrigger");
                         
                        //if(isHoldInteraction && facingDir.Equals(new Vector2(movement.x, movement.y))) {  
                        if(isHoldInteraction && holdActionInput.Equals(new Vector2(movement.x, movement.y))) {
                            Debug.Log("TriggerToJumpDown prepareToJump = false");
                            prepareToJump = false;
        
                            if(!isJumping) {
                                takeOffCoord = m_Coordinate;
                                takeOffDir = facingDir;
                                isJumping = true;
                                maxJumpHeight = currHeight + 1.5f;
                                RevertColliderFromJumpDown();
                                Debug.Log("takeOffPos: "+takeOffCoord);
                            }
                            jumpPointTrigger = false;
                        }
                        else {
                            Debug.Log("TriggerToJumpDown KnockbackFeedback");
                            Debug.Log("TriggerToJumpDown isHoldInteraction "+isHoldInteraction);
                            Debug.Log("TriggerToJumpDown facingDir "+facingDir);
                            Debug.Log("TriggerToJumpDown new Vector2(movement.x, movement.y) "+new Vector2(movement.x, movement.y));
                            KnockbackFeedback feedback = GetComponent<KnockbackFeedback>();
                            feedback.ActiveFeedbackByDir(-new Vector2(movement.x, movement.y));
                            jumpPointTrigger = false;
                        }
                    }

                    // if(!jumpDelaying) {
                    //     jumpDelayRoutine = StartCoroutine(JumpDownDelay());
                    //     //jumpHitColli = true;
                    //     // if(!isHoldInteraction || (isHoldInteraction && !new Vector2(movement.x, movement.y).Equals(facingDir))) {
                    //     //     Debug.Log("TriggerToJumpDown pushBack");
                    //     //     jumpDelayRoutine = StartCoroutine(ProcessPushBack());
                    //     // } else {
                    //     //     Debug.Log("TriggerToJumpDown jumpDelay");
                    //     //     jumpDelayRoutine = StartCoroutine(JumpDownDelay());
                    //     // }
                    // }
                    
                    //m_Rigidbody.AddForce((-new Vector2(movement.x, movement.y))* moveSpeed, ForceMode2D.Force);
                }
            }
        }
        Debug.Log("TriggerToJumpDown end");
    }

    private void TriggerToJumpUp() {
        Debug.Log("TriggerToJumpUp start");
        prepareToJump = true;
        SetRaycastPoint();

        Vector2 distance = new Vector2(movement.x, movement.y) * 0.35f;
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
        //Debug.Log("castEndPos: "+rayCastEndPos);
        Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.red);

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
        Debug.Log("TriggerToJumpUp end");
    }

    #region 跳躍控制
    private void HandleJumpingProcess(JumpState state) {
        Debug.Log("currHeight start: "+currHeight);
        Debug.Log("lastHeight start: "+lastHeight);

        // Debug.Log("jumpOffset start: "+jumpOffset);
        // Debug.Log("jumpIncrement end: "+jumpIncrement);
        float goalheight = currHeight + jumpOffset;
        if(goalheight >= maxJumpHeight) {
            goalheight = maxJumpHeight;
        }
        Debug.Log("goalheight: "+goalheight);


        // if(facingDir == takeOffDir) {
        //     moveSpeed = 5f;
        // } else {
        //     moveSpeed = 14f;
        // }

        if(jumpOffset >= 0) {
            jumpIncrement += jumpOffset;
            lastHeight = currHeight;
            
            currHeight = goalheight;
            
            jumpOffset += (g / 2); 
        } 
        else {
            
            var hm = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;

            float groundCheckHeight = Mathf.Floor(currHeight);

            Debug.Log("jumpstate: "+state);
            Debug.Log("Center: "+m_Center);
            Debug.Log("Coordinate: "+m_Coordinate);
            Debug.Log("transform_pos: "+transform.position);

            if(state != JumpState.JumpUp) {
                
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
            else {
                var jumpPointCollider = GetComponent<BoxCollider2D>();
                var jumpPointColliderPoint1 = jumpPointCollider.bounds.min;
                var jumpPointColliderPoint2 = jumpPointCollider.bounds.max;
                var jumpPointColliderPoint3 = new Vector2(jumpPointColliderPoint1.x, jumpPointColliderPoint2.y);
                var jumpPointColliderPoint4 = new Vector2(jumpPointColliderPoint2.x, jumpPointColliderPoint1.y);

                Debug.Log("jumpPointColliderPoint1: "+jumpPointColliderPoint1);
                Debug.Log("jumpPointColliderPoint2: "+jumpPointColliderPoint2);
                Debug.Log("jumpPointColliderPoint3: "+jumpPointColliderPoint3);
                Debug.Log("jumpPointColliderPoint4: "+jumpPointColliderPoint4);

                groundCheckHeight = Mathf.Floor(goalheight);

                if(hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint1, currHeight, goalheight, movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint2, currHeight, goalheight, movement), groundCheckHeight) ||
                   hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint3, currHeight, goalheight, movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint4, currHeight, goalheight, movement), groundCheckHeight)) {
                    Debug.Log("PredictNext Groundable true, groundCheckHeight: "+groundCheckHeight);
                    // if(hm.NotGroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint1, currHeight, goalheight, movement), groundCheckHeight) ||
                    //    hm.NotGroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint2, currHeight, goalheight, movement), groundCheckHeight) ||
                    //    hm.NotGroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint3, currHeight, goalheight, movement), groundCheckHeight) ||
                    //    hm.NotGroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint4, currHeight, goalheight, movement), groundCheckHeight)) {
                    //     // NotGroundable(ex: 岩壁)判定
                    //     Debug.Log("NotGroundable true");
                    //     bool hasCeiling = hm.CeilingChecked(m_Center, groundCheckHeight); 
                    //     if(hasCeiling) {
                    //         lastHeight = currHeight;
                    //         currHeight = goalheight;
                    //         // currHeight = groundCheckHeight - 0.01f;
                    //         jumpOffset += g;
                    //     }
                    // } else {
                        // Debug.Log("NotGroundable false");
                        lastHeight = currHeight;
                        currHeight = groundCheckHeight;
                        FinishJump();
                        groundDelayRoutine = StartCoroutine(GroundDelay());
                } else {
                    Debug.Log("PredictNext Groundable false");
                    RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));

                    if(Mathf.Ceil(currHeight) - currHeight <= 0.5f) {
                        groundCheckHeight = Mathf.Ceil(currHeight);
                        if(hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint1, currHeight, goalheight, movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint2, currHeight, goalheight, movement), groundCheckHeight) ||
                           hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint3, currHeight, goalheight, movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPoint(jumpPointColliderPoint4, currHeight, goalheight, movement), groundCheckHeight)) {
                            Debug.Log("After Ceil Groundable true");
                            lastHeight = currHeight;
                            currHeight = groundCheckHeight;
                            FinishJump();
                            groundDelayRoutine = StartCoroutine(GroundDelay());
                        } else {
                            if(hits.Length >= 1) {
                                var variableVector = PredictNextJumpPoint(jumpPointColliderPoint1, currHeight, goalheight, movement) - jumpPointColliderPoint1;

                                foreach(RaycastHit2D hit in hits) {
                                    Debug.Log("TriggerToJumpUp hits collider name: "+hit.collider.name);
                                    var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                                    if(heightObj != null) {
                                        Vector2 surfaceVector = Vector2.Perpendicular(hit.normal);
                                        
                                        float dotResult = Vector3.Dot(variableVector, surfaceVector);
                                        Debug.Log("dotResult: "+dotResult);
                                        jumpingMovementVariable = dotResult;
                                        break;
                                    }
                                }
                            }
                            lastHeight = currHeight;
                            currHeight = goalheight;
                            jumpOffset += g;
                        }
                    } else {
                        if(hits.Length >= 1) {
                            var variableVector = PredictNextJumpPoint(jumpPointColliderPoint1, currHeight, goalheight, movement) - jumpPointColliderPoint1;

                            foreach(RaycastHit2D hit in hits) {
                                Debug.Log("TriggerToJumpUp hits collider name: "+hit.collider.name);
                                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                                if(heightObj != null) {
                                    Vector2 surfaceVector = Vector2.Perpendicular(hit.normal);
                                    
                                    float dotResult = Vector3.Dot(variableVector, surfaceVector);
                                    Debug.Log("dotResult: "+dotResult);
                                    jumpingMovementVariable = dotResult;
                                    break;
                                }
                            }
                        }
                        lastHeight = currHeight;
                        currHeight = goalheight;
                        jumpOffset += g; 
                    }
                }
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

        private Vector3 PredictNextJumpPoint(Vector3 point, float lastH, float currH, Vector3 movement) {
            Vector3 result = Vector3.zero;
            result.x = point.x;
            //result.z = currH;
            result.y = point.y - lastH;

            result = result + (Vector3)(m_Rigidbody.velocity * Time.deltaTime);

            return result;
        }

    protected IEnumerator GroundDelay() {
        groundDelaying = true;
        yield return new WaitForSeconds(groundDelay);  // hardcasted casted time for debugged
        //FinishJump();
        groundDelaying = false;
    }

    protected IEnumerator ProcessPushBack() {
        jumpHitColli = true;
        processingPushback = true;
        yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
        processingPushback = false;
        jumpHitColli = false;
    }

    protected IEnumerator JumpDownDelay() {
        jumpDelaying = true;
        yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
        jumpDelaying = false;
        prepareToJump = false;
        
        if(!isJumping) {
            takeOffCoord = m_Coordinate;
            takeOffDir = facingDir;
            isJumping = true;
            maxJumpHeight = currHeight + 1.5f;
            RevertColliderFromJumpDown();
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
        prepareToJump = false;
        
        if(!isJumping) {
            takeOffCoord = m_Coordinate;
            takeOffDir = facingDir;
            isJumping = true;
            maxJumpHeight = currHeight + 1.5f;
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
        moveSpeed = 11f;
        jumpingMovementVariable = 0.5f;

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
