using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using static JumpMechanismUtils;

public class StaticJump_Lamniat : MonoBehaviour
{
    [Header("Jump_Lamniat 基本物件")]
    [SerializeField] protected Lamniat m_Lamniat;
    [SerializeField] protected Movement_Lamniat m_avatarMovement;
    [SerializeField] private HeightManager m_hManager;
    public JumpState jumpState;
    protected Rigidbody2D m_LamniatRdbd;
    protected SpriteRenderer m_LamniatSprtRenderer;
    protected Animator m_LamniatAnimator;

    [Header("Jump_Lamniat 基本參數")]
    [SerializeField] protected bool prepareToJump = false;
    public bool IsJumping;
    protected float altitudeIncrement = 0f;
    [SerializeField] protected float jumpDistance = 1.5f;
    [SerializeField] protected float maxJumpHeight = 0f;
    [SerializeField] protected float jumpDuration = 1f;
    [SerializeField] protected float jumpTimeElapsed = 0f;

    protected Vector3 takeOffPos;
    protected Vector3 takeOffCoord = Vector3.zero;
    protected Vector2 takeOffDir = Vector2.zero;

    protected Coroutine jumpDelayRoutine;
    protected float jumpDelay = 0.1f;
    protected bool jumpDelaying = false;

    protected Vector3 groundPos;
    protected Coroutine groundDelayRoutine;
    protected bool groundDelaying = false;
    protected float groundDelay = 0.2f;

    private bool OnHeightObjCollisionEnter = false;

    private void Awake() {
        m_LamniatRdbd = m_Lamniat.Rigidbody;
        m_LamniatSprtRenderer = m_Lamniat.SprtRenderer;
        m_LamniatAnimator = m_Lamniat.Animator;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // maxJumpHeight =  m_Lamniat.CurrentHeight + 1f;
        // if(!IsJumping) {
        //     //DetectedToJump();
        //     if(!prepareToJump) jumpState = DetectedWhetherNeedToJump();
            
        //     switch(jumpState) {
        //         case JumpState.Ground:
        //             break;
        //         case JumpState.JumpDown:
        //             TriggerToJumpDown();
        //             // if(!processingPushback) {
        //             //     TriggerToJumpDown();
        //             // } else {
        //             //     m_Rigidbody.AddForce((-new Vector2(movement.x, movement.y))* moveSpeed, ForceMode2D.Force);
        //             // }
        //             break;
        //         case JumpState.JumpUp:
        //             TriggerToJumpUp();
        //             break;
        //     }
        // } 
    }

    private void FixedUpdate() {
        // if(IsJumping) ApplyBezierCurvePath();
    }

    // #region 碰撞偵測

    // private void OnCollisionEnter2D(Collision2D other) {
    //     Debug.Log("OnCollisionEnter2D: "+other.gameObject.name);
    //     if(jumpState != JumpState.Ground) {
    //         var heightObj = other.collider.GetComponent<HeightOfLevel>() as HeightOfLevel;
    //         if(heightObj != null) OnHeightObjCollisionEnter = true;
    //     }
    // }

    // private void OnCollisionStay2D(Collision2D other) {
    //     //Debug.Log("OnCollisionStay2D: "+other.gameObject.name);
    // }

    // private void OnCollisionExit2D(Collision2D other) {
    //     Debug.Log("OnCollisionExit2D: "+other.gameObject.name);

    //     if(jumpState != JumpState.Ground) {
    //         var heightObj = other.collider.GetComponent<HeightOfLevel>() as HeightOfLevel;
    //         if(heightObj != null) OnHeightObjCollisionEnter = false;
            
    //         // if(jumpState == JumpState.JumpDown) {
    //         //     RevertColliderFromJumpDown();
    //         // }
    //     }

    // }

    // #endregion

    

    // private JumpState DetectedWhetherNeedToJump() {
    //     //Debug.Log("DetectedWhetherNeedToJump");
    //     if(m_avatarMovement.IsMoving) {
    //         m_Lamniat.SetRaycastPoint();

