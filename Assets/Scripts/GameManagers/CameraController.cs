using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 cameraOffset;

    private Vector3 maxRight;
    private CharacterController characterController;
    private float dist;

    private void Start()
    {
        characterController = FindObjectOfType<CharacterController>();
        maxRight = _camera.ViewportToWorldPoint(new Vector3(1, 0, 0));
        dist = maxRight.x;
    }

    private void FixedUpdate()
    {
        if (characterController != null)
        {
            int division = (int)characterController.transform.position.x / (int)dist;

            float targetX = division * dist * 2;

            transform.position = cameraOffset + new Vector3(Mathf.Lerp(transform.position.x, targetX, speed * Time.deltaTime), 0, 0);
        }
    }
}
