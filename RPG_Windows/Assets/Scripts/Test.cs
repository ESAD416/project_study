using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public Sprite m_sprite;

    public Tile m_Tile;

    public Color[] pixels;

    [ContextMenu("Get Colors")]
    public void GetColors() {
        Debug.Log("rect: "+m_sprite.rect);
        Debug.Log("textureRect: "+m_sprite.textureRect);
        Debug.Log("textureRectOffset: "+m_sprite.textureRectOffset);

        // var map = m_sprite.texture;
        // Debug.Log(map.height);
        // Debug.Log(map.width);
        // int perX = 0;
        // int perY = 0;
        // pixels = map.GetPixels();
        // foreach(Color col in pixels) {
        //     if(col.a != 0f) {
        //         Debug.Log("perX: "+perX+"perY: "+perY);
        //     }

        //     if(perX < map.width) {
        //         perX++;
        //     } else {
        //         perX = 0;
        //         perY++;
        //     }
        // }
    }

    [ContextMenu("Get TileInfo")]
    public void GetTileInfo() {
        // Debug.Log("gameObj name: "+m_Tile.gameObject.name);
        // Debug.Log(m_Tile.)
        
    }

    public void SetColors() {
        
    }
}
