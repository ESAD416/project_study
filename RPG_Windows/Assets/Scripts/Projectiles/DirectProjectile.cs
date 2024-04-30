using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectProjectile : MonoBehaviour
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

    protected virtual void OnCollisionEnter2D(Collision2D other) {
        //Destroy(gameObject);
        //Debug.Log("OnCollisionEnter2D Destory Projectile");
        //gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
    }

    public virtual void SetDirection(Vector3 direction) {
        //Debug.Log("SetDirection: "+direction);
        this.launchDirection = direction;
    }

    protected virtual void DestoryProjectile() {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable() {
        Invoke("DestoryProjectile", duration);
    }

    protected virtual void OnDisable() {
        CancelInvoke();
    }
}
