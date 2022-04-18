using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : LevelTrigger
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D otherCollider) {
        base.OnTriggerEnter2D(otherCollider);
    }

    protected override void OnTriggerExit2D(Collider2D otherCollider) {
        base.OnTriggerExit2D(otherCollider);
    }
}
