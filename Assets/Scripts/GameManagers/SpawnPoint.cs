using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject character;
    public GameObject boomerang;
    public GameObject destroyers;

    public int grabCount;

    public Transform Spawn()
    {
        var boomerangInstance = Instantiate(boomerang, transform.position, Quaternion.identity).GetComponent<Boomerang>();

        Instantiate(destroyers);

        SaveService.LoadGame();

        BoomerangController bc = Instantiate(character, transform.position, Quaternion.identity).GetComponent<BoomerangController>();
        bc.Set(grabCount, boomerangInstance);

        return bc.transform;
    }

}
