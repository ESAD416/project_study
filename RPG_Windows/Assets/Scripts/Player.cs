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
    //private bool OnCollisioning = false;
    private List<Collider2D> OnColliders = new List<Collider2D>();
    private bool processingPushback = false;
    [SerializeField] private ColliderTrigger jumpPoint;
    private bool jumpPointTrigger = false;
    
    [Header("Input Settings")]
    [SerializeField] private InputActionReference movementActionReference;

    [SerializeField] private InputActionReference holdActionReference;
    private bool isHoldInteraction = false;
    
    protected override void Start() {
        rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + new Vector2(0, -1) * 0.35f;   // 預設射線終點
        base.Start();
        attackClipTime = AnimeUtils.GetAnimateClipTime(m_Animator, "Attack_Down");
        transform.position = new Vector3(m_infoStorage.initialPos.x, m_infoStorage.initialPos.y, m_infoStorage.initialHeight);
        currHeight = m_infoStorage.initialHeight;

        movementActionReference.action.performed += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            facingDir = inputVecter2;
        };

        holdActionReference.action.started += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            if(content.interaction is HoldInteraction) {
                // Debug.Log("HoldInteraction started");
                // Debug.Log("inputVecter2: "+inputVecter2);
            }
        };

        holdActionReference.action.performed += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            if(content.interaction is HoldInteraction) {
                // Debug.Log("HoldInteraction performed");
                // Debug.Log("inputVecter2: "+inputVecter2);
                // Debug.Log("facingDir: "+facingDir);
                isHoldInteraction = true;
            }
        };
        
        holdActionReference.action.canceled += content => {
            var inputVecter2 = content.ReadValue<Vector2>();
            if(content.interaction is HoldInteraction) {
                // Debug.Log("HoldInteraction canceled");
                // Debug.Log("inputVecter2: "+inputVecter2);
                // Debug.Log("facingDir: "+facingDir);
                isHoldInteraction = false;
            }
        };

        jumpPoint.OnPlayerEnterTriggerEvent.AddListener(() => {
            //Debug.Log("ColliderTrigger jumpPointTrigger = true");
            jumpPointTrigger = true;
        });
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
        //Debug.Log("OnMovement inputVecter2: "+inputVecter2);
        SetMovement(new Vector3(inputVecter2.x, inputVecter2.y));
        // Debug.Log("movement x: "+movement.x);
        // Debug.Log("movement y: "+movement.y);
        // Debug.Log("movement normalized x: "+movement.normalized.x);
        // Debug.Log("movement normalized y: "+movement.normalized.y); 
    }

    public void OnAttack(InputAction.CallbackContext value) {
        if(value.started) {
            if(isMoving) {
                facingDir = Movement;
            }
            attackRoutine = StartCoroutine(Attack());
        }
    }

    #region 碰撞偵測

    private void OnCollisionEnter2D(Collision2D other) {
        //Debug.Log("OnCollisionEnter2D: "+other.gameObject.name);
        //OnCollisioning = true;
        OnColliders.Add(other.collider);
    }

    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("OnCollisionStay2D: "+other.gameObject.name);
        //OnCollisioning = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        //Debug.Log("OnColliders count: "+OnColliders.Count);
        //Debug.Log("OnCollisionExit2D: "+other.gameObject.name);
        var itemToRemove = OnColliders.Single(r => r.name.Equals(other.collider.name));
        OnColliders.Remove(itemToRemove);
        //Debug.Log("OnColliders count: "+OnColliders.Count);
        //OnCollisioning = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("OnTriggerEnter2D: "+other.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D other) {
        //Debug.Log("OnTriggerStay2D: "+other.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D other) {
        //Debug.Log("OnTriggerStay2D: "+other.gameObject.name);
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
            if(Movement.x == 0 && Movement.y > 0) {
                // Up
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Up");
            } else if(Movement.x == 0 && Movement.y < 0) {
                // Down 
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Down");
            } else if(Movement.x < 0 && Movement.y == 0) {
                // Left
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Left");
            } else if(Movement.x > 0 && Movement.y == 0) {
                // Right
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Right");
            } else if(Movement.x > 0 && Movement.y > 0) {
                // UpRight
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_UpRight");
            } else if(Movement.x < 0 && Movement.y > 0) {
                // UpLeft
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_UpLeft");
            } else if(Movement.x > 0 && Movement.y < 0) {
                // DownRight
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_DownRight");
            } else if(Movement.x < 0 && Movement.y < 0) {
                // DownLeft
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_DownLeft");
            }
        }
    }

    private bool IsObliqueRaycast() {
        if(Movement.x > 0 && Movement.y > 0 || Movement.x < 0 && Movement.y > 0 ||
           Movement.x > 0 && Movement.y < 0 || Movement.x < 0 && Movement.y < 0) {
            return true;
        }

        return false;
    }

    private JumpState DetectedWhetherNeedToJump() {
        if(isMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(Movement.x, Movement.y) * 0.5f;
            rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.blue);

            return JumpMechanismUtils.DetectedJumpState(raycastPoint.position, rayCastEndPos, distance, currHeight, isMoving);
        }
        return JumpState.Ground;
    }

    private void DetectedToJump() {
        if(isMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(Movement.x, Movement.y) * 0.35f;
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

            Vector2 distance = new Vector2(Movement.x, Movement.y) * 0.35f;
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

        RaycastHit2D[] hits = null;
        if(IsObliqueRaycast()) {
            Vector2 distance1 = new Vector2(Movement.x, 0) * 0.35f;
            var rayCastEndPos1 = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance1;
            Debug.DrawLine(raycastPoint.position, rayCastEndPos1, Color.red);
            Vector2 distance2 = new Vector2(0, Movement.y) * 0.35f;
            var rayCastEndPos2 = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance2;
            Debug.DrawLine(raycastPoint.position, rayCastEndPos2, Color.red);

            RaycastHit2D[] hits1 = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos1, 1 << LayerMask.NameToLayer("HeightObj"));
            RaycastHit2D[] hits2 = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos2, 1 << LayerMask.NameToLayer("HeightObj"));

            hits = hits1.Concat(hits2).ToArray();
        } else {
            Vector2 distance = new Vector2(Movement.x, Movement.y) * 0.35f;
            rayCastEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(raycastPoint.position, rayCastEndPos, Color.red);

            hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));
        }

        if(hits.Length >= 1) {
            Debug.Log("TriggerToJumpDown hits.Length > 1");
            foreach(RaycastHit2D hit in hits) {
                Debug.Log("TriggerToJumpDown hits collider name: "+hit.collider.name);
                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    if(jumpPointTrigger) {
                        Debug.Log("TriggerToJumpDown jumpPointTrigger");
                         
                        //if(isHoldInteraction && facingDir.Equals(new Vector2(movement.x, movement.y))) {  
                        Debug.Log("isHoldInteraction: "+isHoldInteraction);
                        Debug.Log("new Vector2(movement.x, movement.y): "+new Vector2(Movement.x, Movement.y));
                        if(isHoldInteraction && facingDir.Equals(new Vector2(Movement.x, Movement.y))) {
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
                            break;
                        }
                        else {
                            Debug.Log("TriggerToJumpDown KnockbackFeedback");
                            Debug.Log("TriggerToJumpDown isHoldInteraction "+isHoldInteraction);
                            Debug.Log("TriggerToJumpDown facingDir "+facingDir);
                            Debug.Log("TriggerToJumpDown new Vector2(movement.x, movement.y) "+new Vector2(Movement.x, Movement.y));
                            KnockbackFeedback feedback = GetComponent<KnockbackFeedback>();
                            feedback.ActiveFeedbackByDir(-new Vector2(Movement.x, Movement.y));
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

        Vector2 distance = new Vector2(Movement.x, Movement.y) * 0.35f;
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
                    Debug.Log("OnCollisioning: "+OnColliders.Contains(hit.collider));
                    if(angle >= 60f && 180f - angle >= 60f && OnColliders.Contains(hit.collider)) {
                        if(!jumpDelaying) {
                            Vector2 faceDirAtJump = facingDir;
                            jumpDelayRoutine = StartCoroutine(JumpUpDelay(faceDirAtJump));
                            break;
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
            Debug.Log("Buttom: "+m_Buttom);
            Debug.Log("Coordinate: "+m_Coordinate);
            Debug.Log("transform_pos: "+transform.position);

            Vector3 shadowCoordinate = new Vector3(m_Coordinate.x, m_Coordinate.y, groundCheckHeight);
            Debug.Log("shadowCoordinate: "+shadowCoordinate);
            Vector3 shadowWorldPos = new Vector3(shadowCoordinate.x, shadowCoordinate.y + shadowCoordinate.z);

            if(state != JumpState.JumpUp) {
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
                var jumpPointCollider = jumpPoint.GetComponent<BoxCollider2D>();
                var jumpPointColliderPoint1 = jumpPointCollider.bounds.min;
                var jumpPointColliderPoint2 = jumpPointCollider.bounds.max;
                var jumpPointColliderPoint3 = new Vector2(jumpPointColliderPoint1.x, jumpPointColliderPoint2.y);
                var jumpPointColliderPoint4 = new Vector2(jumpPointColliderPoint2.x, jumpPointColliderPoint1.y);

                Debug.Log("jumpPointCollider pos: "+jumpPointCollider.transform.position);
                Debug.Log("jumpPointColliderPoint1: "+jumpPointColliderPoint1);
                Debug.Log("jumpPointColliderPoint2: "+jumpPointColliderPoint2);
                Debug.Log("jumpPointColliderPoint3: "+jumpPointColliderPoint3);
                Debug.Log("jumpPointColliderPoint4: "+jumpPointColliderPoint4);

                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint1, currHeight, goalheight, Movement));
                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint2, currHeight, goalheight, Movement));
                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint3, currHeight, goalheight, Movement));
                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint4, currHeight, goalheight, Movement));

                if(Mathf.Floor(goalheight) >= 0) {
                    groundCheckHeight = Mathf.Floor(goalheight);
                } else {
                    groundCheckHeight = 0;
                }
                

                if(hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint1, currHeight, goalheight, Movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint2, currHeight, goalheight, Movement), groundCheckHeight) ||
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint3, currHeight, goalheight, Movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint4, currHeight, goalheight, Movement), groundCheckHeight)) {

                    Debug.Log("PredictNext Groundable true, groundCheckHeight: "+groundCheckHeight);
                    
                    //if(hm.NotGroundableChecked(PredictNextJumpPointWorldPos(m_Center, currHeight, goalheight, movement), groundCheckHeight)) {
                    if(hm.NotGroundableChecked(m_Center, groundCheckHeight) || hm.NotGroundableChecked(shadowWorldPos, groundCheckHeight)) {
                        // NotGroundable(ex: 岩壁)判定
                        Debug.Log("PredictNext NotGroundable true");
                        // bool hasCeiling = hm.CeilingChecked(m_Center, groundCheckHeight); 
                        // if(hasCeiling) {
                            lastHeight = currHeight;
                            currHeight = goalheight;
                            jumpOffset += g;
                        // }
                    } else {
                        Debug.Log("PredictNext NotGroundable false");
                        lastHeight = currHeight;
                        currHeight = groundCheckHeight;
                        FinishJump();
                        groundDelayRoutine = StartCoroutine(GroundDelay());
                    }
                } else {
                    Debug.Log("PredictNext Groundable false");
                    RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, rayCastEndPos, 1 << LayerMask.NameToLayer("HeightObj"));

                    if(Mathf.Ceil(currHeight) - currHeight <= 0.5f) {
                        groundCheckHeight = Mathf.Ceil(currHeight);
                        if(hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint1, currHeight, goalheight, Movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint2, currHeight, goalheight, Movement), groundCheckHeight) ||
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint3, currHeight, goalheight, Movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint4, currHeight, goalheight, Movement), groundCheckHeight)) {
                            Debug.Log("After Ceil Groundable true");
                            lastHeight = currHeight;
                            currHeight = groundCheckHeight;
                            FinishJump();
                            groundDelayRoutine = StartCoroutine(GroundDelay());
                        } else {
                            if(hits.Length >= 1) {
                                var variableVector = PredictNextJumpPointWorldPos(jumpPointColliderPoint1, currHeight, goalheight, Movement) - jumpPointColliderPoint1;

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
                            var variableVector = PredictNextJumpPointWorldPos(jumpPointColliderPoint1, currHeight, goalheight, Movement) - jumpPointColliderPoint1;

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

    private Vector3 PredictNextJumpPointWorldPos(Vector3 point, float lastH, float currH, Vector3 movement) {
        Debug.Log("PredictNextJumpPoint before add velocity lastH: "+lastH);
        Debug.Log("PredictNextJumpPoint before add velocity currH: "+currH);

        Vector3 coord = Vector3.zero;
        coord.x = point.x;
        coord.z = currH;
        coord.y = point.y - lastH;

        Vector3 result = Vector3.zero;
        result.x = coord.x;
        result.y = coord.y + coord.z;
        result.z = coord.z;

        Debug.Log("PredictNextJumpPoint before add velocity result: "+result);
        Debug.Log("PredictNextJumpPoint before add velocity: "+m_Rigidbody.velocity);
        Debug.Log("PredictNextJumpPoint before add Time.deltaTime: "+Time.deltaTime);

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
        SetMoveSpeed(11f);
        jumpingMovementVariable = 0.5f;
        OnColliders.Clear();

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
