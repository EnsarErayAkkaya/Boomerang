using RR.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangCollectable : Collectable
{
    public override void OnCollect()
    {
        SaveService.saveData.boomerangCount++;
        SaveService.SaveGame();
    }
}
