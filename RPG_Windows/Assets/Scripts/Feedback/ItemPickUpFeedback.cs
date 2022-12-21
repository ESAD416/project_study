using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpFeedback : MonoBehaviour
{
    //public float pickUpRadius = 1f;
    public InventoryItemData itemData;
    private Collider2D selfCollider;

    private void Start()
    {
        selfCollider = GetComponent<Collider2D>();
        selfCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        var inventory = other.transform.GetComponent<InventoryHolder>();
        if(!inventory) return;

        if(inventory.InventorySystem.AddToInventory(itemData, 1)) {
            Destroy(this.gameObject);
        }
    }
}
