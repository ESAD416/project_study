using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Horizontal : Charactor
{
    public int currHealth = 10;
    private SpriteRenderer m_SprtRenderer;
    public bool moveRight;

    protected override void Start() {
        m_SprtRenderer = GetComponentInChildren<SpriteRenderer>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Coordinate = Vector3.zero;
    }

    protected override void Update() {
        var center = transform.Find("AttrinkCenter").GetComponent<Transform>();
        m_Center = new Vector3(center.position.x, center.position.y);

        var buttom = transform.Find("AttrinkButtom").GetComponent<Transform>();
        m_Buttom = new Vector3(buttom.position.x, buttom.position.y);

        if(moveRight) {
            movement = Vector3.right;
            m_SprtRenderer.flipX = true;

        } else {
            movement = Vector3.left;
            m_SprtRenderer.flipX = false;
        }

        m_Animator.SetFloat("moveSpeed", moveSpeed);
    }

    private void FixedUpdate() {
        Move();
    }

    public void TakeDamage(int damage) {
        Debug.Log("TakeDamage: "+damage);
        currHealth -= damage;
        if(currHealth <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("Enemy Die");
    }

    void OnTriggerEnter2D(Collider2D trig)
	{
		if (trig.gameObject.tag == "Enemy_Turn"){

			if (moveRight){
				moveRight = false;
			}
			else{
				moveRight = true;
			}	
		}
	}
}
