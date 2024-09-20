
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangCollectable : Collectable
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDesc;
    public override void OnCollect()
    {
        SaveService.saveData.boomerangCount++;
        SaveService.SaveGame();

        FindObjectOfType<NewItemCollectUI>().Show(itemName, itemDesc);
    }
}
