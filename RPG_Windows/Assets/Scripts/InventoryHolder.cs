using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystemModel inventorySystem;

    //Getter
    public InventorySystemModel InventorySystem => inventorySystem;

    public static UnityAction<InventorySystemModel> OnDynamicInventoryDisplayRequested;

    private void Awake() {
        inventorySystem = new InventorySystemModel(inventorySize);
    }
}
