using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform Map;
    [SerializeField] private Texture2D[] MapData;
    [SerializeField] private MapElement[] MapElements;
    [SerializeField] private Sprite defaultTile;
    
    private Vector3 WorldStartPos {
        get {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateMap() {
        Debug.Log("GenerateMap start");
        foreach(Texture2D layer in MapData) {
            for(int x = 0; x < layer.width; x++) {
                for(int y = 0; y < layer.height; y++) {
                    Color c = layer.GetPixel(x, y);
                    Debug.Log("layer RGBA: "+c.r + ", "+ c.g + ", "+c.b + ", " + c.a);
                    Debug.Log("element RGBA: "+MapElements[0].GetMapColor.r + ", "+ MapElements[0].GetMapColor.g + ", "+MapElements[0].GetMapColor.b + ", " + MapElements[0].GetMapColor.a);
                    MapElement newElement = Array.Find(MapElements, e => e.GetMapColor == c);

                    if(newElement != null) {
                        float xPos = WorldStartPos.x + (defaultTile.bounds.size.x * x);
                        float yPos = WorldStartPos.y + (defaultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.GetMapElementPrefab);
                        go.transform.position = new Vector2(xPos, yPos);
                        go.transform.parent = Map;
                    }
                }
            }
        }
        Debug.Log("GenerateMap end");
    }
}

[Serializable]
public class MapElement {
    [SerializeField] private string tileTag; 
    [SerializeField] private Color color;
    [SerializeField] private GameObject elementPrefab; 

    public GameObject GetMapElementPrefab{
        get {
            return elementPrefab;
        }
    }

    public Color GetMapColor {
        get {
            return color;
        }
    }

    public string GetMapTileTag {
        get {
            return tileTag;
        }
    }
}
