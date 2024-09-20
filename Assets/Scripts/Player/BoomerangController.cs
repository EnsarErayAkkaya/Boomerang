using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoomerangController : MonoBehaviour
{
    [SerializeField] private BoomerangData boomerangData;
    [SerializeField] private GrabCountUI grabCountUI;

    [Header("Sounds")]
    [SerializeField] private AudioClip boomerangGrabSfx;

    private int speedMultiplier = 1;
    private Boomerang boomerang = null;
    private CharacterController characterController;
    private Vector2 dir;

    private new Camera camera;
    private bool controllerDisabled = false;
    private int grabCount;
    private bool hasBumerang = false;

    public bool HasBumerang { get => hasBumerang; set => hasBumerang = value; }
    public int GrabCount { get => grabCount; set => grabCount = value; }
    public CharacterController CharacterController { get => characterController; }
    public Boomerang Boomerang { get => boomerang;}

    private void Start()
    {
        camera = Camera.main;
        characterController = gameObject.GetComponent<CharacterController>();
    }

    public void Set(int grabCount, Boomerang boomerang)
    {
        this.GrabCount = grabCount;

        if (boomerang != null)
        {
            HasBumerang = true;
            this.boomerang = boomerang;
            boomerang.SetBoomerang(boomerangData.boomerangDetails[SaveService.saveData.boomerang]);
        }
        else
        {
            HasBumerang = false;
        }

        grabCountUI.SetGrabCount(this.GrabCount);
        grabCountUI.Show();
    }
    void Update()
    {
        if (HasBumerang)
        {
            SetDir((Vector2)transform.position, (Vector2)camera.ScreenToWorldPoint(Input.mousePosition));
            Boomerang.SetBoomerangPosBeforeShooting(CharacterController.BoxCollider.bounds.center, dir);
        }

        if (!controllerDisabled)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (HasBumerang)
                {
                    ThrowBoomerang();
                }
                else if (GrabCount > 0 && !HasBumerang)
                {
                    PullBoomerang();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                speedMultiplier++;
                if (speedMultiplier > 3)
                    speedMultiplier = 1;
                Boomerang.SetSpeed(speedMultiplier);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (++SaveService.saveData.boomerang >= SaveService.saveData.boomerangCount)
                {
                    SaveService.saveData.boomerang = 0;
                }

                if (HasBumerang)
                {
                    Boomerang.SetBoomerang(boomerangData.boomerangDetails[SaveService.saveData.boomerang]);
                }

                Debug.Log("Current Boomerang: " + SaveService.saveData.boomerang);

                SaveService.SaveGame();
            }
        }
    }
    private void SetDir(Vector2 from, Vector2 to)
    {
        dir = (to - from).normalized;
    }
    public void ThrowBoomerang()
    {
        if (Boomerang != null)
        {
            HasBumerang = false;

            Boomerang.Collider.enabled = true;

            Boomerang.ThrowBoomerang(dir);
        }
    }
    public void PullBoomerang()
    {
        if (Boomerang != null)
        {
            SetDir(Boomerang.transform.position, transform.position);
            Boomerang.ThrowBoomerang(dir);
        }
    }
    public void GrabBoomerang()
    {
        characterController.OnGrabBoomerang();

        if (!HasBumerang && GrabCount > 0)
        {
            Boomerang.SetBoomerang(boomerangData.boomerangDetails[SaveService.saveData.boomerang]);

            Boomerang.Collider.enabled = false;

            HasBumerang = true;
            GrabCount--;
            grabCountUI.SetGrabCount(GrabCount);

            characterController.PlaySound(boomerangGrabSfx);
        }
        else if(GrabCount <= 0)
        {
            // die
            CharacterController.Die("Boomerang");
        }
    }

    public void DisableController()
    {
        controllerDisabled = true;
    }

    public void ActivateController()
    {
        controllerDisabled = false;
    }

    public void SetBoomerang(Boomerang boomerang)
    {
        this.boomerang = boomerang;
    }
}
