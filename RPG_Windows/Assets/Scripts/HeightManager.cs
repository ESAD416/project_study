using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class HeightManager : MonoBehaviour
{
    [SerializeField]
    public List<TileData> defaultTileDatas;
    private Tilemap[] levels;
    private Dictionary<Tile, TileData> dataFromTiles;
    Vector2 mousePosition;

    void Start()
    {
        Tilemap[] maps = transform.GetComponentsInChildren<Tilemap>();
        levels = maps.Where(x => x.tag == "Level").ToArray();
    }

    private void Awake() {
        dataFromTiles = new Dictionary<Tile, TileData>();
        foreach(TileData data in defaultTileDatas) {
            foreach(Tile tile in data.tiles) {
                //Debug.Log("tile name: "+tile.name);
                dataFromTiles.Add(tile, data);
            }
        }
    }

    void Update()
    {
        //var projectedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
    }

    public void OnTestClick(InputAction.CallbackContext value) {
        if(value.started) {
            mousePosition = new Vector2(0, 0);

            //GetHeightFromTile(mousePosition);
            //GetCoordinate(mousePosition);

            mousePosition = new Vector2(-3.2f, -2.7f);

            //GetHeightFromTile(mousePosition);
            //GetCoordinate(mousePosition);
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
        Debug.Log("NotGroundable Check worldPos: "+worldPos);
        foreach(var map in levels) {
            Vector3Int gridPos = map.WorldToCell(worldPos);
            if(map.HasTile(gridPos)) {
                Tile resultTile = map.GetTile<Tile>(gridPos);
                if(dataFromTiles[resultTile].height == height) {
                    Debug.Log("NotGroundable height: "+dataFromTiles[resultTile].height);
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
}
