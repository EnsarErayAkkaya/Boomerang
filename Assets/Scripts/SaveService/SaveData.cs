using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RR.Services
{
    [System.Serializable]
    public partial class SaveData
    {
        public int boomerangCount;
        public int boomerang = 0;
        public SaveData()
        {
            boomerang = 0;
            boomerangCount = 1;
        }
    }
}