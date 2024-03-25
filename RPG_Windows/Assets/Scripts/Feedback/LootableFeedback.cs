using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableFeedback : MonoBehaviour
{
    [SerializeField] private GameObject lootPrefeb;
    [SerializeField] private List<LootData> lootList = new List<LootData>();
    [Range(1, 5)]
    [SerializeField] private int dropLootCount;
    private float spread = 2f;

    public void InstantiateLoot() {
        for(int i = 0; i < dropLootCount; i++) {
            LootData lootItem = GetDroppedLoot();
            if(!lootItem) {
                continue;
            }

            Vector3 pos = transform.position;
            pos.x += spread * Random.value - spread / 2;
            pos.y += spread * Random.value - spread / 2;

            GameObject dropLoot = Instantiate(lootPrefeb);
            dropLoot.GetComponent<SpriteRenderer>().sprite = lootItem.LootSprite;
            dropLoot.GetComponent<ItemPickUpFeedback>().itemData = lootItem;
            dropLoot.transform.position = pos;
        }
    }

    private LootData GetDroppedLoot() {
        int randomChance = Random.Range(1, 101);     // 1 - 100
        List<LootData> possibleLoot = GetPossibleLoot(randomChance);
        if(possibleLoot.Count > 0 ) {
            LootData loot = possibleLoot[Random.Range(0, possibleLoot.Count)];
            return loot;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    private List<LootData> GetPossibleLoot(int randomChance) {
        List<LootData> possibleLoot = new List<LootData>();
        foreach(LootData loot in lootList) {
            if(randomChance <= loot.DropChance) {
                possibleLoot.Add(loot);
            }
        }
        return possibleLoot;
    }
}
