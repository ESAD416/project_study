using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndirectProjectile : MonoBehaviour
{
    public Collider2D projectileHitBox;
    [SerializeField] float duration = 5;
    private float delayTime = 0.1f;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 destinationPos;

    
    private Vector3 bezierControlPoint;
    private Vector3[] path;
    [SerializeField] private float timeElapsed;


    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed >= duration)
        {
            timeElapsed = duration;
        } 

        var t = timeElapsed / duration;
        transform.position = GetQuadraticBezierPoint(t, startPos, bezierControlPoint, destinationPos);
        timeElapsed += Time.deltaTime;

    }

    private void OnCollisionEnter2D(Collision2D other) {
        //Destroy(gameObject);
        //Debug.Log("OnCollisionEnter2D Destory Projectile");
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if(other.gameObject.tag == "Player") {
        //     Debug.Log("OnTriggerEnter2D Destory Projectile");
        //     //Destroy(gameObject);
        //     DestoryProjectile();
        // }
    }

    public void SetPositionOfBezierCurve(Vector3 start, Vector3 destination, float height) {
        this.startPos = start;
        this.destinationPos = destination;
        this.bezierControlPoint = (startPos + destinationPos) * 0.5f+(Vector3.up * height);

        transform.position = startPos;
    }

    private Vector3 GetQuadraticBezierPoint(float t, Vector3 start, Vector3 controlPoint, Vector3 end) {
        /// <param name="t"> 0 <= t <= 1，0獲得曲線的起點，1獲得曲線的終點</param>
        /// <param name="start">曲線的起始位置</param>
        /// <param name="controlPoint">決定曲線形狀的控制點</param>
        /// <param name="end">曲線的終點位置</param>
        return Mathf.Pow(1 - t, 2) * start + 2 * t * (1 - t) * controlPoint + Mathf.Pow(t, 2) * end;
    }

    public void SetDuration(float duration) {
        this.duration = duration;
    }

    private void DestoryProjectile() {
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        timeElapsed = 0f;
        Invoke("DestoryProjectile", duration);
    }

    private void OnDisable() {
        CancelInvoke();
    }
}
