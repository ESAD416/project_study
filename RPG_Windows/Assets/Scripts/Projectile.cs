using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Collider2D projectileHitBox;
    public float speed;
    public float duration;
    public Vector3 referenceAxis;
    protected Vector3 launchDirection;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        transform.position += launchDirection * speed * Time.deltaTime;
    }

    private void OnEnable() {
        Invoke("DestoryProjectile", duration);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //Destroy(gameObject);
        //Debug.Log("OnCollisionEnter2D Destory Projectile");
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            Debug.Log("OnTriggerEnter2D Destory Projectile");
            //Destroy(gameObject);
            DestoryProjectile();
        }
    }

    public void SetDirection(Vector3 direction) {
        //Debug.Log("SetDirection: "+direction);
        this.launchDirection = direction;
    }

    private void DestoryProjectile() {
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        CancelInvoke();
    }
}
