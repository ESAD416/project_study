using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlotModel
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;
    
    // Getter
    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    public InventorySlotModel(InventoryItemData source, int amount) {
        itemData = source;
        stackSize = amount;
    }

    public InventorySlotModel() {
        ClearSlot();
    }

    public void ClearSlot() {
        itemData = null;
        stackSize = -1;
    }

    public void UpdateInventorySlot(InventoryItemData data, int amount) {
        itemData = data;
        stackSize = amount;
    }

    public void IncreaseToStack(int amount) {
        stackSize += amount;
    }

    public void ReduceFromStack(int amount) {
        stackSize -= amount;
    }

    public bool RoomLeftInStack(int amountToIncrease) {
        if(stackSize + amountToIncrease <= itemData.maxStackSize) return true;
        return false;
    }

    public bool RoomLeftInStack(int amountToIncrease, out int amountRemaining) {
        amountRemaining = itemData.maxStackSize - stackSize;
        return RoomLeftInStack(amountToIncrease);
    }
}
