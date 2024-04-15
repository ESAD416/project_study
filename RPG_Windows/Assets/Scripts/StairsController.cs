using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StairsController : MonoBehaviour
{
    [SerializeField] protected Avatar m_avatar;
    [SerializeField] protected Tilemap[] stairsTunnelsTileMaps;

    protected void Start() {
        // Debug.Log("Stairs gateWayPos_0: "+gateWayPos_top);
        // Debug.Log("Stairs gateWayPos_1: "+gateWayPos_down);
        // Debug.Log("Stairs Distance: "+stairDistance);
    }    

    public bool OnStairsAtPlayerPosition(Bounds playerColliderBounds, bool isAndTrue = false) {
        Debug.Log("StairsController start stairsTunnelsTileMaps length"+stairsTunnelsTileMaps.Length);
        foreach(Tilemap tilemap in stairsTunnelsTileMaps) {
            // 偵測Player的Collider對應位置是否有TileMap的地塊
            // Debug.Log("StairsController tilemap "+tilemap.name);

            Vector3Int body_bottom_left = tilemap.WorldToCell(new Vector3(playerColliderBounds.min.x, playerColliderBounds.min.y));
            Vector3Int body_top_right = tilemap.WorldToCell(new Vector3(playerColliderBounds.max.x, playerColliderBounds.max.y));
            Vector3Int body_bottom_right = tilemap.WorldToCell(new Vector3(playerColliderBounds.max.x, playerColliderBounds.min.y));
            Vector3Int body_top_left = tilemap.WorldToCell(new Vector3(playerColliderBounds.min.x, playerColliderBounds.max.y));

            // Debug.Log("body_bottom_left: "+body_bottom_left);
            // Debug.Log("body_top_right: "+body_top_right);
            // Debug.Log("body_bottom_right: "+body_bottom_right);
            // Debug.Log("body_top_left: "+body_top_left);

            if(isAndTrue) {
                if (TileUtils.HasTileAtPosition(tilemap, body_bottom_left) && 
                    TileUtils.HasTileAtPosition(tilemap, body_bottom_right) && 
                    TileUtils.HasTileAtPosition(tilemap, body_top_left) && 
                    TileUtils.HasTileAtPosition(tilemap, body_top_right))
                {
                    Debug.Log("StairsController true: isAndTrue = true");
                    return true; // 找到Tile
                }
            } else {
                if (TileUtils.HasTileAtPosition(tilemap, body_bottom_left) || 
                    TileUtils.HasTileAtPosition(tilemap, body_bottom_right) || 
                    TileUtils.HasTileAtPosition(tilemap, body_top_left) || 
                    TileUtils.HasTileAtPosition(tilemap, body_top_right))
                {
                    Debug.Log("StairsController true: isAndTrue = false");
                    return true; // 找到Tile
                }
            }
        }

        Debug.Log("StairsController false");
        return false;
    }

    // protected void OnTriggerEnter2D(Collider2D otherCollider) {
    //     //Debug.Log("Stairs_Tunnel OnTriggerEnter2D name: "+otherCollider.gameObject.name);
    //     if(otherCollider.gameObject.tag == "Player" && otherCollider.gameObject.layer !=  LayerMask.NameToLayer("Hittable")) {
    //         Debug.Log("player enter in Stairs_Tunnel "+otherCollider.gameObject.name);
    //         m_gateWay_Start =  m_gateWays.Where(x => x.StairState == m_avatar.OnStairState).FirstOrDefault();
    //         m_gateWay_End =  m_gateWays.Where(x => x.StairState != m_avatar.OnStairState).FirstOrDefault();
    //     }
    // }

    // protected void OnTriggerExit2D(Collider2D otherCollider) {
    //     //Debug.Log("Stairs_Tunnel OnTriggerEnter2D name: "+otherCollider.gameObject.name);
    //     if(otherCollider.gameObject.tag == "Player" && otherCollider.gameObject.layer !=  LayerMask.NameToLayer("Hittable")) {
    //         Debug.Log("player leave Stairs_Tunnel");
    //         InitAvatarStairsStatus();
    //     }
    // }

    // private void InitAvatarStairsStatus() 
    // {
    //     m_avatar.OnStairs = false;
    // }

    // public float SetPlayerHeightOnStair() {
    //     Debug.Log("playerc orgin height: "+playerOrginHeight);
    //     float result = playerOrginHeight;
        
    //     Vector2 startPos = new Vector2();
    //     float dir = 1f;
    //     if(player.stair_start == "Stair_Top") {
    //         // 下樓梯(起點高處)
    //         startPos = gateWayPos_top;
    //         dir = -1f;
    //     }
    //     else if(player.stair_start == "Stair_Down") {
    //         // 上樓梯(起點低處)
    //         startPos = gateWayPos_down;
    //         dir = 1f;
    //     }

    //     float limitedHeight = playerOrginHeight + altitudeVariation * dir;
    //     Debug.Log("height var dir: "+dir);
    //     Debug.Log("stair startPos: "+startPos);
    //     Debug.Log("player curr centerPos: "+player.Center);
    //     Debug.Log("limitedHeight: "+limitedHeight);

    //     float playerMoveDist;
    //     if(isVertical) {
    //         playerMoveDist = Math.Abs(startPos.y - player.Center.y);
    //     } else {
    //         playerMoveDist = Math.Abs(startPos.x - player.Center.x);
    //     }
    //     Debug.Log("playerMoveDist: "+playerMoveDist);
    //     Debug.Log("( playerMoveDist / stairDistance ) : "+( playerMoveDist / stairDistance ));

    //     result += ( playerMoveDist / stairDistance ) * altitudeVariation * dir;
    //     bool overflowHeight = (dir == 1f && result > limitedHeight) || (dir == -1f && result < limitedHeight);
    //     if(player.stair_start == player.stair_end) {
    //         result = playerOrginHeight;
    //     } else if(overflowHeight) {
    //         result = limitedHeight;
    //     } 

    //     return result;
    // }

    

    // private void FocusOnStairsColliders() {
    //     foreach(var collider2D in collider2Ds) {
    //         if(collider2D.tag != "Stairs" && collider2D.tag != "Stair_Top" && collider2D.tag != "Stair_Down") {
    //             collider2D.enabled = false;
    //         }
    //     }
    // }
    
    // private void RevertFocusOnStairsColliders() {
    //     foreach(var collider2D in collider2Ds) {
    //         if(collider2D.tag != "Stairs" && collider2D.tag != "Stair_Top" && collider2D.tag != "Stair_Down") {
    //             collider2D.enabled = true;
    //         }
    //     }
    // }
}
