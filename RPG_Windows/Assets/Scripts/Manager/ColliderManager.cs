using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColliderManager : MonoBehaviour
{
    public static ColliderManager instance;

    [SerializeField] protected Tilemap[] mapLevels;
    [SerializeField] protected Collider2D[] tilemapColliders;
    [SerializeField] protected Collider2D[] tilemapTriggers;
    [SerializeField] protected int minimumLevel = 0;
    public int MinimumLevel => minimumLevel;

    protected virtual void Awake() 
    {
        if(instance == null) 
        {
            instance = this;
        }
    }

    void Start()
    {
    }


    void Update()
    {
    }

    public Tilemap GetCurrentTilemapByAvatarHeight(int avatarHeight) {
        foreach(var level in mapLevels) 
        {
            var heightOfLevel = level.GetComponent<HeightOfLevel>() as HeightOfLevel;
            if(heightOfLevel != null && heightOfLevel.GetSelfHeight() == avatarHeight) 
            {
                return level;
            }
        }
        return null;
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

    public void SetCollidersWithAvatarHeight(float avatarHeight) {
        // if(mapColliders != null) {
        //     foreach(var collider2D in mapColliders) {
        //         var heightObj = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
        //         if(heightObj != null) {
        //             // Debug.Log("SetCollidersWithAvatarHeight collider2D name: "+collider2D.name);
        //             // Debug.Log("SetCollidersWithAvatarHeight collider2D type: "+collider2D.GetType());
        //             if(avatarHeight == heightObj.GetSelfHeight()) {
        //                 collider2D.enabled = true;
        //             } else {
        //                 collider2D.enabled = false;
        //             }
        //             // Debug.Log("SetCollidersWithAvatarHeight collider2D enabled: "+collider2D.enabled);
        //         }
        //     }
        // }
    }

    public void SetCollidersWithAvatarHeightWhileAvatarJump(float avatarHeight) {
        // if(mapColliders != null) {
        //     foreach(var collider2D in mapColliders) {
        //         var heightObj = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
        //         if(heightObj != null) {
        //             float groundCheckHeight = Mathf.Floor(avatarHeight);
        //             float maxGroundCheckHeight = groundCheckHeight + 1;
        //             // Debug.Log("FocusCollidersWithHeight collider2D name: "+collider2D.name);
        //             // Debug.Log("FocusCollidersWithHeight collider2D type: "+collider2D.GetType());
        //             if(maxGroundCheckHeight == heightObj.GetSelfHeight()) {
        //                 collider2D.enabled = true;
        //             } else {
        //                 collider2D.enabled = false;
        //             }
        //             // Debug.Log("FocusCollidersWithHeight collider2D enabled: "+collider2D.enabled);
        //         }
        //     }
        // }
    }

    
}
