using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Abstract : Charactor
{
    /// <summary>
    /// 暫定，可能會搬至Charactor，已死亡
    /// </summary>
    protected bool isDead = false;
    /// <summary>
    /// 暫定，可能會搬至Charactor，正在受擊
    /// </summary>
    protected bool isTakingDmg = false;
    /// <summary>
    /// 暫定，可能會搬至Charactor，血量
    /// </summary>
    public int currHealth = 20;
    /// <summary>
    /// 暫定，可能會搬至Charactor，受擊動作為即時觸發，故宣告一協程進行處理獨立的動作
    /// </summary>
    protected Coroutine takeDmgRoutine;
    /// <summary>
    /// 暫定，可能會搬至Charactor，一次受擊動畫所需的時間
    /// </summary>
    protected float takeDmgClipTime;

    // Start is called before the first frame update
    protected override void Start() {
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Coordinate = Vector3.zero;
        SetAnimateTakeDmgClipTime();
    }

    // Update is called once per frame
    protected override void Update() {
        Debug.Log("moveSpeed: "+moveSpeed);
        var center = transform.Find("AttrinkCenter").GetComponent<Transform>();
        m_Center = new Vector3(center.position.x, center.position.y);

        var buttom = transform.Find("AttrinkButtom").GetComponent<Transform>();
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

    private IEnumerator TakeDmg() {
        moveSpeed = 0f;
        isTakingDmg = true;
        yield return new WaitForSeconds(takeDmgClipTime);  // hardcasted casted time for debugged
        FinishTakeDmg();
    }

    public void FinishTakeDmg() {
        if(takeDmgRoutine != null) {
            StopCoroutine(takeDmgRoutine);
        }

        isTakingDmg = false;
        moveSpeed = 5f;
        //Debug.Log("TakeDmg end");
    }

    public void TakeDmgProcess(int damage) {
        Debug.Log("TakeDamage: "+damage);
        currHealth -= damage;
        m_Animator.SetTrigger("hurt");

        takeDmgRoutine = StartCoroutine(TakeDmg());

        if(currHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("Enemy Die");
        isDead = true;
        m_Animator.SetBool("isDead", isDead); 
        GetComponent<Collider2D>().enabled = false;
    }

    private void SetAnimateTakeDmgClipTime() {
        RuntimeAnimatorController ac = m_Animator.runtimeAnimatorController;
        for(int i = 0; i < ac.animationClips.Length; i++) {
            if( ac.animationClips[i].name == "Hurt")        //If it has the same name as your clip
            {
                takeDmgClipTime = ac.animationClips[i].length;
                break;
            }
        }
    }
}
