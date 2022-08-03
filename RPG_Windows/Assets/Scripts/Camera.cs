using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float offset;
    [SerializeField] private float offsetSmoothing;
    private Vector3 cameraCenter;

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        var player = playerObj.GetComponent(typeof(Player)) as Player;
        cameraCenter = new Vector3(player.m_Center.x, player.m_Center.y, transform.position.z);

        if(offsetSmoothing > 0) {
            transform.position = Vector3.Lerp(transform.position, cameraCenter, offsetSmoothing * Time.deltaTime);
        } else {
            transform.position = new Vector3(cameraCenter.x, cameraCenter.y, transform.position.z);
        }
    }
}
