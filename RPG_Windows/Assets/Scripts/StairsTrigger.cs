using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    public float altitudeVariation = 1f;
    public bool isVertical;
    public float stairDistance;

    protected Player player;
    protected float playerOrginHeight;
    protected Coroutine heightSettleRoutine;
    protected Vector2 gateWayPos_top;
    protected Vector2 gateWayPos_down;

    Collider2D[] collider2Ds;

    protected void Start() {
        player = GameObject.FindObjectOfType(typeof(Player)) as Player;
        var grid = GameObject.FindObjectOfType(typeof(Grid)) as Grid;
        collider2Ds = grid.GetComponentsInChildren<Collider2D>();

        // InitStairsStatus();

        var parent = gameObject.transform.parent.gameObject;
        var top = parent.GetComponentsInChildren<EdgeCollider2D>().Where(collider => collider.tag == "Stair_Top").FirstOrDefault();
        gateWayPos_top = top.gameObject.transform.position;
        var down = parent.GetComponentsInChildren<EdgeCollider2D>().Where(collider => collider.tag == "Stair_Down").FirstOrDefault();
        gateWayPos_down = down.gameObject.transform.position;
        if(isVertical) {
            stairDistance = Math.Abs(gateWayPos_top.y - gateWayPos_down.y) ;
        } else {
            stairDistance = Math.Abs(gateWayPos_top.x - gateWayPos_down.x) ;
        }


        // Debug.Log("Stairs gateWayPos_0: "+gateWayPos_top);
        // Debug.Log("Stairs gateWayPos_1: "+gateWayPos_down);
        // Debug.Log("Stairs Distance: "+stairDistance);
    }    

    protected void OnTriggerEnter2D(Collider2D otherCollider) {
        //Debug.Log("Enter otherCollider name: "+otherCollider.gameObject.name);
        if(otherCollider.gameObject.tag == "Player") {
            Debug.Log("player enter in StairsTrigger");
            player.onStairs = gameObject.name;
            playerOrginHeight = player.currHeight;
            // if(string.IsNullOrEmpty(player.onStairs)) {
            //     Debug.Log("player not in StairsTrigger yet");
            //     playerStartHeight = player.height;
            //     // Debug.Log("startPos: "+startPos);
            // }

            FocusOnStairsColliders();
            //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Blocks"));
        }
    }

    protected void OnTriggerExit2D(Collider2D otherCollider) {
        //Debug.Log("Exit otherCollider name: "+otherCollider.gameObject.name);
        if(otherCollider.gameObject.tag == "Player") {
            Debug.Log("player leave StairsTrigger");
            InitStairsStatus();
            // if(string.IsNullOrEmpty(player.onStairs)) {
            //     Debug.Log("player enter in StairsTrigger");
            //     player.onStairs = gameObject.name;
                
            // } else {
            //     Debug.Log("player prepare to leave StairsTrigger");
            //     // endPos = new Vector2(player.gameObject.transform.position.x, player.gameObject.transform.position.y);
            //     // Debug.Log("endPos: "+endPos);
            //     //heightSettleRoutine = StartCoroutine(HeightSettleDown());
            // }
        }
    }

    private IEnumerator HeightSettleDown() {
        //計算player高度 ver 2
        var heightManager = GameObject.FindObjectOfType(typeof(HeightManager)) as HeightManager; 
        List<float> heightsOfTile = heightManager.GetHeightsFromTileMapsByWorldPos(player.m_Center);
        if(heightsOfTile.Count > 0) {
            if(heightsOfTile.Count == 1) {
                float first = heightsOfTile.First();
                player.currHeight = first;
            }
            else {
                float final = player.currHeight;
                foreach(float h in heightsOfTile) {
                    if(h != final) {
                        final = h;
                    }
                }

                player.currHeight = final;
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
        yield return new WaitForSeconds(0.2f);

        InitStairsStatus();
    }

    public float SetPlayerHeightOnStair() {
        Debug.Log("playerc orgin height: "+playerOrginHeight);
        float result = playerOrginHeight;
        
        Vector2 startPos = new Vector2();
        float dir = 1f;
        if(player.stair_start == "Stair_Top") {
            // 下樓梯(起點高處)
            startPos = gateWayPos_top;
            dir = -1f;
        }
        else if(player.stair_start == "Stair_Down") {
            // 上樓梯(起點低處)
            startPos = gateWayPos_down;
            dir = 1f;
        }
        float limitedHeight = playerOrginHeight + altitudeVariation * dir;
        Debug.Log("height var dir: "+dir);
        Debug.Log("stair startPos: "+startPos);
        Debug.Log("player curr centerPos: "+player.m_Center);
        Debug.Log("limitedHeight: "+limitedHeight);

        float playerMoveDist;
        if(isVertical) {
            playerMoveDist = Math.Abs(startPos.y - player.m_Center.y);
        } else {
            playerMoveDist = Math.Abs(startPos.x - player.m_Center.x);
        }
        Debug.Log("playerMoveDist: "+playerMoveDist);
        Debug.Log("( playerMoveDist / stairDistance ) : "+( playerMoveDist / stairDistance ));

        result += ( playerMoveDist / stairDistance ) * altitudeVariation * dir;
        bool overflowHeight = (dir == 1f && result > limitedHeight) || (dir == -1f && result < limitedHeight);
        if(player.stair_start == player.stair_end) {
            result = playerOrginHeight;
        } else if(overflowHeight) {
            result = limitedHeight;
        } 

        return result;
    }

    private void InitStairsStatus() {
        if(heightSettleRoutine != null) {
            StopCoroutine(heightSettleRoutine);
        }

        player.onStairs = string.Empty;
        player.stair_start = string.Empty;
        player.stair_end = string.Empty;
        RevertFocusOnStairsColliders();
    }

    private void FocusOnStairsColliders() {
        foreach(var collider2D in collider2Ds) {
            //Debug.Log("collider2D name: "+collider2D.name);
            if(collider2D.gameObject.layer != LayerMask.NameToLayer("Trigger")) {
                //Debug.Log("collider2D tag: "+collider2D.tag);
                if(collider2D.tag != "Stairs") {
                    collider2D.enabled = false;
                }
            }
        }
    }
    
    private void RevertFocusOnStairsColliders() {
        foreach(var collider2D in collider2Ds) {
            //Debug.Log("collider2D name: "+collider2D.name);
            if(collider2D.gameObject.layer != LayerMask.NameToLayer("Trigger")) {
                //Debug.Log("collider2D tag: "+collider2D.tag);
                if(collider2D.tag != "Stairs") {
                    collider2D.enabled = true;
                }
            }
        }
    }
}
