using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Charactor
{
    [Header("Input Settings")]
    public PlayerInput playerInput;

    public float altitudeIncrease = 0;

    public bool inLevelTrigger = false;

    protected override void Update()
    {   
        // if(altitudeIncrease != 0) {
        //     Vector3 currentMov = movement;
        //     movement = new Vector3(currentMov.x, currentMov.y, altitudeIncrease);
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

        movement = new Vector3(inputVecter2.x, inputVecter2.y, 2);
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
