using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Abstract : Charactor
{
    [SerializeField]  private Vector3 defaultMovement = Vector3.left;
    public bool isPatroling = false;
    public bool isChasing = false;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        hitRecoveryTime = AnimeUtils.GetAnimateClipTime(m_Animator, "Hurt");
        isPatroling = true;
    }

    // Update is called once per frame
    protected override void Update() {
        
        //Debug.Log("moveSpeed: "+moveSpeed);
        var center = transform.Find(centerObjName).GetComponent<Transform>();
        m_Center = new Vector3(center.position.x, center.position.y);

        var buttom = transform.Find(buttomObjName).GetComponent<Transform>();
        m_Buttom = new Vector3(buttom.position.x, buttom.position.y);

        UpdateCoordinate();

        m_Animator.SetFloat("moveSpeed", moveSpeed);
    }

    protected virtual void FixedUpdate() {
        if(isDead) {
            movement = Vector3.zero;
        }
        Move();
    }

    public void SetDefaultMovement() {
        movement = defaultMovement;
    }

    public void OnAttack() {
        Debug.Log("Enemy onAttack: ");
        if(isMoving) {
            facingDir = movement;
        }
        attackRoutine = StartCoroutine(Attack());
    }
}
