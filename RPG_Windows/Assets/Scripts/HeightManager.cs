using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class HeightManager : MonoBehaviour
{
    [Header("Input Test")]
    public PlayerInput playerInput;

    private Tilemap[] maps;

    Vector2 mousePosition;



    void Start()
    {
        maps = transform.GetComponentsInChildren<Tilemap>();
    }

    void Update()
    {
        //var projectedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
    }

    public void OnTestClick(InputAction.CallbackContext value) {
        if(value.started) {
            mousePosition = new Vector2(0, 0);
            Debug.Log("HeightManager click, mousePosition: "+mousePosition);
            foreach(var map in maps) {
                Vector3Int gridPos = map.WorldToCell(mousePosition);
                TileBase clickedTile = map.GetTile(gridPos);

                Debug.Log("At position "+gridPos+" there is a "+clickedTile+" in map "+map.name);
            }

            mousePosition = new Vector2(-2.8f, 1.3f);
            Debug.Log("HeightManager click, mousePosition: "+mousePosition);
            foreach(var map in maps) {
                Vector3Int gridPos = map.WorldToCell(mousePosition);
                TileBase clickedTile = map.GetTile(gridPos);

                Debug.Log("At position "+gridPos+" there is a "+clickedTile+" in map "+map.name);
            }
            
        }
        
    }

    public void OnPos(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        
    }

    // public float GetHeightFromTile(Vector2 worldPosition) {
    //     Vector3Int gridPos = 
    // }
}
