using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string[] startingTexts;
    [SerializeField] private float textShowDelay;

    private SpawnPoint spawnPoint;
    private Tooltip tooltip;
    private Dictionary<Boomerang, Vector3> boomerangVelocities = new();

    public UnityEvent OnPlayerDead;

    void Start()
    {
        spawnPoint = FindObjectOfType<SpawnPoint>();

        Transform player = spawnPoint.Spawn();

        var characterController = player.GetComponent<CharacterController>();

        tooltip = FindObjectOfType<Tooltip>();

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (startingTexts.Length > 0 && (PlayerPrefs.GetInt(sceneName + "_story", 0) == 0))
        {
            PlayerPrefs.SetInt(sceneName + "_story", 1);

            characterController.DisableCharacter();
            characterController.BoomerangController.DisableController();

            DOVirtual.DelayedCall(textShowDelay, () =>
            {
                tooltip.Set(player.position, new Vector2(0, 150), startingTexts, () =>
                {
                    characterController.ActivateCharacter();
                    characterController.BoomerangController.ActivateController();
                });
            });
        }
    }

    public void SetPlayerDead()
    {
        OnPlayerDead?.Invoke();
    }

    public void StopAllGame()
    {
        var characterController = FindObjectOfType<CharacterController>();
        var enemyManager = FindObjectOfType<EnemyManager>();
        var boomerangs = FindObjectsOfType<Boomerang>();

        if (characterController != null)
        {
            characterController.DisableCharacter();
            characterController.BoomerangController.DisableController();
        }

        enemyManager.DeactivateEnemies();

        boomerangVelocities.Clear();

        foreach (var boomerang in boomerangs)
        {
            boomerang.Rigidbody.isKinematic = true; 
            boomerangVelocities.Add(boomerang, new Vector3(boomerang.Rigidbody.velocity.x, boomerang.Rigidbody.velocity.y, boomerang.Rigidbody.angularVelocity));
            boomerang.Rigidbody.velocity = Vector2.zero;
            boomerang.Rigidbody.angularVelocity = 0;
        }
    }

    public void StartAllGame()
    {
        var characterController = FindObjectOfType<CharacterController>();
        var enemyManager = FindObjectOfType<EnemyManager>();

        if (characterController != null)
        {
            characterController.ActivateCharacter();
            characterController.BoomerangController.ActivateController();
        }

        enemyManager.ActivateEnemies();

        foreach (var boomerang in boomerangVelocities)
        {
            boomerang.Key.Rigidbody.isKinematic = false;
            boomerang.Key.Rigidbody.velocity = new Vector2(boomerang.Value.x, boomerang.Value.y);
            boomerang.Key.Rigidbody.angularVelocity = boomerang.Value.z;
        }

        boomerangVelocities.Clear();
    }
}
