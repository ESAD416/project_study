using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlotModel
{
    [SerializeField] private InventoryItemData m_itemData;
    public InventoryItemData ItemData => m_itemData;
    [SerializeField] private int m_stackSize;
    public int StackSize => m_stackSize;
    

    public InventorySlotModel(InventoryItemData source, int amount) {
        m_itemData = source;
        m_stackSize = amount;
    }

    public InventorySlotModel() {
        ClearSlot();
    }

    public void ClearSlot() {
        m_itemData = null;
        m_stackSize = -1;
    }

    public void UpdateInventorySlot(InventoryItemData data, int amount) {
        m_itemData = data;
        m_stackSize = amount;
    }

    public void IncreaseToStack(int amount) {
        m_stackSize += amount;
    }

    public void ReduceFromStack(int amount) {
        m_stackSize -= amount;
    }

    public bool RoomLeftInStack(int amountToIncrease) {
        if(m_stackSize + amountToIncrease <= m_itemData.MaxStackSize) return true;
        return false;
    }

    public bool RoomLeftInStack(int amountToIncrease, out int amountRemaining) {
        amountRemaining = m_itemData.MaxStackSize - m_stackSize;
        return RoomLeftInStack(amountToIncrease);
    }
}
