using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpFeedback : MonoBehaviour
{
    //public float pickUpRadius = 1f;
    public InventoryItemData itemData;
    public int itemAmount = 1;
    private Collider2D selfCollider;

    private void Start()
    {
        selfCollider = GetComponent<Collider2D>();
        selfCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var inventory = other.transform.GetComponent<InventoryHolder>();
        if(!inventory) return;

        if(inventory.InventorySystem.AddToInventory(itemData, itemAmount)) {
            Destroy(this.gameObject);
        }
    }
}
