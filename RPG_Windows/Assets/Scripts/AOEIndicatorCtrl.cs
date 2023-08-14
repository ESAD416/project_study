using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class AOEIndicatorCtrl : MonoBehaviour
{
    public CinemachineVirtualCamera virtualMainCam;
    public Player player;
    public GameObject indicatorPrefab;
    public CompositeCollider2D areaRange;
    public Vector2 areaMin;
    public Vector2 areaMax;
    public float checkFrequency = 0.1f;
    public float offsetDistance = 1f;
    protected Camera activeCam;
    private Transform cameraTransform;


    private enum Area {
        area1,
        area2,
        area3,
        area4
    }

    private List<Area> areas = Area.GetValues(typeof(Area)).Cast<Area>().ToList();
    public List<Vector3> aoePositions;

    protected void Start()
    {
        activeCam = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;
        cameraTransform = activeCam.transform;
        //UpdateCombination();
        // InstantiateIndicators();
        StartCoroutine(UpdateIndicators());
    }
    public void InstantiateIndicator(Vector3 pos, float duration = 0) {
        GameObject offsetObject = Instantiate(indicatorPrefab);
        offsetObject.transform.position = pos;
        offsetObject.transform.parent = transform;

        if(duration > 0) {
            Invoke("DestoryIndicators", duration);
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


    public void UpdateCombination() {
        List<Area> combinedDir = GenerateRandomCombination(areas.Count);
        aoePositions = GetAOEPositionsFromArea(combinedDir);
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

    private List<Vector3> GetAOEPositionsFromArea(List<Area> combinations) {
        Vector3 distance = areaMax - areaMin;
        Vector3 unitD = distance / 10;

        Debug.Log("Bounds min: "+areaMin+", max: "+areaMax);
        Debug.Log("Bounds distance: "+distance+", unitD: "+unitD);

        List<Vector3> result = new List<Vector3>();
        Vector3 randomPos;
        foreach(var combination in combinations) {
            switch(combination) {
                case Area.area1:
                    randomPos = new Vector3(Random.Range(areaMin.x, areaMin.x+unitD.x), Random.Range(areaMin.y, areaMin.y+unitD.y));
                    result.Add(randomPos);
                    break;
                case Area.area2:
                    randomPos = new Vector3(Random.Range(areaMin.x+unitD.x*3, areaMin.x+unitD.x*4), Random.Range(areaMin.y+unitD.y*3, areaMin.y+unitD.y*4));
                    result.Add(randomPos);
                    break;
                case Area.area3:
                    randomPos = new Vector3(Random.Range(areaMin.x+unitD.x*6, areaMin.x+unitD.x*7), Random.Range(areaMin.y+unitD.y*6, areaMin.y+unitD.y*7));
                    result.Add(randomPos);
                    break;
                case Area.area4:
                    randomPos = new Vector3(Random.Range(areaMin.x+unitD.x*9, areaMax.x), Random.Range(areaMin.y+unitD.y*9, areaMax.y));
                    result.Add(randomPos);
                    break;
            }
        }

        return result;
    }

    protected virtual void UpdatePosition() {
        activeCam = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;

    }

    protected virtual IEnumerator UpdateIndicators() {
        while(true) {
            foreach(var pos in aoePositions) {
                UpdatePosition();
            }

            yield return new WaitForSeconds(checkFrequency);  
        }
    }

    protected virtual void DestoryIndicators() {
        var indicators = GetComponentsInChildren<Transform>();
        foreach(var indicator in indicators) {
            if(indicator != transform) {
                Destroy(indicator.gameObject);
            }
        }

    }
}
