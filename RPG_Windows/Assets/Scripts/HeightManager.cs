using System.Collections;
using System.Collections.Generic;
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

            GetHeightFromTile(mousePosition);

            mousePosition = new Vector2(-2.8f, 1.3f);

            GetHeightFromTile(mousePosition);
            
        }
    }

    public List<float> GetHeightFromTile(Vector2 worldPosition) {
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
}
