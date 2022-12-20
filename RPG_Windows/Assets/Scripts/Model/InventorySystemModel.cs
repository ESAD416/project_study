using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystemModel
{
    [SerializeField] private List<InventorySlotModel> inventorySlots;

    // Getter
    public List<InventorySlotModel> InventorySlots => inventorySlots;
    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlotModel> OnInventorySlotChanged;

    public InventorySystemModel(int size) {
        inventorySlots = new List<InventorySlotModel>(size);

        for(int i = 0; i < size; i++) {
            inventorySlots.Add(new InventorySlotModel());
        }
    }
}
