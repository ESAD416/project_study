using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HitEffectPool : GameObjectPool
{
    public static HitEffectPool instance;
    [SerializeField] private GameObject gameObjectPrefab;

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

    public IEnumerator DestroyHitEffectObjects(GameObject obj) {
        var duration = 0.5f;
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
    }
}
