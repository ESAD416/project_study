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
        // List<float> levelsHeight = defaultTileDatas.OrderByDescending(h => h.height).Select(h => h.height).ToList();   // 取現有Level的高，由高至低排序
        // float groundCheckH = Mathf.Floor(height);

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
                }
            }
        }
        return false;
    }

    public bool GroundableChecked(Vector3 coordinate) {
        List<float> levelsHeight = defaultTileDatas.OrderByDescending(h => h.height).Select(h => h.height).ToList();   // 取現有Level的高，由高至低排序
        float groundCheckH = Mathf.Floor(coordinate.z);

        foreach(var h in levelsHeight) {
            Vector3 shadowCoordinate = new Vector3(coordinate.x, coordinate.y, groundCheckH);
            Vector3 worldPos = new Vector3(shadowCoordinate.x, shadowCoordinate.y + shadowCoordinate.z);
            Debug.Log("groundCheck shadowCoordinate: "+shadowCoordinate);
            Debug.Log("groundCheck worldPos: "+worldPos);

            foreach(var map in levels) {
                Vector3Int gridPos = map.WorldToCell(worldPos);
                if(map.HasTile(gridPos)) {
                    Tile resultTile = map.GetTile<Tile>(gridPos);
                    Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+map.name);
                    Debug.Log("resultTile height: "+dataFromTiles[resultTile].height);
                    Debug.Log("groundCheck height: "+groundCheckH);
                    if(dataFromTiles[resultTile].height == groundCheckH) {
                        TileSpriteModel model = new TileSpriteModel(resultTile.sprite, map.GetTransformMatrix(gridPos).rotation.eulerAngles.z);
                        bool IsTransparent = TileUtils.TilePixelIsTransparent(model, worldPos);
                        if(!IsTransparent) {
                            return true;
                        }
                    }
                }
            }

            groundCheckH--;
        }
        
        return false;
    }
}
