using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static JumpMechanismUtils;

public class Jump_Lamniat : MonoBehaviour
{
    [Header("Jump_Lamniat 基本物件")]
    [SerializeField] protected Avatar_Lamniat m_Lamniat;
    [SerializeField] protected Movement_Lamniat m_avatarMovement;
    [SerializeField] private ColliderTrigger jumpPoint;
    public JumpState jumpState;
    protected Rigidbody2D m_LamniatRdbd;
    protected SpriteRenderer m_LamniatSprtRenderer;
    protected Animator m_LamniatAnimator;
    protected AvatarInputActionsControls m_inputControls;
    
    
    [Header("Jump_Lamniat 基本參數")]

    public bool IsJumping;
    protected float minjumpOffSet = -0.3f;
    protected float jumpOffset = 0.3f;
    [SerializeField] protected float maxJumpHeight = 0f;
    protected float maxJumpHeightOffSet = 1.5f;
    protected float jumpIncrement = 0f;
    protected float g = -0.0565f; // -0.0565f;
    protected float jumpingMovementVariable = 0.5f;
    protected bool jumpHitColli;

    protected Vector3 takeOffCoord = Vector3.zero;
    protected Vector2 takeOffDir = Vector2.zero;
    
    protected Coroutine jumpDelayRoutine;
    protected float jumpDelay = 0.1f;
    protected bool jumpDelaying = false;


    protected Coroutine groundDelayRoutine;
    protected bool groundDelaying = false;
    protected float groundDelay = 0.2f;

    private bool prepareToJump = false;
    //private bool OnCollisioning = false;
    private List<Collider2D> OnColliders = new List<Collider2D>();
    private bool processingPushback = false;
    
    private bool jumpPointTrigger = false;
    public bool testVer4 = true;


    protected virtual void Awake() 
    {
        m_LamniatRdbd = m_Lamniat.Rigidbody;
        m_LamniatSprtRenderer = m_Lamniat.SprtRenderer;
        m_LamniatAnimator = m_Lamniat.Animator;
    }

    // Start is called before the first frame update
    private void Start()
    {
        jumpPoint.OnPlayerEnterTriggerEvent.AddListener(() => {
            Debug.Log("ColliderTrigger jumpPointTrigger = true");
            jumpPointTrigger = true;
        });

        // Debug.Log("m_LamniatSprtRenderer position: "+m_LamniatSprtRenderer.transform.position);
        // m_LamniatSprtRenderer.transform.position = new Vector3(0, 2f);
        // Debug.Log("m_LamniatSprtRenderer position: "+m_LamniatSprtRenderer.transform.position);
        // Debug.Log("m_Lamniat position: "+m_Lamniat.transform.position);

        // var lastPos = m_LamniatSprtRenderer.transform.position;
        // m_LamniatSprtRenderer.transform.position = new Vector3(0, 0);
        // m_Lamniat.transform.position = lastPos;
    }

    // Update is called once per frame
    private void Update()
    {
        maxJumpHeight =  m_Lamniat.CurrentHeight + maxJumpHeightOffSet;

        if(!groundDelaying) {
            if(!IsJumping) {
                DetectedToJump();
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
            else if(!jumpHitColli) {
                DetectedWhileJump();
            }
        }
    }

    private void FixedUpdate() {
        if(IsJumping) {
        //if(IsJumping && !m_Lamniat.CurrentBaseState.State.Equals(BaseStateMachine_Avatar.BaseState.Attack)) {

            if(testVer4) {
                #region Ver.4
                m_LamniatSprtRenderer.transform.position = m_Lamniat.GetWorldPosByCoordinate(m_Lamniat.Coordinate) - new Vector3(0, 2.0f);   // 預設中心點是(x, y+2)
                HandleJumpingProcess(jumpState);

                #endregion
            } else {
                #region Ver.3 
                m_Lamniat.transform.position = m_Lamniat.GetWorldPosByCoordinate(m_Lamniat.Coordinate) - new Vector3(0, 2.0f);   // 預設中心點是(x, y+2)
                HandleJumpingProcess(jumpState);

                #endregion
            }
        }
    }

    #region 碰撞偵測

    private void OnCollisionEnter2D(Collision2D other) {
        //Debug.Log("OnCollisionEnter2D: "+other.gameObject.name);
        OnColliders.Add(other.collider);
    }

    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("OnCollisionStay2D: "+other.gameObject.name);
    }

