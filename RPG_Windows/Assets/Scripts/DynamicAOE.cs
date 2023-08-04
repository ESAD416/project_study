using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicAOE : MonoBehaviour
{
    public CinemachineVirtualCamera virtualMainCam;
    public CompositeCollider2D areaRange;
    public int offsetCount = 4;
    public int randomOffsetCount = 2;
    public float offsetDistance = 1f;

    private Camera activeCam;
    private Transform cameraTransform;
    
    private List<Vector3> aoePositions;

    private enum Area {
        area1,
        area2,
        area3,
        area4
    }

    private List<Area> areas = Area.GetValues(typeof(Area)).Cast<Area>().ToList();
    public  List<Transform> offsets;

    // Start is called before the first frame update
    void Start()
    {
        activeCam = CinemachineCore.Instance.GetActiveBrain(0).OutputCamera;
        cameraTransform = activeCam.transform;
        offsets = new List<Transform>();
        for (int i = 0; i < offsetCount; i++)
        {
            offsets.Add(CreateOffset());
        }

        UpdateCombination();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAOEPosition();
    }

    private Transform CreateOffset()
    {
        GameObject offsetObject = new GameObject("Offset");
        offsetObject.transform.parent = transform;
        
        return offsetObject.transform;
    }

    public void UpdateAOEPosition() {
        if(!aoePositions.Any()) {
            return;
        }

        for (int i = 0; i < offsetCount; i++)
        {
            var combinedAOE = aoePositions[i];
            offsets[i].position = combinedAOE;
        }
    }

    public void UpdateCombination() {
        List<Area> combinedDir = GenerateRandomCombination(offsetCount);
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
        Vector3 min = new Vector3(areaRange.bounds.min.x >= 0 ? areaRange.bounds.min.x-offsetDistance : areaRange.bounds.min.x+offsetDistance, 
                                  areaRange.bounds.min.y >= 0 ? areaRange.bounds.min.y-offsetDistance : areaRange.bounds.min.y+offsetDistance);
        Vector3 max = new Vector3(areaRange.bounds.max.x >= 0 ? areaRange.bounds.max.x-offsetDistance : areaRange.bounds.max.x+offsetDistance,
                                  areaRange.bounds.max.y >= 0 ? areaRange.bounds.max.y-offsetDistance : areaRange.bounds.max.y+offsetDistance);
        Vector3 distance = max - min;
        Vector3 unitD = distance / 4;

        Debug.Log("Bounds distance: "+distance+", unitD: "+distance / 4);

        List<Vector3> result = new List<Vector3>();
        Vector3 randomPos;

        foreach(var combination in combinations) {
            switch(combination) {
                case Area.area1:
                    randomPos = new Vector3(Random.Range(min.x, min.x+unitD.x), Random.Range(min.y, min.y+unitD.y));
                    result.Add(randomPos);
                    break;
                case Area.area2:
                    randomPos = new Vector3(Random.Range(min.x+unitD.x, min.x+unitD.x*2), Random.Range(min.x+unitD.y, min.y+unitD.y*2));
                    result.Add(randomPos);
                    break;
                case Area.area3:
                    randomPos = new Vector3(Random.Range(min.x+unitD.x*2, min.x+unitD.x*3), Random.Range(min.y+unitD.y*2, min.y+unitD.y*3));
                    result.Add(randomPos);
                    break;
                case Area.area4:
                    randomPos = new Vector3(Random.Range(min.x+unitD.x*3, max.x), Random.Range(min.y+unitD.y*3, max.y));
                    result.Add(randomPos);
                    break;
            }
        }

        return result;
    }
}
