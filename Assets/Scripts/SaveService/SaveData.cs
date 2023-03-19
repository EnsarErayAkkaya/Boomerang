using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public partial class SaveData
{
    public int currentLevel;
    public int boomerangCount;
    public int boomerang = 0;
    public SaveData()
    {
        currentLevel = 0;
        boomerang = 0;
        boomerangCount = 1;
    }
}
