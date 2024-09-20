using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject character;
    public GameObject boomerang;
    public GameObject destroyers;
    public Transform boomerangSpawnTransform;
    [SerializeField] private AudioSource audioSource;

    public int grabCount;


    public Transform Spawn()
    {
        var boomerangInstance = Instantiate(boomerang, 
            (boomerangSpawnTransform != null) ? boomerangSpawnTransform.position : transform.position, 
            Quaternion.identity).GetComponent<Boomerang>();
        boomerangInstance.SetUp(boomerangSpawnTransform != null);

        Instantiate(destroyers);

        SaveService.LoadGame();

        BoomerangController bc = Instantiate(character, transform.position, Quaternion.identity).GetComponent<BoomerangController>();

        if (boomerangSpawnTransform == null)
        {
            bc.Set(grabCount, boomerangInstance);
        }
        else
        {
            bc.Set(grabCount, null);
        }

        return bc.transform;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
