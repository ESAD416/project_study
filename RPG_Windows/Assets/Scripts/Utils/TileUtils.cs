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
        if(pixel.a == 0f) {
            Debug.Log("IsTransparent: "+true);
            return true;
        } else {
            Debug.Log("IsTransparent: "+false);
            return false;
        }
    }
}
