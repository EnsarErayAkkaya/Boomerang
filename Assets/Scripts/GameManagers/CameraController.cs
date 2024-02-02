using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 cameraOffset;

    private Vector3 threshold;
    private Vector3 maxRight;
    private CharacterController characterController;
    private Vector2 halfRect;

    private void Start()
    {
        characterController = FindObjectOfType<CharacterController>();
        maxRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        halfRect = maxRight;
    }

    private void FixedUpdate()
    {
        if (characterController != null)
        {
            Vector2 charPos = characterController.transform.position;

            charPos.x += charPos.x > 0 ? halfRect.x : -halfRect.x;
            charPos.y += charPos.y > 0 ? halfRect.y : -halfRect.y;

            int xDivision = (int)(charPos.x / (halfRect.x * 2));
            int yDivision = (int)(charPos.y / (halfRect.y * 2));

            Vector3 target = new Vector3((xDivision * halfRect.x * 2),
                (yDivision * halfRect.y * 2));

            transform.localPosition = Vector3.Lerp(transform.localPosition, target, speed * Time.deltaTime);
        }
    }
}
