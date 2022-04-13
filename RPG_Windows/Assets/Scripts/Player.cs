using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Charactor
{
    private KeyCode lastKeyDown;
    private KeyCode lastKeyUp;

    [Header("Input Settings")]
    public PlayerInput playerInput;

    // private void OnGUI() {
    //     Event e = Event.current;
    //     if (e.isKey)
    //     {
    //         if(e.type == EventType.KeyUp) {
    //             //Debug.Log("Detected key code up: " + e.keyCode);
    //             lastKeyUp = e.keyCode;
    //         } 
    //         else if(e.type == EventType.KeyDown && e.keyCode != KeyCode.None){
    //             //Debug.Log("Detected key code down: " + e.keyCode);
    //             lastKeyDown = e.keyCode;
    //         }
            
    //     }
    // }

    protected override void Update()
    {
        //SetMovementByKeyInput();
        //SetFacingDirByLastKeyInput();
        base.Update();
    }

    // private void SetFacingDirByLastKeyInput() {
    //     if(!isMoving && lastKeyDown != lastKeyUp) {
    //         lastKeyDown = lastKeyUp;
    //     }

    //     if(lastKeyDown == KeyCode.W) {
    //         facingDir = Vector2.up;
    //     }
    //     else if(lastKeyDown == KeyCode.A) {
    //         facingDir = Vector2.left;
    //     }
    //     else if(lastKeyDown == KeyCode.S) {
    //         facingDir = Vector2.down;
    //     }
    //     else if(lastKeyDown == KeyCode.D) {
    //         facingDir = Vector2.right;
    //     }
    // }

    // private void SetMovementByKeyInput() {
    //     movement = Vector2.zero;

    //     if(Input.GetKey(KeyCode.W)) {
    //         movement += Vector2.up;
    //     } 
    //     if(Input.GetKey(KeyCode.A)) {
    //         movement += Vector2.left;
    //     } 
    //     if(Input.GetKey(KeyCode.S)) {
    //         movement += Vector2.down;
    //     } 
    //     if(Input.GetKey(KeyCode.D)) {
    //         movement += Vector2.right;
    //     }

    //     if(Input.GetKeyDown(KeyCode.Space)) {
    //         attackRoutine = StartCoroutine(Attack());
    //     }

    //     // Debug.Log("x: "+movement.x+", y: "+movement.y);
    // }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputVecter2 = value.ReadValue<Vector2>();
        bool noInputMovement = inputVecter2.x == 0 && inputVecter2.y == 0;
        if(noInputMovement && movement.x != 0 || movement.y != 0) {
            facingDir = movement;
        }

        movement = inputVecter2;
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
