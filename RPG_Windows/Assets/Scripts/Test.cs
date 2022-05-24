using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Sprite m_sprite;

    public Color[] pixels;

    [ContextMenu("Get Colors")]
    public void GetColors() {
        var map = m_sprite.texture;
        Debug.Log(map.height);
        Debug.Log(map.width);
        pixels = map.GetPixels();
        for(int i = 0; i < 100; i++){
            Debug.Log("texture color: "+pixels[i]);
        }
    }

    public void SetColors() {
        
    }
}
