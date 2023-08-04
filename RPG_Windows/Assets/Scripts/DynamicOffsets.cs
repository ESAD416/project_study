using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class DynamicOffsets : MonoBehaviour
{
    public CinemachineVirtualCamera virtualMainCam;
    public int offsetCount = 7;
    public float offsetDistance = 5f;

    private Camera activeCam;
    private Transform cameraTransform;
    private List<float> quadrants;
    private enum Direction {
        Quadrant1,
        Quadrant2,
        Quadrant3,
        Quadrant4,
        Quadrant5,
        Quadrant6,
        Quadrant7,
        Quadrant8
    }
    private List<Direction> directions = Direction.GetValues(typeof(Direction)).Cast<Direction>().ToList();

    public  List<Transform> offsets;

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
        UpdateOffsets();
    }

    private Transform CreateOffset()
    {
        GameObject offsetObject = new GameObject("Offset");
        offsetObject.transform.parent = transform;
        
        return offsetObject.transform;
    }

    public void UpdateOffsets() {
        if(!quadrants.Any()) {
            return;
        }

        for (int i = 0; i < offsetCount; i++)
        {
            var combinedQuadrant = quadrants[i];
            UpdateOffsetPosition(offsets[i], combinedQuadrant);
        }
    }

    public void UpdateCombination() {
        List<Direction> combinedDir = GenerateRandomCombination(offsetCount);
        quadrants = GetQuadrantFromDir(combinedDir);
    }

    private List<Direction> GenerateRandomCombination(int size) {
        List<Direction> combination = new List<Direction>();

        while (combination.Count < size) {
            Direction randomDir = directions[UnityEngine.Random.Range(0, directions.Count)];
            if (!combination.Contains(randomDir))
            {
                combination.Add(randomDir);
            }
        }

        return combination;
    }

    private List<float> GetQuadrantFromDir(List<Direction> combinations) {
        List<float> result = new List<float>();
        float randomDegrees;

        foreach(var combination in combinations) {
            switch(combination) {
                case Direction.Quadrant1:
                    randomDegrees = UnityEngine.Random.Range(0, 45);
                    result.Add(randomDegrees);
                    break;
                case Direction.Quadrant2:
                    randomDegrees = UnityEngine.Random.Range(45, 90);
                    result.Add(randomDegrees);
                    break;
                case Direction.Quadrant3:
                    randomDegrees = UnityEngine.Random.Range(90, 135);
                    result.Add(randomDegrees);
                    break;
                case Direction.Quadrant4:
                    randomDegrees = UnityEngine.Random.Range(135, 180);
                    result.Add(randomDegrees);
                    break;
                case Direction.Quadrant5:
                    randomDegrees = UnityEngine.Random.Range(180, 225);
                    result.Add(randomDegrees);
                    break;
                case Direction.Quadrant6:
                    randomDegrees = UnityEngine.Random.Range(225, 270);
                    result.Add(randomDegrees);
                    break;
                case Direction.Quadrant7:
                    randomDegrees = UnityEngine.Random.Range(270, 315);
                    result.Add(randomDegrees);
                    break;
                case Direction.Quadrant8:
                    randomDegrees = UnityEngine.Random.Range(-45, 0);
                    result.Add(randomDegrees);
                    break;
            }
        }

        return result;
    }

    private Vector2 GetVectorFromDegrees(float angleInDegrees) {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
    }

    private void UpdateOffsetPosition(Transform offset, float randomDegrees) {
        //Debug.Log("UpdateOffsetPosition randomDegrees: "+randomDegrees); 
        Vector2 dir = GetVectorFromDegrees(randomDegrees);

        
        Vector3 offsetPosition = GetOffsetPositionOutOfView(cameraTransform.position, (Vector3)dir);

        //Debug.Log("UpdateOffsetPosition offsetPosition: "+offsetPosition);
        // offsetPosition.y = cameraTransform.position.y;
        offsetPosition.z = 0;
        offset.position = offsetPosition;

        // // 檢查Offset是否位於鏡頭的可視範圍內
        // Vector3 viewportPos = activeCam.WorldToViewportPoint(offset.position);
        // if (viewportPos.x > 0f && viewportPos.x < 1f && viewportPos.y > 0f && viewportPos.y < 1f && viewportPos.z > 0f)
        // {
        //     Vector3 direction = offset.position - cameraTransform.position;
        //     direction.Normalize();
        //     offset.position = cameraTransform.position + direction * minDistanceFromCamera;
        // }
    }

    private Vector3 GetOffsetPositionOutOfView(Vector3 cameraPosition, Vector3 dir) {
        Vector3 topRight = activeCam.ViewportToWorldPoint(new Vector3(1, 1, activeCam.nearClipPlane));
        Vector3 diagonalDir = (topRight - cameraTransform.position).normalized;
        Vector2 increment = diagonalDir * offsetDistance;
        
        Vector3 offsetPosition = cameraPosition + Vector3.Scale(dir, increment);

        Vector3 viewportPos = activeCam.WorldToViewportPoint(offsetPosition);
        while(viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1) {
            offsetPosition += Vector3.Scale(dir, increment);
            viewportPos = activeCam.WorldToViewportPoint(offsetPosition);
        }

        return offsetPosition;
    }
}
