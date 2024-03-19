using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

internal static class TileUtils
{
    public static bool TilePixelIsTransparent(TileSpriteModel model, Vector2 worldPos) {
        Vector2 v = model.GetPixelsPosFromWorldPos(worldPos) - model.GetCenter();
        float vX = v.x * Mathf.Cos(model.rotatedEulerAngles * Mathf.Deg2Rad) + v.y * Mathf.Sin(model.rotatedEulerAngles * Mathf.Deg2Rad);
        float vY = v.x * -Mathf.Sin(model.rotatedEulerAngles * Mathf.Deg2Rad) + v.y * Mathf.Cos(model.rotatedEulerAngles * Mathf.Deg2Rad);

        int pixelX = (int) (model.GetCenter().x + vX);
        int pixelY = (int) (model.GetCenter().y + vY);
        Color pixel = model.spriteOfTile.texture.GetPixel(pixelX, pixelY);
        Debug.Log("pixel Color: "+pixel);
        if(pixel.a == 0f) {
            Debug.Log("IsTransparent: "+true);
            return true;
        } else {
            Debug.Log("IsTransparent: "+false);
            return false;
        }
    }

    public static bool HasTileAtPlayerPosition(Tilemap currentTilemap, Bounds playerColliderBounds)
    {
        // 偵測Player的Collider對應位置是否有TileMap的地塊
        Vector3Int body_bottom_left = currentTilemap.WorldToCell(new Vector3(playerColliderBounds.min.x, playerColliderBounds.min.y));
        Vector3Int body_top_right = currentTilemap.WorldToCell(new Vector3(playerColliderBounds.max.x, playerColliderBounds.max.y));
        Vector3Int body_bottom_right = currentTilemap.WorldToCell(new Vector3(playerColliderBounds.max.x, playerColliderBounds.min.y));
        Vector3Int body_top_left = currentTilemap.WorldToCell(new Vector3(playerColliderBounds.min.x, playerColliderBounds.max.y));

        /*if (TileExistsAtPosition(currentTilemap, bottomLeft))
        {
            Debug.Log("bottomLeft");
        }

        if (TileExistsAtPosition(currentTilemap, bottomRight))
        {
            Debug.Log("bottomRight");
        }

        if (TileExistsAtPosition(currentTilemap, topLeft))
        {
            Debug.Log("topLeft");
        }

        if (TileExistsAtPosition(currentTilemap, topRight))
        {
            Debug.Log("topRight");
        }
        Debug.Log("IsTileAtPlayerPosition LEVEL:" + level);*/
        
        if (HasTileAtPosition(currentTilemap, body_bottom_left) || 
            HasTileAtPosition(currentTilemap, body_bottom_right) || 
            HasTileAtPosition(currentTilemap, body_top_left) || 
            HasTileAtPosition(currentTilemap, body_top_right))
        {
            return true; // 找到Tile
        }

        return false; // 未找到Tile*/

    }

    public static bool HasTileAtPosition(Tilemap currentTilemap, Vector3Int bottomLeft)
    {
        return currentTilemap.GetTile(bottomLeft) != null;
    }

}
