using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator_Area : MonoBehaviour
{
    [Range(0, 1)]
    public float areaValue = 0f;
    public float duration;
    [SerializeField] private Transform fillArea;
    public SpriteRenderer SprtRenderer;
    
    private float elapsedTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        SprtRenderer = GetComponent<SpriteRenderer>();
        Bounds bounds = SprtRenderer.sprite.bounds;
        Vector3 scale = SprtRenderer.transform.localScale;

        // 计算实际半径，考虑 Scale
        float radius = bounds.size.x * scale.x / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        // 遞增時間
        elapsedTime += Time.deltaTime;

        // 計算當前值（根據時間的插值）
        float t = Mathf.Clamp01(elapsedTime / duration);
        areaValue = Mathf.Lerp(0, 1f, t);

        fillArea.localScale = new Vector3(areaValue, areaValue, 1);

        if(elapsedTime >= duration) {
            // 確保最終值正確
            elapsedTime = duration;
            areaValue = 1f;
            fillArea.localScale = new Vector3(areaValue, areaValue, 1);
        }
    }

}
