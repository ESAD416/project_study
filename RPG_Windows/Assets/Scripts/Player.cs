using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Charactor
{
    [Header("Input Settings")]
    public PlayerInput playerInput;

    /// <summary>
    /// 角色中心
    /// </summary>
    protected Vector3 m_center;
    public float radius = 2f;
    [SerializeField] private Transform raycastPoint;

    public float altitude = 0;
    public bool inLevelTrigger = false;
    public string stair_start = "Untagged";
    public string stair_end = "Untagged";

    
    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    { 
        // if(altitudeIncrease != 0 && isMoving) {
        //     altitude += altitudeIncrease;
        // }  
        DetectedColiderToJump();
        base.Update();
    }

    private void OnDrawGizmos() {
        m_center = new Vector3(transform.position.x, transform.position.y - 0.25f);
        Gizmos.DrawWireSphere(m_center, radius);
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

    private void DetectedColiderToJump() {
        // Debug.Log("facingDir: "+facingDir);
        // Debug.Log("movement: "+movement);
        Vector2 casrEndPos;
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

            casrEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + new Vector2(movement.x, movement.y) * 2f;

            // Debug.Log("endPos: "+casrEndPos);
            Debug.DrawLine(raycastPoint.position, casrEndPos, Color.blue);
            Collider2D[] hit = Physics2D.OverlapCircleAll(m_center, radius, 1 << LayerMask.NameToLayer("Trigger"));

            if(hit.Length > 0) {
                foreach (var hitCollider in hit)
                {
                    Debug.Log("hitted: "+hitCollider.gameObject.name);
                    Level level = hitCollider.GetComponent(typeof(Level)) as Level;
                    if(level != null) {
                        Debug.Log("level altitude: "+level.altitude);
                        
                        if(level.altitude == 1f || level.altitude < 0) {

                        }

                    }
                }
            }
            // RaycastHit2D hit = Physics2D.Linecast(raycastPoint.position, casrEndPos, 1 << LayerMask.NameToLayer("Trigger"));
            // if(hit == null) {
            //     Debug.Log("Not hit"); 
            // } else {
            //     Debug.Log("Hitted: "+hit.name); 
            // }
        } 
    }

    private IEnumerator Attack() {
        //Debug.Log("attack start");
        isAttacking = true;
        m_Animator.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(attackClipTime);  // hardcasted casted time for debugged

        StopAttack();
    }

}
