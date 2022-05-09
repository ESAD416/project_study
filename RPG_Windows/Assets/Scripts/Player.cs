using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Charactor
{
    [Header("Input Settings")]
    public PlayerInput playerInput;

    /// <summary>
    /// 角色中心
    /// </summary>
    public Vector3 m_center;
    public Vector2 capsuleSize = new Vector2(1.5f, 2f);
    public float radius = 2f;
    [SerializeField] private Transform raycastPoint;

    public float height = 0;
    public bool onStairs = false;
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
        m_center = new Vector3(transform.position.x, transform.position.y - 0.5f);
        //Gizmos.DrawWireSphere(m_center, radius);
        DrawCapsule(m_center, capsuleSize);
    }

    private void DrawCapsule(Vector3 orgin, Vector2 size) {
        Vector3 up = transform.up * (size.y - size.x) / 2f;
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(orgin + up, Vector3.forward, transform.right, 180f, size.x / 2.3f, 2);
        UnityEditor.Handles.DrawWireArc(orgin - up, Vector3.forward, -transform.right, 180f, size.x / 2.3f, 2);
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
        Vector2 castEndPos;
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

            castEndPos = new Vector2(raycastPoint.position.x, raycastPoint.position.y) + new Vector2(movement.x, movement.y) * 0.5f;

            Debug.DrawLine(raycastPoint.position, castEndPos, Color.blue);
            RaycastHit2D[] hits = Physics2D.LinecastAll(raycastPoint.position, castEndPos, 1 << LayerMask.NameToLayer("Trigger"));
            if(hits.Length > 0) {
                float altitudeVariation = 0;
                bool jumpUp = false;
                bool jumpDown = false;

                if(hits.Length == 1) {
                    var first = hits.First();
                    var lev = first.collider.GetComponent(typeof(LevelTile)) as LevelTile;
                    if(lev != null) {
                        float levAltitude = lev.altitude;
                        altitudeVariation = Math.Abs(height - levAltitude) ;
                        if(height < levAltitude && altitudeVariation > 0 && altitudeVariation <= 1) {
                            // jumpUp
                            jumpUp = true;
                        } else if(height == levAltitude) {
                            // jumpDown
                            jumpDown = true;
                        }
                    }
                }
                else if(hits.Length > 1) {
                    foreach(RaycastHit2D hit in hits) {
                        var lev = hit.collider.GetComponent(typeof(LevelTile)) as LevelTile;
                        if(lev != null) {
                            float levAltitude = lev.altitude;
                            // jumpUp check only
                            if(height < levAltitude && altitudeVariation > 0 && altitudeVariation <= 1) {
                                jumpUp = true;
                            } else {
                                jumpUp = false;
                                break;
                            }
                        }
                    }
                }

                if(jumpUp) {
                    Debug.Log("jumpUp");
                } else if(jumpDown){
                    Debug.Log("jumpDown");
                }
            }
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
