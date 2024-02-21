using System;
using UnityEngine;

public class HeightOfLevel : MonoBehaviour
{
    [SerializeField] private int selfHeight = 0;
    [SerializeField] private int correspondHeight = 0;

    void Start() {
        
    }

    public int GetCorrespondHeight() {
        return correspondHeight;
    }

    public int GetSelfHeight() {
        return selfHeight;
    }

    protected void OnCollisionExit2D(Collision2D otherCollider) {
        // if(otherCollider.gameObject.tag == "Player") {
        //     if(player.isJumping) {
        //         Debug.Log("Player hit HeightOfObject while jumping");
        //         player.jumpHitColli = true;
        //     }
        // }
    }
}
