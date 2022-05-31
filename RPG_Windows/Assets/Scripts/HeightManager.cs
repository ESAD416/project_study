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
        Debug.Log("worldPos: "+worldPos);
        Debug.Log("height: "+height);
        foreach(var map in levels) {
            Vector3Int gridPos = map.WorldToCell(worldPos);
            Debug.Log("Matrix: \n"+map.GetTransformMatrix(gridPos));
            Debug.Log("rotation: "+map.GetTransformMatrix(gridPos).rotation);
            Debug.Log("eulerAngles: "+map.GetTransformMatrix(gridPos).rotation.eulerAngles);
            if(map.HasTile(gridPos)) {
                Tile resultTile = map.GetTile<Tile>(gridPos);
                Debug.Log("At grid position "+gridPos+" there is a "+resultTile+" in map "+map.name);
                Debug.Log("resultTile height: "+dataFromTiles[resultTile].height);
                if(dataFromTiles[resultTile].height == height) {
                    // TODO Get Sptite to get color
                    // Sprite s = resultTile.sprite;
                    // Debug.Log("sprite name: "+s.name);
                    
                    
                    // float perX = worldPos.x - Mathf.Round(worldPos.x);
                    // float perY = worldPos.y - Mathf.Round(worldPos.y);
                    // if(perX < 0) {
                    //     perX = 1f + perX;
                    // }
                    // if(perY < 0) {
                    //     perY = 1f + perY;
                    // }
                    // Debug.Log("perX: "+perX);
                    // Debug.Log("perY: "+perY);


                    // int pixelX = (int) (s.rect.xMin + Mathf.Round(perX * textureWidth));
                    // int pixelY = (int) (s.rect.yMin + Mathf.Round(perY * textureHeight));
                    // Debug.Log("pixelX: "+pixelX);
                    // Debug.Log("pixelY: "+pixelY);

                    TileSpriteModel model = new TileSpriteModel(resultTile.sprite, map.GetTransformMatrix(gridPos).rotation.eulerAngles.z);
                    return !TileUtils.TilePixelIsTransparent(model, worldPos);
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
