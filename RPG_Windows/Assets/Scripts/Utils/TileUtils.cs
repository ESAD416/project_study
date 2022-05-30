using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TileUtils
{
    public bool TilePixelIsTransparent(TileSpriteModel model, Vector2 worldPos) {
        Vector2 v = model.GetSemiAntiDiagonal();
        float vX = v.x * Mathf.Cos(model.rotatedEulerAngles * Mathf.Deg2Rad) + v.y * Mathf.Sin(model.rotatedEulerAngles * Mathf.Deg2Rad);
        float vY = v.x * -Mathf.Sin(model.rotatedEulerAngles * Mathf.Deg2Rad) + v.y * Mathf.Cos(model.rotatedEulerAngles * Mathf.Deg2Rad);

        Vector2 adjustPixelPos = new Vector2(model.GetCenter().x + vX, model.GetCenter().y + vY);
        Color pixel = model.spriteOfTile.texture.GetPixel(adjustPixelPos.x, adjustPixelPos.y);
        if(pixel.a == 0f) {
            return false;
        } else {
            return true;
        }
    }
}