    //         Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * 0.5f;
    //         m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
    //         Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.blue);

    //         return JumpMechanismUtils.DetectedJumpState(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, distance, m_Lamniat.CurrentHeight, m_avatarMovement.IsMoving);
    //     }
    //     return JumpState.Ground;
    // }

    // private void TriggerToJumpUp() {
    //     Debug.Log("TriggerToJumpUp start");
    //     prepareToJump = true;
    //     int targetHeight = m_Lamniat.CurrentHeight + 1;
    //     m_Lamniat.SetRaycastPoint();

    //     Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * jumpDistance;
    //     m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
    //     //Debug.Log("castEndPos: "+rayCastEndPos);
    //     Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.red);

    //     bool groundable = false;
    //     Vector3 targetPos = default(Vector3);
    //     if(m_hManager.GroundableChecked(m_Lamniat.RaycastEnd, targetHeight)) {
    //         groundable = true;
    //         targetPos = m_Lamniat.RaycastEnd;
    //     }

    //     if(groundable && OnHeightObjCollisionEnter) {        
    //         if(!jumpDelaying) {
    //             Vector2 faceDirAtJump = m_avatarMovement.FacingDir;
    //             jumpDelayRoutine = StartCoroutine(JumpDelay(faceDirAtJump, targetHeight, targetPos));
    //         }
    //     }
    //     Debug.Log("TriggerToJumpUp end");
    // }

    // private void TriggerToJumpDown() {
    //     Debug.Log("TriggerToJumpUp start");

    //     prepareToJump = true;
    //     int targetHeight = m_Lamniat.CurrentHeight;
    //     m_Lamniat.SetRaycastPoint();

    //     Vector2 distance = new Vector2(m_avatarMovement.Movement.x, m_avatarMovement.Movement.y) * jumpDistance;
    //     m_Lamniat.RaycastEnd = new Vector2(m_Lamniat.RaycastStart.position.x, m_Lamniat.RaycastStart.position.y) + distance;
    //     //Debug.Log("castEndPos: "+rayCastEndPos);
    //     Debug.DrawLine(m_Lamniat.RaycastStart.position, m_Lamniat.RaycastEnd, Color.red);

    //     bool groundable = false;
    //     Vector3 targetPos = default(Vector3);
    //     var checkedHeight = targetHeight;
    //     while(!groundable && checkedHeight >= 0) {
    //         if(m_hManager.GroundableChecked(m_Lamniat.RaycastEnd, targetHeight)) {
    //             if(!m_hManager.NotGroundableChecked(m_Lamniat.RaycastEnd, targetHeight)) {
    //                 groundable = true;
    //                 targetPos = m_Lamniat.RaycastEnd;
    //             }
    //         }

    //         checkedHeight--;
    //     }
        
    //     if(groundable && OnHeightObjCollisionEnter) {        
    //         if(!jumpDelaying) {
    //             Vector2 faceDirAtJump = m_avatarMovement.FacingDir;
    //             jumpDelayRoutine = StartCoroutine(JumpDelay(faceDirAtJump, targetHeight, targetPos));
    //         }
    //     }


    //     Debug.Log("TriggerToJumpUp end");
    // }

    // protected IEnumerator JumpDelay(Vector2 faceDir, int targetHeight, Vector3 targetPos) {
    //     Debug.Log("jumpUpDelay");
    //     jumpDelaying = true;
    //     // if button is change before delay time
    //     if(m_avatarMovement.FacingDir != faceDir) {
    //         jumpDelaying = false;
    //         yield break;
    //     }

    //     yield return new WaitForSeconds(jumpDelay);  // hardcasted casted time for debugged
    //     jumpDelaying = false;
    //     prepareToJump = false;
        
    //     if(!IsJumping) {
    //         takeOffPos = m_Lamniat.Buttom;
    //         takeOffCoord = m_Lamniat.Coordinate;
    //         takeOffDir = m_avatarMovement.FacingDir;
    //         groundPos = targetPos;
    //         m_Lamniat.SetCurrentHeight(targetHeight);
    //         //m_Lamniat.transform.position = new Vector3(m_Lamniat.transform.position.x, m_Lamniat.transform.position.y, m_Lamniat.CurrentHeight);
    //         IsJumping = true;
    //         Debug.Log("takeOffPos: "+takeOffCoord);
    //     }
    // }



