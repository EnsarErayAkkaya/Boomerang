using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string[] startingTexts;
    [SerializeField] private float textShowDelay;

    private SpawnPoint spawnPoint;
    private Tooltip tooltip;

    void Start()
    {
        spawnPoint = FindObjectOfType<SpawnPoint>();

        Transform player = spawnPoint.Spawn();

        var characterController = player.GetComponent<CharacterController>();

        tooltip = FindObjectOfType<Tooltip>();

        if (startingTexts.Length > 0)
        {
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
}
