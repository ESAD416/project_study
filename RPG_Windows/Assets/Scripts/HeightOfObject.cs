using System;
using UnityEngine;

public class HeightOfObject : MonoBehaviour
{
    [SerializeField] private float selfHeight = 0;
    [SerializeField] private float correspondHeight = 0;
    protected Player player;

    void Start() {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player;
    }

    public float GetCorrespondHeight() {
        return correspondHeight;
    }

    public float GetSelfHeight() {
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
