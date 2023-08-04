using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Abstract : Charactor
{
    [SerializeField] private Vector3 defaultMovement = Vector3.left;
    public bool isPatroling = false;
    public bool isChasing = false;

    [SerializeField] private HealthBar healthBar;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        if(m_Animator != null) hitRecoveryTime = AnimeUtils.GetAnimateClipTime(m_Animator, "Hurt");
        isPatroling = true;
    }

    // Update is called once per frame
    protected override void Update() {
        //Debug.Log("moveSpeed: "+moveSpeed);
        var center = transform.Find(centerObjName).GetComponent<Transform>();
        m_Center = center?.position ?? Vector3.zero;

        var buttom = transform.Find(buttomObjName).GetComponent<Transform>();
        m_Buttom = buttom?.position ?? Vector3.zero;

        UpdateCoordinate();

        m_Animator?.SetFloat("moveSpeed", moveSpeed);
    }

    protected override void FixedUpdate() {
        if(isDead) {
            movement = Vector3.zero;
        }
        Move();
    }

    public void SetDefaultMovement() {
        movement = defaultMovement;
    }

    public virtual void OnAttack() {
        Debug.Log("Enemy_Abstract onAttack: ");
        if(isMoving) {
            facingDir = movement;
        }
        attackRoutine = StartCoroutine(Attack());
    }

    // public override void DmgCalculate(int damage)
    // {
    //     base.DmgCalculate(damage);
    //     healthBar.SetHealth(currHealth, maxHealth);
    // }
}
