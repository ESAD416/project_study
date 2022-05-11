using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    public float altitudeVariation = 1f;

    protected Player player;

    Vector2 startPos;
    Vector2 endPos;

    Collider2D[] collider2Ds;

    protected void Start() {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player;
        var grid = GameObject.FindObjectOfType(typeof(Grid)) as Grid;
        collider2Ds = grid.GetComponentsInChildren<Collider2D>();
    }    

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
            if(!player.onStairs) {
                Debug.Log("player not in StairsTrigger");
                startPos = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y);
                Debug.Log("startPos: "+startPos);
            }

            FocusOnStairsColliders();
            //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blocks"));
        }
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {
        if(otherCollider.gameObject.tag == "Player") {
            if(!player.onStairs) {
                Debug.Log("player enter in StairsTrigger");
                player.onStairs = true;
            } else {
                Debug.Log("player prepare to leave StairsTrigger");
                endPos = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y);
                Debug.Log("endPos: "+endPos);

                //計算player高度 ver 2
                var heightManager = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager; 
                List<float> heightsOfTile = heightManager.GetHeightFromTile(player.m_center);
                if(heightsOfTile.Count > 0) {
                    if(heightsOfTile.Count == 1) {
                        float first = heightsOfTile.First();
                        player.height = first;
                    }
                    else {
                        float final = player.height;
                        foreach(float h in heightsOfTile) {
                            if(h != final) {
                                final = h;
                            }
                        }

                        player.height = final;
                    }
                }

                //計算player高度 ver 1
                // if(player.stair_start == "Stair_Top" && player.stair_end == "Stair_Down") {
                //     // 下樓梯
                //     player.height -= altitudeVariation;
                // }
                // else if(player.stair_start == "Stair_Down" && player.stair_end == "Stair_Top") {
                //     // 上樓梯
                //     player.height += altitudeVariation;
                // }
                // else {
                //     // TODO 途中離開樓梯
                //     float distance = Vector2.Distance( endPos, startPos );  // 計算途中離開樓梯區域的高度距離向量
                // }

                // Physics2D.IgnoreLayerCollision(6, 8, false);
                player.onStairs = false;
                player.stair_start = "Untagged";
                player.stair_end = "Untagged";
            }
        }
    }

    protected void OnCollisionStay2D(Collision2D other) {
        Debug.Log("OnCollisionStay2D");
    }

    private void FocusOnStairsColliders() {
        foreach(var collider2D in collider2Ds) {
            Debug.Log("collider2D name: "+collider2D.name);
            if(collider2D.gameObject.layer != LayerMask.NameToLayer("Trigger")) {
                //Debug.Log("collider2D tag: "+collider2D.tag);
                if(collider2D.tag != "Stairs") {
                    collider2D.enabled = false;
                }
            }
        }
    }
}
