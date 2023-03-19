using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoomerangDetail
{
    public Sprite boomerangSprite;
    public float speed;
    public Vector3 scale;
}

[CreateAssetMenu(menuName = "Scriptable Objects/Boomerang Data", fileName = "BoomerangData")]
public class BoomerangData : ScriptableObject
{
    public BoomerangDetail[] boomerangDetails;
}
