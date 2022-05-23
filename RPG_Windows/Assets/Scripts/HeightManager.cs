using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class HeightManager : MonoBehaviour
{
    [SerializeField]
    private List<TileData> defaultTileDatas;
    private Tilemap[] maps;
    private Dictionary<TileBase, TileData> dataFromTiles;
    Vector2 mousePosition;

    void Start()
    {
        maps = transform.GetComponentsInChildren<Tilemap>();
    }

    private void Awake() {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach(TileData data in defaultTileDatas) {
            foreach(TileBase tile in data.tiles) {
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

    public float GetHeightByTileBase(TileBase tileBase) {
        return dataFromTiles[tileBase].height;
    }

    public List<float> GetHeightsFromTileMapsByWorldPos(Vector2 worldPosition) {
        List<float> result = new List<float>();
        foreach(var map in maps) {
            Vector3Int gridPos = map.WorldToCell(worldPosition);
            if(map.HasTile(gridPos)) {
                TileBase resultTile = map.GetTile(gridPos);
                //Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+map.name);

                result.Add(dataFromTiles[resultTile].height);
            }
            
        }

        return result;
    }

    public bool GroundableChecked(Vector2 worldPos, float height) {
        foreach(var map in maps) {
            Vector3Int gridPos = map.WorldToCell(worldPos);
            if(map.HasTile(gridPos)) {
                TileBase resultTile = map.GetTile(gridPos);
                //Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+map.name);
                if(dataFromTiles[resultTile].height == height) {
                    // TODO Get Sptite to get color 
                    return true;
                }
            }
        }
        return false;
    }

    public Vector3 GetCoordinate(Vector2 worldPosition) {
        Vector3 result = new Vector3();
        List<float> height = GetHeightsFromTileMapsByWorldPos(worldPosition);
        if(height.Count == 1) {
            Debug.Log("height.Count == 1");
            result.x = worldPosition.x;
            result.z = height.First();
            result.y =  worldPosition.y - result.z;
        } else if(height.Count > 1){
            // TODO
            Debug.Log("height.Count > 1");
        }
        Debug.Log("At world pos "+worldPosition+" to Coordinate "+result);
        return result;
    }

    public Vector2 GetWorldPosFromCoordinate(Vector3 coordinate) {
        Vector2 result = new Vector2();
        result.x = coordinate.x;
        result.y = coordinate.x + coordinate.z;
        return result;
    }
}
