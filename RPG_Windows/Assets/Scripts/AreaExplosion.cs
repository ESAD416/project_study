using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExplosion : MonoBehaviour
{
    [SerializeField] private Attack byWhom;
    public void SetByWhom(Attack attacker) => this.byWhom = attacker;

    [SerializeField] private float duration = 5;
    public void SetDuration(float duration) => this.duration = duration;
    private float timeElapsed;

    private void OnEnable() {
        timeElapsed = 0f;
        Invoke("DestoryExplosion", duration);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed >= duration)
        {
            timeElapsed = duration;
        } 
        timeElapsed += Time.deltaTime;
    }

    public void SetPosition(Vector3 targetPos) {
        transform.position = targetPos;
    }

    private void OnDisable() {
        CancelInvoke();
    }
    private void DestoryExplosion() {
        gameObject.SetActive(false);
    }

}
