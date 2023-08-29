using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class AOEIndicatorCtrl : MonoBehaviour
{
    [SerializeField] private Enemy_Horizontal boss;
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private int aoeCount = 4;
    [SerializeField] private Vector2 areaMin;
    [SerializeField] private Vector2 areaMax;
    [SerializeField] private float checkFrequency = 0.1f;
    [SerializeField] private float offsetDistance = 1f;

    [SerializeField] private Vector3 trailedPos = Vector3.zero;
    [SerializeField] private float trailOffset = 3f;
    private float aC = 0.1f;
    private float randomAngleRangeOfDir = 45f;
    private Vector3 trailDir = Vector3.right;

    public enum Area {
        area1, area2, area3, area4, area5, area6, area7, area8, area9, area10,
        area11, area12, area13, area14, area15, area16, area17, area18, area19, area20,
    }

    private List<Area> areas = Area.GetValues(typeof(Area)).Cast<Area>().ToList();
    private List<Area> combinedAreas;

    public List<Area> stackAreas;
    public List<Vector3> aoePositions;
    
    
    protected void Start()
    {
        //UpdateCombination();
        // InstantiateIndicators();
    }

    private void Update() {
        aC += 0.001f;
    }

    
    public void InstantiateIndicator(Vector3 pos, float duration = 0) {
        GameObject offsetObject = Instantiate(indicatorPrefab);
        offsetObject.transform.position = pos;
        offsetObject.transform.parent = transform;

        if(duration > 0) {
            StartCoroutine(DestoryIndicator(offsetObject, duration));
        }
    }

    public void InstantiateIndicators(float duration = 0) {
        foreach(var pos in aoePositions)
        {
            GameObject offsetObject = Instantiate(indicatorPrefab);
            offsetObject.transform.position = pos;
            offsetObject.transform.parent = transform;
        }
        
        if(duration > 0) {
            Invoke("DestoryIndicators", duration);
        }
    }

    private List<Area> GenerateRandomCombination(int size) {
        List<Area> combination = new List<Area>();

        while (combination.Count < size) {
            Area randomArea = areas[UnityEngine.Random.Range(0, areas.Count)];
            if (!combination.Contains(randomArea))
            {
                combination.Add(randomArea);
            }
        }

        return combination;
    }

    public void UpdateCombination() {
        combinedAreas = GenerateRandomCombination(aoeCount);
        aoePositions = GetAOEPositionsFromArea(combinedAreas);
    }

    private Vector3 GetPositionFromArea(Area area) {
        Vector3 distance = areaMax - areaMin;
        float unitX = distance.x / 10;
        float unitY = distance.y / 2;

        Debug.Log("Bounds min: "+areaMin+", max: "+areaMax);
        Debug.Log("Bounds distance: "+distance+", unitX: "+unitX+"unitY: "+unitY);

        Vector3 randomPos = Vector3.zero;
        switch(area) {
            case Area.area1:
                randomPos = new Vector3(Random.Range(areaMin.x, areaMin.x+unitX), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area2:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX, areaMin.x+unitX*2), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area3:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*2, areaMin.x+unitX*3), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area4:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*3, areaMin.x+unitX*4), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area5:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*4, areaMin.x+unitX*5), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area6:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*5, areaMin.x+unitX*6), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area7:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*6, areaMin.x+unitX*7), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area8:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*7, areaMin.x+unitX*8), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area9:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*8, areaMin.x+unitX*9), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area10:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*9, areaMax.x), Random.Range(areaMin.y, areaMin.y+unitY));
                break;
            case Area.area11:
                randomPos = new Vector3(Random.Range(areaMin.x, areaMin.x+unitX), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area12:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX, areaMin.x+unitX*2), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area13:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*2, areaMin.x+unitX*3), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area14:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*3, areaMin.x+unitX*4), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area15:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*4, areaMin.x+unitX*5), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area16:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*5, areaMin.x+unitX*6), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area17:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*6, areaMin.x+unitX*7), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area18:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*7, areaMin.x+unitX*8), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area19:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*8, areaMin.x+unitX*9), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
            case Area.area20:
                randomPos = new Vector3(Random.Range(areaMin.x+unitX*9, areaMax.x), Random.Range(areaMin.y+unitY, areaMax.y));
                break;
        }

        return randomPos;
    }

    public Vector3 GetAOEPositionByStackCombination() {
        Area randomArea = areas[UnityEngine.Random.Range(0, areas.Count)];
        while(stackAreas.Contains(randomArea))
        {
            randomArea = areas[UnityEngine.Random.Range(0, areas.Count)];
        }

        stackAreas.Add(randomArea);
        Vector3 randomPos = GetPositionFromArea(randomArea);
        return randomPos;
    }

    private List<Vector3> GetAOEPositionsFromArea(List<Area> combinations) {
        List<Vector3> result = new List<Vector3>();
        foreach(var combination in combinations) {
            Vector3 randomPos = GetPositionFromArea(combination);
            result.Add(randomPos);
        }

        return result;
    }

    protected IEnumerator DestoryIndicator(GameObject indicator, float duration) {
        yield return new WaitForSeconds(duration);
        Destroy(indicator);
        if(stackAreas.Any()) stackAreas.RemoveAt(0);
    }

    protected virtual void DestoryIndicators() {
        var indicators = GetComponentsInChildren<Transform>();
        foreach(var indicator in indicators) {
            if(indicator != transform) {
                Destroy(indicator.gameObject);
            }
        }

    }
    public Vector3 GetTrailedAOEPosition(Player target) {
        if(trailedPos.Equals(Vector3.zero)) {
            // 第一次的生成
            trailedPos = new Vector3(0, -3);
            Debug.Log("First trailedPos: "+trailedPos);
        } else {
            Vector3 lastTrailedPos = trailedPos;
            // 4. 根據trailDir與trailOffset與lastTrailedPos進行計算，得出新的位置
            
            float amplitudeY = 2.5f;      // Y 軸振幅
            float frequencyY = 16f;      // Y 軸頻率
            Debug.Log("Mathf.Sin: "+Mathf.Sin(aC * frequencyY));
            float yOffset = amplitudeY * Mathf.Sin(aC * frequencyY);
            Debug.Log("lastTrailedPos: "+lastTrailedPos);
            Debug.Log("offset: "+new Vector3(trailOffset * trailDir.x, yOffset, 0f));
            trailedPos = new Vector3(lastTrailedPos.x + (trailOffset * trailDir.x), -3 + yOffset);
            Debug.Log("trailedPos: "+trailedPos);

            // 5. 新的位置需確認是否在areaRange內，如果不在則須進行再計算得新的位置
            if(trailedPos.y < areaMin.y) {
                while(trailedPos.y < areaMin.y) {
                    trailedPos = new Vector3(trailedPos.x, trailedPos.y + 0.1f);
                }
            } else if(trailedPos.y > areaMax.y) {
                while(trailedPos.y > areaMax.y) {
                    trailedPos = new Vector3(trailedPos.x, trailedPos.y - 0.1f);
                }
            }

            if(trailedPos.x < areaMin.x) {
                while(trailedPos.x < areaMin.x) {
                    trailedPos = new Vector3(trailedPos.x + 0.1f, trailedPos.y);
                }
                trailDir = Vector3.right;
            } else if(trailedPos.x > areaMax.x) {
                while(trailedPos.x > areaMax.x) {
                    trailedPos = new Vector3(trailedPos.x - 0.1f, trailedPos.y);
                }
                trailDir = Vector3.left;
            } 
            

            Debug.Log("Non-First trailedPos: "+trailedPos);
        }           

        //trailDir = UpdateTrailDirection(trailedPos, target);
        Debug.Log("trailDir: "+trailDir);
        // 6. 上述都府和後變回傳
        return trailedPos;
    }
}
