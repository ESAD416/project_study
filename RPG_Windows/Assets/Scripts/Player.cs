using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Charactor
{
    [Header("Input Settings")]
    public PlayerInput playerInput;

    public float altitude = 0;

    public bool inLevelTrigger = false;
    public string stair_start = "Untagged";
    public string stair_end = "Untagged";

    

    protected override void Update()
    { 
        // if(altitudeIncrease != 0 && isMoving) {
        //     altitude += altitudeIncrease;
        // }  
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

    private IEnumerator Attack() {
        //Debug.Log("attack start");
        isAttacking = true;
        m_Animator.SetBool("attack", isAttacking);
        yield return new WaitForSeconds(attackClipTime);  // hardcasted casted time for debugged

        StopAttack();
    }

}
