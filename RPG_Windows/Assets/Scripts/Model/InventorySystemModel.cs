using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[Serializable]
public class InventorySystemModel
{
    [SerializeField] private List<InventorySlotModel> m_inventorySlots;
    // Getter
    public List<InventorySlotModel> InventorySlots => m_inventorySlots;
    public int InventorySize => m_inventorySlots.Count;

    public UnityAction<InventorySlotModel> OnInventorySlotChanged;

    public InventorySystemModel(int size) {
        m_inventorySlots = new List<InventorySlotModel>(size);

        for(int i = 0; i < size; i++) {
            m_inventorySlots.Add(new InventorySlotModel());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd) {
        if(ContainsItem(itemToAdd, out List<InventorySlotModel> invSlots)) {  // Check whether item exists in inventory
            foreach(var slot in invSlots) {
                if(slot.RoomLeftInStack(amountToAdd)) {
                    slot.IncreaseToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        } 
        
        if(HasFreeSlot(out InventorySlotModel freeSlot)) { // Check the first available slot
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }

        // inventorySlots.Add(new InventorySlotModel(itemToAdd, amountToAdd));
        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlotModel> invSlots) {
        invSlots = m_inventorySlots.Where(i => i.ItemData != null && i.ItemData.ID == itemToAdd.ID).ToList();

        return invSlots == null ? false : invSlots.Count >= 1 ? true : false;
    }
    
    public bool HasFreeSlot(out InventorySlotModel freeSlot) {
        freeSlot = m_inventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
