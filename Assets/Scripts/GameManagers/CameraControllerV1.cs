using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraControllerV1 : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 followOffset;
    [SerializeField] private float speed;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    private Vector2 screenRect;
    private CharacterController characterController;
    private Vector3 threshold;

    private void Start()
    {
        characterController = FindObjectOfType<CharacterController>();

        threshold = CalculateThreshold();

        screenRect = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0));
    }

    private void FixedUpdate()
    {
        if (characterController != null)
        {
            Vector2 follow = characterController.transform.position;

            float xDifference = (transform.position.x - follow.x);
            float yDifference = (transform.position.y - follow.y);

            Vector3 newPos = transform.position;

            if (Mathf.Abs(xDifference) >= threshold.x)
            {
                newPos.x = follow.x;
            }

            if (Mathf.Abs(yDifference) >= threshold.y)
            {
                newPos.y = follow.y;
            }

            if (newPos.x < minX)
            {
                newPos.x = minX;
            }
            
            if (newPos.x > maxX)
            {
                newPos.x = maxX;
            }

            transform.position = Vector3.MoveTowards(transform.position, newPos, speed * Time.deltaTime);
        }
        else
        {
            characterController = FindObjectOfType<CharacterController>();
        }
    }

    private Vector3 CalculateThreshold()
    {
        Rect aspect = _camera.pixelRect;

        Vector2 t = new Vector2(_camera.orthographicSize * aspect.width / aspect.height, _camera.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;

        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector2 border = CalculateThreshold();

        Gizmos.DrawWireCube(_camera.transform.position, new Vector3(border.x * 2, border.y, 1));

    }
}
