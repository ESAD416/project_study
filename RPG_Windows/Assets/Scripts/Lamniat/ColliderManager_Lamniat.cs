using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColliderManager_Lamniat : ColliderManager
{
    [SerializeField] private Lamniat m_collidingTarget;

    protected override void Awake() 
    {
        base.Awake();
    }

    void Start()
    {
        // Tilemap[] maps = transform.GetComponentsInChildren<Tilemap>();
        // levels = maps.Where(x => x.tag == "Level").ToArray();
    }


    void Update()
    {
        if(m_collidingTarget != null) 
        {
            if(m_collidingTarget.OnStairs)
                DisableTilemapCollision();
            else
                UpdateTilemapCollision();
        }
    }

    private void UpdateTilemapCollision()
    {
        foreach(var collider2D in tilemapColliders) {
            var heightLevel = collider2D.GetComponent<HeightOfLevel>() as HeightOfLevel;
            if(heightLevel != null) collider2D.gameObject.SetActive(m_collidingTarget.CurrentHeight == heightLevel.GetSelfHeight());
        }

        foreach(var trigger2D in tilemapTriggers) {
            var heightLevel = trigger2D.GetComponent<HeightOfLevel>() as HeightOfLevel;
            if(heightLevel != null) trigger2D.gameObject.SetActive(m_collidingTarget.CurrentHeight == heightLevel.GetSelfHeight());
        }
        

        // float newZPosition = -level - 1.9f;
        // transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
    
        // 根据需要继续设置其他level的Collider
    }

    private void DisableTilemapCollision()
    {
        foreach(var collider2D in tilemapColliders) {
            collider2D.gameObject.SetActive(false);
        }

        foreach(var trigger2D in tilemapTriggers) {
            trigger2D.gameObject.SetActive(false);
        }
    }
}
