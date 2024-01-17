using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using Fungus;

public class HeightManager : MonoBehaviour
{
    [SerializeField] private Avatar m_avatar;
    [SerializeField] private Jump_Lamniat m_avatarJump;
    [SerializeField] private Grid mapGrid;
    private Collider2D[] mapColliders;

    public List<TileData> defaultTileDatas;
    private Tilemap[] levels;
    private Dictionary<Tile, TileData> dataFromTiles;

    private void Awake() {
        dataFromTiles = new Dictionary<Tile, TileData>();
        foreach(TileData data in defaultTileDatas) {
            foreach(Tile tile in data.tiles) {
                //Debug.Log("tile name: "+tile.name);
                dataFromTiles.Add(tile, data);
            }
        }

        mapColliders = mapGrid.GetComponentsInChildren<Collider2D>().Where(c => c.GetType() != typeof(CompositeCollider2D)).ToArray();
    }

    void Start()
    {
        Tilemap[] maps = transform.GetComponentsInChildren<Tilemap>();
        levels = maps.Where(x => x.tag == "Level").ToArray();
    }


    void Update()
    {
        if(m_avatar != null) {
            if(m_avatarJump != null && m_avatarJump.IsJumping) {
                SetCollidersWithAvatarHeightWhileAvatarJump(m_avatar.CurrentHeight);
            } else {
                SetCollidersWithAvatarHeight(m_avatar.CurrentHeight);
            }
        }
        
    }

    public float GetHeightByTileBase(Tile tileBase) {
        return dataFromTiles[tileBase].height;
    }

    public List<float> GetHeightsFromTileMapsByWorldPos(Vector2 worldPosition) {
        List<float> result = new List<float>();
        
        foreach(var map in levels) {
            Vector3Int gridPos = map.WorldToCell(worldPosition);
            if(map.HasTile(gridPos)) {
                Tile resultTile = map.GetTile<Tile>(gridPos);
                //Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+map.name);

                result.Add(dataFromTiles[resultTile].height);
            }
            
        }

        return result;
    }

    public bool GroundableChecked(Vector2 worldPos, float height) {
        Debug.Log("groundCheck worldPos: "+worldPos);
        Debug.Log("groundCheck height: "+height);
        foreach(var map in levels) {
            Vector3Int gridPos = map.WorldToCell(worldPos);
            if(map.HasTile(gridPos)) {
                Tile resultTile = map.GetTile<Tile>(gridPos);
                Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+map.name);
                Debug.Log("resultTile height: "+dataFromTiles[resultTile].height);
                if(dataFromTiles[resultTile].height == height) {
                    TileSpriteModel model = new TileSpriteModel(resultTile.sprite, map.GetTransformMatrix(gridPos).rotation.eulerAngles.z);
                    bool IsTransparent = TileUtils.TilePixelIsTransparent(model, worldPos);
                    return !IsTransparent;
                    // if(!IsTransparent) {
                    //     bool hasCeiling = CeilingChecked(worldPos, height);
                    //     return !hasCeiling;
                    // }
                }
            }
        }
        return false;
    }

    public bool NotGroundableChecked(Vector2 worldPos, float height) {
        // Debug.Log("NotGroundable Check worldPos: "+worldPos+" height: "+height);
        // for(float i = height; i >= 0; i--) {
            // Debug.Log("NotGroundable Check i: "+i);
            foreach(var map in levels) {
                Vector3Int gridPos = map.WorldToCell(worldPos);
                if(map.HasTile(gridPos)) {
                    Tile resultTile = map.GetTile<Tile>(gridPos);
                    if(dataFromTiles[resultTile].height == height) {
                        Debug.Log("NotGroundable, worldPos" + worldPos + " height: "+dataFromTiles[resultTile].height);
                        var notGroundable = map.GetComponentsInChildren<Tilemap>().Where(x => x.tag != "Level").FirstOrDefault();
                        gridPos = notGroundable.WorldToCell(worldPos);
                        if(notGroundable.HasTile(gridPos)) {
                            resultTile = notGroundable.GetTile<Tile>(gridPos);
                            Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+notGroundable.name);
                            TileSpriteModel model = new TileSpriteModel(resultTile.sprite, notGroundable.GetTransformMatrix(gridPos).rotation.eulerAngles.z);
                            bool IsTransparent = TileUtils.TilePixelIsTransparent(model, worldPos);
                            return !IsTransparent;
                        }
                    }
                }
            }
        // }
        
        return false;
    }

    
    public bool CeilingChecked(Vector2 worldPos, float height) {
        Debug.Log("CeilingChecked worldPos: "+worldPos);
        Debug.Log("CeilingChecked height: "+height);
        foreach(var map in levels) {
            Vector3Int gridPos = map.WorldToCell(worldPos);
            if(map.HasTile(gridPos)) {
                Tile resultTile = map.GetTile<Tile>(gridPos);
                Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+map.name);
                Debug.Log("resultTile height: "+dataFromTiles[resultTile].height);
                if(dataFromTiles[resultTile].height > height) {
                    TileSpriteModel model = new TileSpriteModel(resultTile.sprite, map.GetTransformMatrix(gridPos).rotation.eulerAngles.z);
                    bool IsTransparent = TileUtils.TilePixelIsTransparent(model, worldPos);
                    return !IsTransparent;
                }
            }
        }
        return false;
    }

    public void SetCollidersWithAvatarHeight(float avatarHeight) {
        if(mapColliders != null) {
            foreach(var collider2D in mapColliders) {
                var heightObj = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    // Debug.Log("SetCollidersWithAvatarHeight collider2D name: "+collider2D.name);
                    // Debug.Log("SetCollidersWithAvatarHeight collider2D type: "+collider2D.GetType());
                    if(avatarHeight == heightObj.GetSelfHeight()) {
                        collider2D.enabled = true;
                    } else {
                        collider2D.enabled = false;
                    }
                    // Debug.Log("SetCollidersWithAvatarHeight collider2D enabled: "+collider2D.enabled);
                }
            }
        }
    }

    public void SetCollidersWithAvatarHeightWhileAvatarJump(float avatarHeight) {
        if(mapColliders != null) {
            foreach(var collider2D in mapColliders) {
                var heightObj = collider2D.GetComponent<HeightOfObject>() as HeightOfObject;
                if(heightObj != null) {
                    float groundCheckHeight = Mathf.Floor(avatarHeight);
                    float maxGroundCheckHeight = groundCheckHeight + 1;
                    // Debug.Log("FocusCollidersWithHeight collider2D name: "+collider2D.name);
                    // Debug.Log("FocusCollidersWithHeight collider2D type: "+collider2D.GetType());
                    if(maxGroundCheckHeight == heightObj.GetSelfHeight()) {
                        collider2D.enabled = true;
                    } else {
                        collider2D.enabled = false;
                    }
                    // Debug.Log("FocusCollidersWithHeight collider2D enabled: "+collider2D.enabled);
                }
            }
        }
    }

    
}