    private void OnCollisionExit2D(Collision2D other) {
        //Debug.Log("OnColliders count: "+OnColliders.Count);
        //Debug.Log("OnCollisionExit2D: "+other.gameObject.name);
        var itemToRemove = OnColliders.Single(r => r.name.Equals(other.collider.name));
        OnColliders.Remove(itemToRemove);
        //Debug.Log("OnColliders count: "+OnColliders.Count);
        //OnCollisioning = false;
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

    private JumpState DetectedWhetherNeedToJump() {
        if(m_avatarMovement.IsMoving) {
            m_Lamniat.SetRaycastPoint();

            Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * 0.5f;
            m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.blue);

            return JumpMechanismUtils.DetectedJumpState(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, distance, m_Lamniat.CurrentHeight, m_avatarMovement.IsMoving);
        }
        return JumpState.Ground;
    }

    private void DetectedToJump() {
        if(m_avatarMovement.IsMoving) {
            m_Lamniat.SetRaycastPoint();

            Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * 0.35f;
            m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.blue);

            // 偵測跳躍Edge ver.3
            RaycastHit2D[] hits = Physics2D.LinecastAll(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
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
                            altitudeVariation = Mathf.Abs(m_Lamniat.CurrentHeight - correspondHeight) ;
                            var angle = Vector2.Angle((Vector2)m_Lamniat.RaycastStart.position - m_Lamniat.RaycastEnd, hit.normal);
                            angle = 90.0f - Mathf.Abs(angle);
                            Debug.Log("DetectedToJump linecast angle:"+angle);

                            if(m_Lamniat.CurrentHeight < correspondHeight && altitudeVariation > 0 && altitudeVariation <= 1) {
                                // jumpUp
                                if(angle >= 60f && 180f - angle >= 60f) {
                                    jumpUp = true;
                                }
                            }  else if(m_Lamniat.CurrentHeight >= correspondHeight) {
                            //else if(currHeight >= correspondHeight  && altitudeVariation > 0 && altitudeVariation <= 1) {
                                // jumpDown
                                jumpDown = true;
                            }
                        }
                    }
                }

                if(jumpUp || jumpDown) {
                    if(!IsJumping) {
                        takeOffCoord = m_Lamniat.Coordinate;
                        takeOffDir = m_avatarMovement.FacingDir;
                        IsJumping = true;
                        Debug.Log("takeOffPos: "+takeOffCoord);
                    }
                }
            }
        } 
    }

    private void DetectedWhileJump() {
        if(m_avatarMovement.IsMoving) {
            m_Lamniat.SetRaycastPoint();

            Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * 0.35f;
            m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.blue);

            RaycastHit2D[] hits = Physics2D.LinecastAll(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
            if(hits.Length > 0) {
                var heightManager = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;
                if(hits.Length >= 1) {
                    foreach(RaycastHit2D hit in hits) {
                        Debug.Log("DetectedWhileJump hits collider name: "+hit.collider.name);
                        var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                        if(heightObj != null) {
                            float correspondHeight = heightObj.GetCorrespondHeight();
                            float selfHeight = heightObj.GetSelfHeight();

                            if(m_Lamniat.CurrentHeight < selfHeight && jumpOffset <= 0) {
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
        m_Lamniat.SetRaycastPoint("_Down");

        RaycastHit2D[] hits = null;
        if(m_Lamniat.IsObliqueRaycast()) {
            Vector2 distance1 = new Vector2(m_avatarMovement.Movement.x, 0) * 0.35f;
            var rayCastEndPos1 = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance1;
            Debug.DrawLine(m_Lamniat.RaycastStart.position, rayCastEndPos1, Color.red);
            Vector2 distance2 = new Vector2(0, m_avatarMovement.Movement.y) * 0.35f;
            var rayCastEndPos2 = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance2;
            Debug.DrawLine(m_Lamniat.RaycastStart.position, rayCastEndPos2, Color.red);

            RaycastHit2D[] hits1 = Physics2D.LinecastAll(m_Lamniat.RaycastStart.position, rayCastEndPos1, 1 << LayerMask.NameToLayer("HeightObj"));
            RaycastHit2D[] hits2 = Physics2D.LinecastAll(m_Lamniat.RaycastStart.position, rayCastEndPos2, 1 << LayerMask.NameToLayer("HeightObj"));

            hits = hits1.Concat(hits2).ToArray();
        } else {
            Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * 0.35f;
            m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
            //Debug.Log("castEndPos: "+rayCastEndPos);
            Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.red);

            hits = Physics2D.LinecastAll(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
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
                        Debug.Log("isHoldInteraction: "+m_avatarMovement.IsHoldInteraction);
                        Debug.Log("new Vector2(movement.x, movement.y): "+new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y));
                        if(m_avatarMovement.IsHoldInteraction && m_avatarMovement.FacingDir.Equals(new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y))) {
                            Debug.Log("TriggerToJumpDown prepareToJump = false");
                            prepareToJump = false;
        
                            if(!IsJumping) {
                                takeOffCoord = m_Lamniat.Coordinate;
                                takeOffDir = m_avatarMovement.FacingDir;
                                IsJumping = true;
                                RevertColliderFromJumpDown();
                                Debug.Log("takeOffPos: "+takeOffCoord);
                            }
                            jumpPointTrigger = false;
                            break;
                        }
                        else {
                            Debug.Log("TriggerToJumpDown KnockbackFeedback");
                            Debug.Log("TriggerToJumpDown isHoldInteraction "+m_avatarMovement.IsHoldInteraction );
                            Debug.Log("TriggerToJumpDown facingDir "+m_avatarMovement.FacingDir);
                            Debug.Log("TriggerToJumpDown new Vector2(movement.x, movement.y) "+new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y));
                            KnockbackFeedback feedback = GetComponent<KnockbackFeedback>();
                            feedback.ActiveFeedbackByDir(-new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y));
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
        m_Lamniat.SetRaycastPoint();

        Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * 0.35f;
        m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
        //Debug.Log("castEndPos: "+rayCastEndPos);
        Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.red);

        RaycastHit2D[] hits = Physics2D.LinecastAll(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));
        if(hits.Length >= 1) {
            Debug.Log("TriggerToJumpUp hits.Length > 1");
            foreach(RaycastHit2D hit in hits) {
                Debug.Log("TriggerToJumpUp hits collider name: "+hit.collider.name);
                var heightObj = hit.collider.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    var angle = Vector2.Angle((Vector2)m_Lamniat.RaycastStart.position - m_Lamniat.RaycastEnd, hit.normal);
                    angle = 90.0f - Mathf.Abs(angle);
                    Debug.Log("DetectedJumpState linecast angle:"+angle);
                    Debug.Log("OnCollisioning: "+OnColliders.Contains(hit.collider));
                    if(angle >= 60f && 180f - angle >= 60f && OnColliders.Contains(hit.collider)) {
                        if(!jumpDelaying) {
                            Vector2 faceDirAtJump = m_avatarMovement.FacingDir;
                            jumpDelayRoutine = StartCoroutine(JumpUpDelay(faceDirAtJump));
                            break;
                        }
                    }
                }
            }
        }
        Debug.Log("TriggerToJumpUp end");
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
        Debug.Log("PredictNextJumpPoint before add velocity: "+m_Lamniat.Rigidbody.velocity);
        Debug.Log("PredictNextJumpPoint before add Time.deltaTime: "+Time.deltaTime);

        result = result + (Vector3)(m_Lamniat.Rigidbody.velocity * Time.deltaTime);

        return result;
    }

    #region 跳躍控制

    private void HandleJumpingProcess(JumpState state) {
        Debug.Log("currHeight start: "+m_Lamniat.CurrentHeight);
        Debug.Log("lastHeight start: "+m_Lamniat.LastHeight);

        // Debug.Log("jumpOffset start: "+jumpOffset);
        // Debug.Log("jumpIncrement end: "+jumpIncrement);
        float goalheight = m_Lamniat.CurrentHeight + jumpOffset;
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
            m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
            
            m_Lamniat.SetCurrentHeight(goalheight);
            
            jumpOffset += (g / 2); 
        } 
        else {
            
            var hm = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager;

            float groundCheckHeight = Mathf.Floor(m_Lamniat.CurrentHeight);

            Debug.Log("jumpstate: "+state);
            Debug.Log("Center: "+m_Lamniat.Center);
            Debug.Log("Buttom: "+m_Lamniat.Buttom);
            Debug.Log("Coordinate: "+m_Lamniat.Coordinate);
            Debug.Log("transform_pos: "+transform.position);

            Vector3 shadowCoordinate = new Vector3(m_Lamniat.Coordinate.x, m_Lamniat.Coordinate.y, groundCheckHeight);
            Debug.Log("shadowCoordinate: "+shadowCoordinate);
            Vector3 shadowWorldPos = new Vector3(shadowCoordinate.x, shadowCoordinate.y + shadowCoordinate.z);

            if(state != JumpState.JumpUp) {
                if(hm.GroundableChecked(shadowWorldPos, groundCheckHeight)) {
                // if(hm.GroundableChecked(m_Coordinate)) {
                    Debug.Log("Groundable true");
                    if(goalheight <= groundCheckHeight) {
                        if(hm.NotGroundableChecked(m_Lamniat.Center, groundCheckHeight) || hm.NotGroundableChecked(shadowWorldPos, groundCheckHeight)) {
                            // NotGroundable(ex: 岩壁)判定
                            Debug.Log("NotGroundable true");
                            bool hasCeiling = hm.CeilingChecked(m_Lamniat.Center, groundCheckHeight); 
                            if(hasCeiling) {
                                m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                                m_Lamniat.SetCurrentHeight(goalheight);
                                // currHeight = groundCheckHeight - 0.01f;
                                jumpOffset += g;
                            }
                        } else {
                            Debug.Log("NotGroundable false");
                            m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                            m_Lamniat.SetCurrentHeight(groundCheckHeight);
                            FinishJump();
                            groundDelayRoutine = StartCoroutine(GroundDelay());
                        }                    
                    } else {
                        m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                        m_Lamniat.SetCurrentHeight(goalheight);
                        jumpOffset += g;
                    }
                } else {
                    Debug.Log("Groundable false");
                    m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                    m_Lamniat.SetCurrentHeight(goalheight);
                    jumpOffset += g; 
                }
            } 
            else {
                var jumpPointCollider = jumpPoint.GetComponent<BoxCollider2D>();
                var jumpPointColliderPoint1 = jumpPointCollider.bounds.min;
                var jumpPointColliderPoint2 = jumpPointCollider.bounds.max;
                var jumpPointColliderPoint3 = new Vector2(jumpPointColliderPoint1.x, jumpPointColliderPoint2.y);
                var jumpPointColliderPoint4 = new Vector2(jumpPointColliderPoint2.x, jumpPointColliderPoint1.y);

                // Debug.Log("jumpPointCollider pos: "+jumpPointCollider.transform.position);
                // Debug.Log("jumpPointColliderPoint1: "+jumpPointColliderPoint1);
                // Debug.Log("jumpPointColliderPoint2: "+jumpPointColliderPoint2);
                // Debug.Log("jumpPointColliderPoint3: "+jumpPointColliderPoint3);
                // Debug.Log("jumpPointColliderPoint4: "+jumpPointColliderPoint4);

                // Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement));
                // Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint2, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement));
                // Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint3, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement));
                // Debug.Log("PredictNext jumpPointColliderPoint1: "+PredictNextJumpPointWorldPos(jumpPointColliderPoint4, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement));

                if(Mathf.Floor(goalheight) >= 0) {
                    groundCheckHeight = Mathf.Floor(goalheight);
                } else {
                    groundCheckHeight = 0;
                }
                

                if(hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint2, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight) ||
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint3, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight) || 
                   hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint4, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight)) {

                    Debug.Log("PredictNext Groundable true, groundCheckHeight: "+groundCheckHeight);
                    
                    //if(hm.NotGroundableChecked(PredictNextJumpPointWorldPos(m_Center, currHeight, goalheight, movement), groundCheckHeight)) {
                    if(hm.NotGroundableChecked(m_Lamniat.Center, groundCheckHeight) || hm.NotGroundableChecked(shadowWorldPos, groundCheckHeight)) {
                        // NotGroundable(ex: 岩壁)判定
                        Debug.Log("PredictNext NotGroundable true");
                        // bool hasCeiling = hm.CeilingChecked(m_Center, groundCheckHeight); 
                        // if(hasCeiling) {
                            m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                            m_Lamniat.SetCurrentHeight(goalheight);
                            jumpOffset += g;
                        // }
                    } else {
                        Debug.Log("PredictNext NotGroundable false");
                        m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                        m_Lamniat.SetCurrentHeight(groundCheckHeight);
                        FinishJump();
                        groundDelayRoutine = StartCoroutine(GroundDelay());
                    }
                } else {
                    Debug.Log("PredictNext Groundable false");
                    RaycastHit2D[] hits = Physics2D.LinecastAll(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, 1 << LayerMask.NameToLayer("HeightObj"));

                    if(Mathf.Ceil(m_Lamniat.CurrentHeight) - m_Lamniat.CurrentHeight <= 0.5f) {
                        groundCheckHeight = Mathf.Ceil(m_Lamniat.CurrentHeight);
                        if(hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint2, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight) ||
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint3, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight) || 
                           hm.GroundableChecked(PredictNextJumpPointWorldPos(jumpPointColliderPoint4, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement), groundCheckHeight)) {
                            Debug.Log("After Ceil Groundable true");
                            m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                            m_Lamniat.SetCurrentHeight(groundCheckHeight);
                            FinishJump();
                            groundDelayRoutine = StartCoroutine(GroundDelay());
                        } else {
                            if(hits.Length >= 1) {
                                var variableVector = PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement) - jumpPointColliderPoint1;

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
                            m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                            m_Lamniat.SetCurrentHeight(goalheight);
                            jumpOffset += g;
                        }
                    } else {
                        if(hits.Length >= 1) {
                            var variableVector = PredictNextJumpPointWorldPos(jumpPointColliderPoint1, m_Lamniat.CurrentHeight, goalheight, m_avatarMovement.Movement) - jumpPointColliderPoint1;

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
                        m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
                        m_Lamniat.SetCurrentHeight(goalheight);
                        jumpOffset += g; 
                    }
                }
            }

            
        }

        if(jumpOffset <= minjumpOffSet) {
            jumpOffset = minjumpOffSet;
        }
        
        Debug.Log("currHeight end: "+m_Lamniat.CurrentHeight);
        Debug.Log("lastHeight end: "+m_Lamniat.LastHeight);
        Debug.Log("jumpOffset end: "+jumpOffset);
        Debug.Log("jumpIncrement end: "+jumpIncrement);
    }

    private void HandleJumpingProcessVer2(JumpState state) {
        Debug.Log("currHeight start: "+m_Lamniat.CurrentHeight);
        Debug.Log("lastHeight start: "+m_Lamniat.LastHeight);

        // Debug.Log("jumpOffset start: "+jumpOffset);
        // Debug.Log("jumpIncrement end: "+jumpIncrement);
        float goalheight = m_Lamniat.CurrentHeight + jumpOffset;
    }

    protected IEnumerator GroundDelay() {
        groundDelaying = true;
        yield return new WaitForSeconds(groundDelay);  // hardcasted casted time for debugged
        //FinishJump();
        groundDelaying = false;
        jumpState = JumpState.Ground;
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
        
        if(!IsJumping) {
            takeOffCoord = m_Lamniat.Coordinate;
            takeOffDir = m_avatarMovement.FacingDir;
            IsJumping = true;
            RevertColliderFromJumpDown();
            Debug.Log("takeOffPos: "+takeOffCoord);
        }
    }

    protected IEnumerator JumpUpDelay(Vector2 faceDir) {
        jumpDelaying = true;
        // if button is change before delay time
        if(m_avatarMovement.FacingDir != faceDir) {
            jumpDelaying = false;
            yield break;
        }

        yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
        jumpDelaying = false;
        prepareToJump = false;
        
        if(!IsJumping) {
            takeOffCoord = m_Lamniat.Coordinate;
            takeOffDir = m_avatarMovement.FacingDir;
            IsJumping = true;
            Debug.Log("takeOffPos: "+takeOffCoord);
        }
    }

    public void FinishJump() {
        if(groundDelayRoutine != null) {
            StopCoroutine(groundDelayRoutine);
        }

        Debug.Log("StopJump currHeight: "+m_Lamniat.CurrentHeight);
        Debug.Log("StopJump takeOffPos: "+takeOffCoord);

        IsJumping = false;
        jumpHitColli = false;
        groundDelaying = false;
        jumpOffset = 0.3f;
        jumpIncrement = 0f;
        m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
        m_avatarMovement.SetMoveSpeed(11f);
        jumpingMovementVariable = 0.5f;
        OnColliders.Clear();

        if(testVer4) {
            var lastPos = m_LamniatSprtRenderer.transform.position;
            Debug.Log("lastSprtPos: "+lastPos);
            m_LamniatSprtRenderer.transform.localPosition = new Vector3(0, 0);
            m_Lamniat.transform.position = new Vector3(lastPos.x, lastPos.y, m_Lamniat.CurrentHeight);
            Debug.Log("New Set SprtPos: "+m_LamniatSprtRenderer.transform.position);
            
        } else {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_Lamniat.CurrentHeight);
        }
        
        Debug.Log("After Set SprtPos: "+m_LamniatSprtRenderer.transform.position);
        takeOffCoord = Vector3.zero;
    }

    #endregion

    
}
