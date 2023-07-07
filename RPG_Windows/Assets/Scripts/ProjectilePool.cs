using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool instance;

    private List<GameObject> pooledProjectiles = new List<GameObject>();
    private int amountToPool = 20;

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

    public GameObject GetPooledProjectile() {
        for (int i = 0; i < pooledProjectiles.Count; i++) {
            if (!pooledProjectiles[i].activeInHierarchy) {
                return pooledProjectiles[i];
            }
        }

        return null;
    }
}
