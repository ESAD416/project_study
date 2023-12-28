using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using static JumpMechanismUtils;

public class Avatar : Charactor
{
    #region 可操作角色物件

    [Header("Avatar基本物件")]
    [SerializeField] protected Movement_Avatar m_avatarMovement;
    /// <summary>
    /// 可操作角色的移動控制
    /// </summary>
    public Movement_Avatar AvatarMovement =>this.m_avatarMovement;

    [SerializeField] protected Jump_Lamniat m_avatarJump;
    /// <summary>
    /// 可操作角色的跳躍機制
    /// </summary>
    public Jump_Lamniat AvatarJump => this.m_avatarJump;

    protected AvatarInputActionsControls m_inputControls;
    /// <summary>
    /// 可操作角色的使用者輸入
    /// </summary>
    public AvatarInputActionsControls InputCtrl => this.m_inputControls;
    protected bool isHoldInteraction = false;

    #region Raycast物件

    [SerializeField] private List<Transform> m_raycastStartPosition = new List<Transform>();
    public List<Transform> RaycastStartPosition => this.m_raycastStartPosition;
    [SerializeField] private Transform m_raycastStart;
    public Transform RaycastStart => this.m_raycastStart;
    public Vector2 RaycastEnd;

    #endregion


    public string onStairs;
    public string stair_start;
    public string stair_end ;

    #endregion

    #region 可操作角色狀態

    private BaseStateMachine_Avatar m_currentBaseState;
    public BaseStateMachine_Avatar CurrentBaseState => this.m_currentBaseState;
    public void SetCurrentBaseState(BaseStateMachine_Avatar state) {
        this.m_currentBaseState.OnExit();
        this.m_currentBaseState = state;
        this.m_currentBaseState.OnEnter(this);
    }

    protected BaseStateMachine_Avatar m_idle;
    public BaseStateMachine_Avatar Idle => m_idle;
    protected BaseStateMachine_Avatar m_move;
    public BaseStateMachine_Avatar Move => m_move;
    protected BaseStateMachine_Avatar m_dodge;
    public BaseStateMachine_Avatar Dodge => m_dodge;
    protected BaseStateMachine_Avatar m_attack;
    public BaseStateMachine_Avatar Attack => m_attack;
    protected BaseStateMachine_Avatar m_hurt;
    public BaseStateMachine_Avatar Hurt => m_hurt;
    protected BaseStateMachine_Avatar m_dead;
    public BaseStateMachine_Avatar Dead => m_dead;

    #endregion

    protected override void Awake() {
        base.Awake();

        m_inputControls = new AvatarInputActionsControls();
    }

