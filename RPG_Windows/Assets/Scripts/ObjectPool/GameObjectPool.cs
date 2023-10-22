using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] protected int amountToPool = 20;
    protected List<GameObject> pooledGameObjects = new List<GameObject>();

    protected virtual void Awake() {
        for (int i = 0; i < amountToPool; i++) {
            GameObject obj = new GameObject();
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledGameObjects.Add(obj);
        }
    }

    public virtual GameObject GetPooledGameObject(Vector3 initialPosition = default(Vector3)){
        for (int i = 0; i < pooledGameObjects.Count; i++) {
            if (!pooledGameObjects[i].activeInHierarchy) {
                if(initialPosition != null) pooledGameObjects[i].transform.position = initialPosition;
                return pooledGameObjects[i];
            }
        }

        return null;
    }
}
