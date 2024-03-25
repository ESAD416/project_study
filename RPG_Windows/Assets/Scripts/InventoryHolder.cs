using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int m_inventorySize;
    [SerializeField] protected InventorySystemModel m_inventorySystem;

    //Getter
    public InventorySystemModel InventorySystem => m_inventorySystem;

    public static UnityAction<InventorySystemModel> OnDynamicInventoryDisplayRequested;

    private void Start() {
        m_inventorySystem = new InventorySystemModel(m_inventorySize);
    }
}
