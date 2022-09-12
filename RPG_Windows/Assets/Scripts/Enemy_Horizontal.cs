using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Horizontal : Charactor
{
    public bool moveRight;

    protected override void Start() {
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
        } else {
            movement = Vector3.left;
        }
    }

    private void FixedUpdate() {
        Move();
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
