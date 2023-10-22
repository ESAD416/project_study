using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : GameObjectPool
{
    public static ProjectilePool instance;
    [SerializeField] private GameObject gameObjectPrefab;
    // Start is called before the first frame update

    protected override void Awake() {
        if (instance == null) {
            instance = this;
        }

        for (int i = 0; i < amountToPool; i++) {
            GameObject obj = Instantiate(gameObjectPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledGameObjects.Add(obj);
        }
    }

}
