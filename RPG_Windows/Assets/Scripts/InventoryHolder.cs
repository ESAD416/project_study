using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] protected Transform m_target;
    [SerializeField] private int m_inventorySize;
    [SerializeField] protected InventorySystemModel m_inventorySystem;
    protected BoxCollider2D m_targetHitBoxCollider;

    //Getter
    public InventorySystemModel InventorySystem => m_inventorySystem;

    public static UnityAction<InventorySystemModel> OnDynamicInventoryDisplayRequested;

    private void Awake() {
    }

    private void Start() {
        m_inventorySystem = new InventorySystemModel(m_inventorySize);
        m_targetHitBoxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update() 
    {
    }
}
