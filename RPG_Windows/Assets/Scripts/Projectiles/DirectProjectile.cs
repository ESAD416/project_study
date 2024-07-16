using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectProjectile : MonoBehaviour
{
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

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        int hittableLayerMask = 1 << LayerMask.NameToLayer("Hittable");
        //if (other.CompareTag("Enemies") && (hittableLayerMask & 1 << other.gameObject.layer) > 0)
        if ((hittableLayerMask & 1 << other.gameObject.layer) > 0)
        {
            Debug.Log("OnTriggerEnter2D Destory Projectile Trigger name: "+other.name);
            //Destroy(gameObject);
            DestoryProjectile();
        }
        // if(other.gameObject.tag == "Player") {
        //     Debug.Log("OnTriggerEnter2D Destory Projectile");
        //     //Destroy(gameObject);
        //     DestoryProjectile();
        // }
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
