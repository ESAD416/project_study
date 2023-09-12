using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;
    [SerializeField] private int amountToPool = 20;

    private List<GameObject> pooledProjectiles = new List<GameObject>();

    [SerializeField] private GameObject projectilePrefab;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    private void Start() {
        for (int i = 0; i < amountToPool; i++) {
            GameObject obj = Instantiate(projectilePrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledProjectiles.Add(obj);
        }
    }

    public GameObject GetPooledProjectile(Vector3 initialPosition = default(Vector3)){
        for (int i = 0; i < pooledProjectiles.Count; i++) {
            if (!pooledProjectiles[i].activeInHierarchy) {
                if(initialPosition != null) pooledProjectiles[i].transform.position = initialPosition;
                return pooledProjectiles[i];
            }
        }

        return null;
    }
}
