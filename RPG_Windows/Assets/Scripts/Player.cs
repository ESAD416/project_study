using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Charactor
{
    [Header("Input Settings")]
    public PlayerInput playerInput;

    [SerializeField] private Transform raycastPoint;

    public float altitude = 0;

    public bool inLevelTrigger = false;
    public string stair_start = "Untagged";
    public string stair_end = "Untagged";

    

    protected override void Update()
    { 
        // if(altitudeIncrease != 0 && isMoving) {
        //     altitude += altitudeIncrease;
        // }  
        DetectedColiderToJump();
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

    private void DetectedColiderToJump() {
        Debug.Log("facingDir: "+facingDir);
        Debug.Log("movement: "+movement);
        Vector2 casrEndPos;
        if(isMoving) {
            if(movement == new Vector3(0, 1f, 0)) {
                // Up
                Debug.Log("CastPoint Up");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Up");
            } else if(movement == new Vector3(0, -1f)) {
                // Down
                Debug.Log("CastPoint Down");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Down");
            } else if(movement == new Vector3(-1f, 0)) {
                // Left
                Debug.Log("CastPoint Left");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Left");
            } else if(movement == new Vector3(1f, 0)) {
                // Right
                Debug.Log("CastPoint Right");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Right");
            } else if(movement == new Vector3(0.7f, 0.7f)) {
                // UpRight
                Debug.Log("CastPoint UpRight");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_UpRight");
            } else if(movement == new Vector3(-0.7f, 0.7f)) {
                // UpLeft
                Debug.Log("CastPoint UpLeft");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_UpLeft");
            } else if(movement == new Vector3(0.7f, -0.7f)) {
                // DownRight
                Debug.Log("CastPoint DownRight");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_DownRight");
            } else if(movement == new Vector3(-0.7f, -0.7f)) {
                // DownLeft
                Debug.Log("CastPoint DownLeft");
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_DownLeft");
            }

            casrEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + new Vector2(movement.x, movement.y) * 2f;
        } else {
            if(facingDir == new Vector2(0, 1f)) {
                // Up
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Up");
            } else if(facingDir == new Vector2(0, -1f)) {
                // Down
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Down");
            } else if(facingDir == new Vector2(-1f, 0)) {
                // Left
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Left");
            } else if(facingDir == new Vector2(1f, 0)) {
                // Right
                raycastPoint = GetComponentInChildren<Transform>().Find("RaycastPoint_Right");
            }

            casrEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + facingDir * 2f;
        }
        
        Debug.Log("endPos: "+casrEndPos);
        Debug.DrawLine(raycastPoint.position, casrEndPos, Color.blue);
        RaycastHit2D hit = Physics2D.Linecast(raycastPoint.position, casrEndPos);
        if(hit.collider == null) {
            Debug.Log("Not hit"); 
        } else {
            Debug.Log("Hitted"); 
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
