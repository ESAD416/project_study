using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteModel
{
    public Sprite spriteOfTile { get; }
    public Vector2 originNode { get; }
    public float width { get; }
    public float height { get; }
    public float rotatedEulerAngles { get; }
    // Vector2 diagonalNode;
    // Vector2 xAxisNode;
    // Vector2 yAxisNode;
    // Vector2 center;
    // Vector2 targetNode;
    
    public TileSpriteModel(Sprite s, float eulerAngles) {
        this.spriteOfTile = s;
        this.originNode = new Vector2(this.spriteOfTile.rect.xMin, this.spriteOfTile.rect.yMin);
        this.width = spriteOfTile.rect.width;
        this.height = spriteOfTile.rect.height;
        this.rotatedEulerAngles = eulerAngles;
    }

    public Vector2 GetXYMaxNode() {
        return originNode + new Vector2(width, height);
    }

    public Vector2 GetXMaxNode() {
        return originNode + new Vector2(width, 0);
    }

    public Vector2 GetYMaxNode() {
        return originNode + new Vector2(0, height);
    }

    public Vector2 GetCenter() {
        return originNode + new Vector2(width / 2, height / 2);
    }

    public Vector2 GetMainDiagonal() {
        return GetXMaxNode() - GetYMaxNode();
    }

    public Vector2 GetSemiMainDiagonal() {
        return GetCenter() - GetYMaxNode();
    }

    public Vector2 GetAntiDiagonal() {
        return GetXYMaxNode() - this.originNode;
    }

    public Vector2 GetSemiAntiDiagonal() {
        return GetCenter() - this.originNode;
    }

    public Vector2 GetPixelsPosFromWorldPos(Vector2 worldPos) {
        float perX = worldPos.x - Mathf.Round(worldPos.x);
        float perY = worldPos.y - Mathf.Round(worldPos.y);
        if(perX < 0) {
            perX = 1f + perX;
        }
        if(perY < 0) {
            perY = 1f + perY;
        }
        int pixelX = (int) (this.originNode.x + Mathf.Round(perX * this.width));
        int pixelY = (int) (this.originNode.y + Mathf.Round(perY * this.height));
        return new Vector2(pixelX, pixelY);
    }



}