    // private void ApplyBezierCurvePath() {
    //     if (jumpTimeElapsed >= jumpDuration)
    //     {
    //         jumpTimeElapsed = jumpDuration;
    //         FinishJump();
    //         return;
    //     } 

    //     float radio1 = 0.3f;
    //     float radio2 = 0.4f;
    //     float radio3 = 0.3f;

    //     var t = jumpTimeElapsed / jumpDuration;
    //     var adjustT = 0f;
    //     if(t < radio1) {
    //         adjustT = t / radio1 * 0.5f;
    //     } else if(t < radio1 + radio2) {
    //         adjustT = 0.5f + (t - radio1) / radio2 * 0.5f;
    //     } else {
    //         adjustT = 0.8f + (t - radio1 - radio2) / radio3 * 0.2f;
    //     }

    //     Vector3 controlPoint = (takeOffPos + groundPos) * 0.5f+(Vector3.up * 1.5f);
    //     Debug.Log("startPos: "+takeOffPos);
    //     Debug.Log("controlPoint: "+controlPoint);
    //     Debug.Log("endPos: "+groundPos);

    //     var pathPos = GetQuadraticBezierPoint(t, takeOffPos, controlPoint, groundPos);

    //     m_LamniatSprtRenderer.transform.position = new Vector3(pathPos.x, pathPos.y, m_Lamniat.CurrentHeight);
    //     Debug.Log("SprtPos: "+groundPos);
    //     jumpTimeElapsed += Time.deltaTime;
    // }

    // private Vector3 GetQuadraticBezierPoint(float t, Vector3 start, Vector3 controlPoint, Vector3 end) {
    //     /// <param name="t"> 0 <= t <= 1，0獲得曲線的起點，1獲得曲線的終點</param>
    //     /// <param name="start">曲線的起始位置</param>
    //     /// <param name="controlPoint">決定曲線形狀的控制點</param>
    //     /// <param name="end">曲線的終點位置</param>
    //     Vector3 result = Mathf.Pow(1 - t, 2) * start + 2 * t * (1 - t) * controlPoint + Mathf.Pow(t, 2) * end;
    //     Debug.Log("GetQuadraticBezierPoint: "+result);
    //     return Mathf.Pow(1 - t, 2) * start + 2 * t * (1 - t) * controlPoint + Mathf.Pow(t, 2) * end;
    // }

    // public void FinishJump() {
    //     if(groundDelayRoutine != null) {
    //         StopCoroutine(groundDelayRoutine);
    //     }

    //     Debug.Log("StopJump currHeight: "+m_Lamniat.CurrentHeight);
    //     Debug.Log("StopJump takeOffCoord: "+takeOffCoord);


    //     Debug.Log("lastSprtPos: "+groundPos);
    //     var lastPos = groundPos;
    //     m_LamniatSprtRenderer.transform.position = new Vector3(lastPos.x, lastPos.y, m_Lamniat.CurrentHeight);
    //     m_LamniatSprtRenderer.transform.localPosition = new Vector3(0, 0);
    //     m_Lamniat.transform.position = new Vector3(lastPos.x, lastPos.y, m_Lamniat.CurrentHeight);
    //     Debug.Log("New Set SprtPos: "+m_LamniatSprtRenderer.transform.position);
            
    //     takeOffPos = Vector3.zero;
    //     takeOffCoord = Vector3.zero;
    //     groundPos = Vector3.zero;
    //     jumpTimeElapsed = 0f;

    //     IsJumping = false;
    //     groundDelaying = false;
    //     //m_Lamniat.SetLastHeight(m_Lamniat.CurrentHeight);
    //     m_avatarMovement.SetMoveSpeed(11f);
    //     jumpState = JumpState.Ground;
    // }

}
