using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public Sprite m_sprite;

    public Tile m_Tile;

    public Rigidbody2D player;

    public Rigidbody2D enemy;

    public Color[] pixels;

    [ContextMenu("Get Colors")]
    public void GetColors() {
        Debug.Log("rect: "+m_sprite.rect);
        Debug.Log("rect x: "+m_sprite.rect.x);
        Debug.Log("rect y: "+m_sprite.rect.y);
        Debug.Log("rect xMin: "+m_sprite.rect.xMin);
        Debug.Log("rect yMin: "+m_sprite.rect.yMin);
        Debug.Log("rect xMax: "+m_sprite.rect.xMax);
        Debug.Log("rect yMax: "+m_sprite.rect.yMax);
        Debug.Log("textureRect: "+m_sprite.textureRect);
        Debug.Log("textureRect x: "+m_sprite.textureRect.x);
        Debug.Log("textureRect y: "+m_sprite.textureRect.y);
        Debug.Log("textureRect xMin: "+m_sprite.textureRect.xMin);
        Debug.Log("textureRect yMin: "+m_sprite.textureRect.yMin);
        Debug.Log("textureRect xMin: "+m_sprite.textureRect.xMax);
        Debug.Log("textureRect yMin: "+m_sprite.textureRect.yMax);
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

    [ContextMenu("Get Matrix Test")]
    public void GetTileInfo() {
        float rad1 = 180 * Mathf.Deg2Rad;
        float rad2 = 90 * Mathf.Deg2Rad;
        float rad3 = 270 * Mathf.Deg2Rad;
        Matrix4x4 m1 = MatrixUtils.RotateZ(rad1);
        Matrix4x4 m2 = MatrixUtils.RotateZ(rad2);
        Matrix4x4 m3 = MatrixUtils.RotateZ(rad3);
        Debug.Log("m1: \n"+m1);
        Debug.Log("m2: \n"+m2);
        Debug.Log("m3: \n"+m3);
        Vector2 originPos = new Vector2(66, 255);
        Vector2 diagonalPos = originPos + new Vector2(32, 32);
        Vector2 xaxisPos = originPos + new Vector2(32, 0);
        Vector2 yaxisPos = originPos + new Vector2(0, 32);
        Vector2 centerPos = new Vector2(66, 255) + new Vector2(16, 16);
        Vector2 pos1 = new Vector2(97, 271);
        Vector2 pos2 = new Vector2(77, 282);
        Vector2 move1 = pos1 - centerPos;
        Vector2 move2 = pos2 - centerPos;
        
        Debug.Log("originPos: "+originPos);
        Debug.Log("diagonalPos: "+diagonalPos);
        Debug.Log("xaxisPos: "+xaxisPos);
        Debug.Log("yaxisPos: "+yaxisPos);
        Debug.Log("centerPos: "+centerPos);
        Debug.Log("pos1: "+pos1);
        Debug.Log("pos2: "+pos2);
        Debug.Log("move1: "+move1);
        Debug.Log("move2: "+move2);

        float moveX1 = move1.x * Mathf.Cos(rad1) + move1.y * Mathf.Sin(rad1);
        float moveY1 = move1.x * -Mathf.Sin(rad1) + move1.y * Mathf.Cos(rad1);
        Debug.Log("movement1 Vector: "+moveX1+", "+moveY1);

        Vector2 result1 = new Vector2(centerPos.x + moveX1, centerPos.y + moveY1);
        Debug.Log("result1: "+result1);

        float moveX2 = move2.x * Mathf.Cos(rad2) + move2.y * Mathf.Sin(rad2);
        float moveY2 = move2.x * -Mathf.Sin(rad2) + move2.y * Mathf.Cos(rad2);
        Debug.Log("movement2 Vector: "+moveX2+", "+moveY2);

        Vector2 result2 = new Vector2(centerPos.x + moveX2, centerPos.y + moveY2);
        Debug.Log("result1: "+result2);

        // float moveX3 = move2.x * m3.m00 + move2.y * m3.m01;
        // float moveY3 = move2.x * m3.m10 + move2.y * m3.m11;
        // Debug.Log("movement3 Vector: "+moveX3+", "+moveY3);

    }

    
    [ContextMenu("Test Add Force")]
    public void AddRigidbodyForce() {
        Debug.Log("AddRigidbodyForce");
        enemy.velocity = Vector3.zero;
        Vector3 oppositeDir = enemy.transform.position - player.transform.position;
        Vector3 oppositeForce = oppositeDir.normalized * 3;
        
        enemy.AddForce(oppositeForce, ForceMode2D.Impulse);
        StartCoroutine(KnockCo());
    }

    private IEnumerator KnockCo() {
        yield return new WaitForSeconds(0.4f);  // hardcasted casted time for debugged
        enemy.velocity = Vector3.zero;
    }
}