    protected override void OnEnable() {
        base.OnEnable();
        
        m_currHeight = m_InfoStorage.initialHeight;
        RaycastEnd = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + new Vector2(0, -1) * 0.35f;   // 預設射線終點
        transform.position = new Vector3(m_InfoStorage.initialPos.x, m_InfoStorage.initialPos.y, m_InfoStorage.initialHeight);

        m_currentBaseState = m_idle;
        m_currentBaseState.OnEnter();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    {
        m_currentBaseState.OnUpdate();

        m_Center = m_CenterObj?.position ?? Vector3.zero;
        m_Buttom = m_ButtomObj?.position ?? Vector3.zero;

        UpdateCoordinate();
        if(m_InfoStorage != null) {
            if(!string.IsNullOrEmpty(m_InfoStorage.jumpCollidersName)) {
                if(m_avatarJump.IsJumping) {
                    FocusCollidersWithHeightWhileJumping();
                } else {
                    FocusCollidersWithHeight();
                }
            }
        }

        // Debug.Log("m_currentBaseState: "+m_currentBaseState.State);
    }

    protected override void FixedUpdate() {
        m_currentBaseState.OnFixedUpdate();
        // if(isJumping && !CharStatus.Equals(CharactorStatus.Attack)) {
        //     transform.position = GetWorldPosByCoordinate(m_Coordinate) - new Vector3(0, 1.7f);   // 預設中心點是(x, y+1.7)
        //     HandleJumpingProcess(jumpState);
        // }

        base.FixedUpdate();
    }

    protected virtual void OnDisable() {
        m_currentBaseState.OnExit();
    }

    #region 碰撞偵測

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
    
    public void SetRaycastPoint(string raycastPointName = null) {
        if(raycastPointName != null) {
            //m_raycastStart = GetComponentInChildren<Transform>().Find(raycastPointName);
            m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals(raycastPointName));
        } else {
            if(AvatarMovement.Movement.x == 0 && AvatarMovement.Movement.y > 0) {
                // Up
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Up");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Up"));
            } else if(AvatarMovement.Movement.x == 0 && AvatarMovement.Movement.y < 0) {
                // Down 
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Down");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Down"));
            } else if(AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y == 0) {
                // Left
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Left");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Left"));
            } else if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y == 0) {
                // Right
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_Right");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_Right"));
            } else if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y > 0) {
                // UpRight
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_UpRight");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_UpRight"));
            } else if(AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y > 0) {
                // UpLeft
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_UpLeft");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_UpLeft"));
            } else if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y < 0) {
                // DownRight
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_DownRight");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_DownRight"));
            } else if(AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y < 0) {
                // DownLeft
                //m_raycastStart = GetComponentInChildren<Transform>().Find("RaycastPoint_DownLeft");
                m_raycastStart = m_raycastStartPosition.Single(r => r.name.Equals("_DownLeft"));
            }
        }
    }

    public bool IsObliqueRaycast() {
        if(AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y > 0 || AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y > 0 ||
           AvatarMovement.Movement.x > 0 && AvatarMovement.Movement.y < 0 || AvatarMovement.Movement.x < 0 && AvatarMovement.Movement.y < 0) {
            return true;
        }

        return false;
    }

    protected void FocusCollidersWithHeightWhileJumping() {
        Collider2D[] jumpColls = GridUtils.GetColliders(m_InfoStorage.jumpCollidersName);

        if(jumpColls != null) {
            foreach(var collider2D in jumpColls) {
                var heightObj = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    float groundCheckHeight = Mathf.Floor(m_currHeight);
                    float maxGroundCheckHeight = groundCheckHeight + 1;
                    // Debug.Log("FocusCollidersWithHeight collider2D name: "+collider2D.name);
                    // Debug.Log("FocusCollidersWithHeight collider2D type: "+collider2D.GetType());
                    if(groundCheckHeight + 1 == heightObj.GetSelfHeight()) {
                        collider2D.enabled = true;
                    } else {
                        collider2D.enabled = false;
                    }
                    // Debug.Log("FocusCollidersWithHeight collider2D enabled: "+collider2D.enabled);
                }
            }
        }
    }

    /*

    private JumpState DetectedWhetherNeedToJump() {
        if(AvatarMovement.IsMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y) * 0.5f;
            RaycastEnd = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_raycastStart.position, RaycastEnd, Color.blue);

            return JumpMechanismUtils.DetectedJumpState(m_raycastStart.position, RaycastEnd, distance, m_currHeight, AvatarMovement.IsMoving);
        }
        return JumpState.Ground;
    }

    private void DetectedToJump() {
        if(AvatarMovement.IsMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y) * 0.35f;
            RaycastEnd = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_raycastStart.position, RaycastEnd, Color.blue);

            // 偵測跳躍Edge ver.3
            RaycastHit2D[] hits = Physics2D.LinecastAll(m_raycastStart.position, RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
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
                            altitudeVariation = Mathf.Abs(m_currHeight - correspondHeight) ;
                            var angle = Vector2.Angle((Vector2)m_raycastStart.position - RaycastEnd, hit.normal);
                            angle = 90.0f - Mathf.Abs(angle);
                            Debug.Log("DetectedToJump linecast angle:"+angle);

                            if(m_currHeight < correspondHeight && altitudeVariation > 0 && altitudeVariation <= 1) {
                                // jumpUp
                                if(angle >= 60f && 180f - angle >= 60f) {
                                    jumpUp = true;
                                }
                            }  else if(m_currHeight >= correspondHeight) {
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
                        takeOffDir = AvatarMovement.FacingDir;
                        isJumping = true;
                        Debug.Log("takeOffPos: "+takeOffCoord);
                    }
                }
            }
        } 
    }

    private void DetectedWhileJump() {
        if(AvatarMovement.IsMoving) {
            SetRaycastPoint();

            Vector2 distance = new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y) * 0.35f;
            RaycastEnd = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_raycastStart.position, RaycastEnd, Color.blue);

            RaycastHit2D[] hits = Physics2D.LinecastAll(m_raycastStart.position, RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
            if(hits.Length > 0) {
                var heightManager = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;
                if(hits.Length >= 1) {
                    foreach(RaycastHit2D hit in hits) {
                        Debug.Log("DetectedWhileJump hits collider name: "+hit.collider.name);
                        var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                        if(heightObj != null) {
                            float correspondHeight = heightObj.GetCorrespondHeight();
                            float selfHeight = heightObj.GetSelfHeight();

                            if(m_currHeight < selfHeight && jumpOffset <= 0) {
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
        SetRaycastPoint("_Down");

        RaycastHit2D[] hits = null;
        if(IsObliqueRaycast()) {
            Vector2 distance1 = new Vector2(AvatarMovement.Movement.x, 0) * 0.35f;
            var rayCastEndPos1 = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + distance1;
            Debug.DrawLine(m_raycastStart.position, rayCastEndPos1, Color.red);
            Vector2 distance2 = new Vector2(0, AvatarMovement.Movement.y) * 0.35f;
            var rayCastEndPos2 = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + distance2;
            Debug.DrawLine(m_raycastStart.position, rayCastEndPos2, Color.red);

            RaycastHit2D[] hits1 = Physics2D.LinecastAll(m_raycastStart.position, rayCastEndPos1, 1 << LayerMask.NameToLayer("HeightObj"));
            RaycastHit2D[] hits2 = Physics2D.LinecastAll(m_raycastStart.position, rayCastEndPos2, 1 << LayerMask.NameToLayer("HeightObj"));

            hits = hits1.Concat(hits2).ToArray();
        } else {
            Vector2 distance = new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y) * 0.35f;
            RaycastEnd = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_raycastStart.position, RaycastEnd, Color.red);

            hits = Physics2D.LinecastAll(m_raycastStart.position, RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
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
                        Debug.Log("new Vector2(movement.x, movement.y): "+new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y));
                        if(isHoldInteraction && AvatarMovement.FacingDir.Equals(new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y))) {
                            Debug.Log("TriggerToJumpDown prepareToJump = false");
                            prepareToJump = false;
        
                            if(!isJumping) {
                                takeOffCoord = m_Coordinate;
                                takeOffDir = AvatarMovement.FacingDir;
                                isJumping = true;
                                maxJumpHeight = m_currHeight + 1.5f;
                                RevertColliderFromJumpDown();
                                Debug.Log("takeOffPos: "+takeOffCoord);
                            }
                            jumpPointTrigger = false;
                            break;
                        }
                        else {
                            Debug.Log("TriggerToJumpDown KnockbackFeedback");
                            Debug.Log("TriggerToJumpDown isHoldInteraction "+isHoldInteraction);
                            Debug.Log("TriggerToJumpDown facingDir "+AvatarMovement.FacingDir);
                            Debug.Log("TriggerToJumpDown new Vector2(movement.x, movement.y) "+new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y));
                            KnockbackFeedback feedback = GetComponent<KnockbackFeedback>();
                            feedback.ActiveFeedbackByDir(-new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y));
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

        Vector2 distance = new Vector2(AvatarMovement.Movement.x, AvatarMovement.Movement.y) * 0.35f;
        RaycastEnd = new Vector2(m_raycastStart.position.x, m_raycastStart.position.y) + distance;
        //Debug.Log("castEndPos: "+rayCastEndPos);
        Debug.DrawLine(m_raycastStart.position, RaycastEnd, Color.red);

        RaycastHit2D[] hits = Physics2D.LinecastAll(m_raycastStart.position, RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
        if(hits.Length >= 1) {
            Debug.Log("TriggerToJumpUp hits.Length > 1");
            foreach(RaycastHit2D hit in hits) {
                Debug.Log("TriggerToJumpUp hits collider name: "+hit.collider.name);
                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    var angle = Vector2.Angle((Vector2)m_raycastStart.position - RaycastEnd, hit.normal);
                    angle = 90.0f - Mathf.Abs(angle);
                    Debug.Log("DetectedJumpState linecast angle:"+angle);
                    Debug.Log("OnCollisioning: "+OnColliders.Contains(hit.collider));
                    if(angle >= 60f && 180f - angle >= 60f && OnColliders.Contains(hit.collider)) {
                        if(!jumpDelaying) {
                            Vector2 faceDirAtJump = AvatarMovement.FacingDir;
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
        Debug.Log("currHeight start: "+m_currHeight);
        Debug.Log("lastHeight start: "+lastHeight);

        // Debug.Log("jumpOffset start: "+jumpOffset);
        // Debug.Log("jumpIncrement end: "+jumpIncrement);
        float goalheight = m_currHeight + jumpOffset;
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
            lastHeight = m_currHeight;
            
            m_currHeight = goalheight;
            
            jumpOffset += (g / 2); 
        } 
        else {
            
            var hm = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;

            float groundCheckHeight = Mathf.Floor(m_currHeight);

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
                                lastHeight = m_currHeight;
                                m_currHeight = goalheight;
                                // currHeight = groundCheckHeight - 0.01f;
                                jumpOffset += g;
                            }
                        } else {
                            Debug.Log("NotGroundable false");
                            lastHeight = m_currHeight;
                            m_currHeight = groundCheckHeight;
                            FinishJump();
                            groundDelayRoutine = StartCoroutine(GroundDelay());
                        }                    
                    } else {
                        lastHeight = m_currHeight;
                        m_currHeight = goalheight;
                        jumpOffset += g;
                    }
                } else {
                    Debug.Log("Groundable false");
                    lastHeight = m_currHeight;
                    m_currHeight = goalheight;
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

                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_currHeight, goalheight, AvatarMovement.Movement));
                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint2, m_currHeight, goalheight, AvatarMovement.Movement));
                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint3, m_currHeight, goalheight, AvatarMovement.Movement));
                Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint4, m_currHeight, goalheight, AvatarMovement.Movement));

                if(Mathf.Floor(goalheight) >= 0) {
                    groundCheckHeight = Mathf.Floor(goalheight);
                } else {
                    groundCheckHeight = 0;
                }
                

                if(hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint2, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight) ||
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint3, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint4, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight)) {

                    Debug.Log("PredictNext Groundable true, groundCheckHeight: "+groundCheckHeight);
                    
                    //if(hm.NotGroundableChecked(PredictNextJumpPointWorldPos(m_Center, currHeight, goalheight, movement), groundCheckHeight)) {
                    if(hm.NotGroundableChecked(m_Center, groundCheckHeight) || hm.NotGroundableChecked(shadowWorldPos, groundCheckHeight)) {
                        // NotGroundable(ex: 岩壁)判定
                        Debug.Log("PredictNext NotGroundable true");
                        // bool hasCeiling = hm.CeilingChecked(m_Center, groundCheckHeight); 
                        // if(hasCeiling) {
                            lastHeight = m_currHeight;
                            m_currHeight = goalheight;
                            jumpOffset += g;
                        // }
                    } else {
                        Debug.Log("PredictNext NotGroundable false");
                        lastHeight = m_currHeight;
                        m_currHeight = groundCheckHeight;
                        FinishJump();
                        groundDelayRoutine = StartCoroutine(GroundDelay());
                    }
                } else {
                    Debug.Log("PredictNext Groundable false");
                    RaycastHit2D[] hits = Physics2D.LinecastAll(m_raycastStart.position, RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));

                    if(Mathf.Ceil(m_currHeight) - m_currHeight <= 0.5f) {
                        groundCheckHeight = Mathf.Ceil(m_currHeight);
                        if(hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint2, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight) ||
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint3, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint4, m_currHeight, goalheight, AvatarMovement.Movement), groundCheckHeight)) {
                            Debug.Log("After Ceil Groundable true");
                            lastHeight = m_currHeight;
                            m_currHeight = groundCheckHeight;
                            FinishJump();
                            groundDelayRoutine = StartCoroutine(GroundDelay());
                        } else {
                            if(hits.Length >= 1) {
                                var variableVector = PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_currHeight, goalheight, AvatarMovement.Movement) - jumpPointColliderPoint1;

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
                            lastHeight = m_currHeight;
                            m_currHeight = goalheight;
                            jumpOffset += g;
                        }
                    } else {
                        if(hits.Length >= 1) {
                            var variableVector = PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_currHeight, goalheight, AvatarMovement.Movement) - jumpPointColliderPoint1;

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
                        lastHeight = m_currHeight;
                        m_currHeight = goalheight;
                        jumpOffset += g; 
                    }
                }
            }

            
        }

        if(jumpOffset <= minjumpOffSet) {
            jumpOffset = minjumpOffSet;
        }
        
        Debug.Log("currHeight end: "+m_currHeight);
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
        Debug.Log("PredictNextJumpPoint before add velocity: "+Rigidbody.velocity);
        Debug.Log("PredictNextJumpPoint before add Time.deltaTime: "+Time.deltaTime);

        result = result + (Vector3)(Rigidbody.velocity * Time.deltaTime);

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
            takeOffDir = AvatarMovement.FacingDir;
            isJumping = true;
            maxJumpHeight = m_currHeight + 1.5f;
            RevertColliderFromJumpDown();
            Debug.Log("takeOffPos: "+takeOffCoord);
        }
    }

    protected IEnumerator JumpUpDelay(Vector2 faceDir) {
        jumpDelaying = true;
        // if button is change before delay time
        if(AvatarMovement.FacingDir != faceDir) {
            jumpDelaying = false;
            yield break;
        }

        yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
        jumpDelaying = false;
        prepareToJump = false;
        
        if(!isJumping) {
            takeOffCoord = m_Coordinate;
            takeOffDir = AvatarMovement.FacingDir;
            isJumping = true;
            maxJumpHeight = m_currHeight + 1.5f;
            Debug.Log("takeOffPos: "+takeOffCoord);
        }
    }

    public void FinishJump() {
        if(groundDelayRoutine != null) {
            StopCoroutine(groundDelayRoutine);
        }

        Debug.Log("StopJump currHeight: "+m_currHeight);
        Debug.Log("StopJump takeOffPos: "+takeOffCoord);

        isJumping = false;
        jumpHitColli = false;
        groundDelaying = false;
        jumpOffset = 0.3f;
        lastHeight = m_currHeight;
        AvatarMovement.SetMoveSpeed(11f);
        jumpingMovementVariable = 0.5f;
        OnColliders.Clear();

        transform.position = new Vector3(transform.position.x, transform.position.y, m_currHeight);
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
            m_currHeight = currStair.SetPlayerHeightOnStair();
            Debug.Log("SetPlayerHeightOnStair player height: "+m_currHeight);

            transform.position = new Vector3(transform.position.x, transform.position.y, m_currHeight);
        }

    }

    */
}
