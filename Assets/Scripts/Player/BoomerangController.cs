using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    [SerializeField] private BoomerangData boomerangData;
    [SerializeField] private GrabCountUI grabCountUI;

    private int speedMultiplier = 1;
    private Boomerang boomerang;
    public CharacterController characterController;
    private Vector2 dir;

    private Camera camera;

    public int grabCount;
    public bool hasBumerang = true;

    private void Start()
    {
        camera = Camera.main;
    }

    public void Set(int grabCount, Boomerang boomerang)
    {
        this.grabCount = grabCount;

        this.boomerang = boomerang;
        boomerang.SetBoomerang(boomerangData.boomerangDetails[SaveService.saveData.boomerang]);

        grabCountUI.SetGrabCount(this.grabCount);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hasBumerang)
            {
                ThrowBoomerang();
            }
            else if (grabCount > 0 && !hasBumerang)
            {
                PullBoomerang();
            }
        }

        if (hasBumerang)
        {
            SetDir((Vector2)transform.position, (Vector2)camera.ScreenToWorldPoint(Input.mousePosition));
            boomerang.SetBoomerangPosBeforeShooting(characterController.BoxCollider.bounds.center, dir);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            speedMultiplier++;
            if (speedMultiplier > 3)
                speedMultiplier = 1;
            boomerang.SetSpeed(speedMultiplier);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (++SaveService.saveData.boomerang > SaveService.saveData.boomerangCount)
            {
                SaveService.saveData.boomerang = 0;
            }

            if (hasBumerang)
            {
                boomerang.SetBoomerang(boomerangData.boomerangDetails[SaveService.saveData.boomerang]);
            }

            Debug.Log("Current Boomerang: " + SaveService.saveData.boomerang);

            SaveService.SaveGame();
        }
    }
    private void SetDir(Vector2 from, Vector2 to)
    {
        dir = (to - from).normalized;
    }
    public void ThrowBoomerang()
    {
        hasBumerang = false;

        boomerang.Collider.enabled = true;

        boomerang.ThrowBoomerang(dir);
    }
    public void PullBoomerang()
    {
        SetDir(boomerang.transform.position, transform.position);
        boomerang.ThrowBoomerang(dir);
    }
    public void GrabBoomerang()
    {
        if (!hasBumerang && grabCount > 0)
        {
            boomerang.SetBoomerang(boomerangData.boomerangDetails[SaveService.saveData.boomerang]);

            boomerang.Collider.enabled = false;

            hasBumerang = true;
            grabCount--;
            grabCountUI.SetGrabCount(grabCount);
        }
        else if(grabCount <= 0)
        {
            // die
            characterController.Die("Boomerang");
        }
    }
}
