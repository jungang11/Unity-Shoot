using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    // 1인칭 시점 카메라 설정
    
    [SerializeField] Transform cameraRoot;
    [SerializeField] float mouseSensitivity;

    private Vector2 lookDelta;
    private float xRotation;
    private float yRotation;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;
        xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
        // Clamp를 이용해 x회전 범위 제한 ( -80f ~ 80f )
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // 위아래는 카메라만
        cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0); 
        // 좌우는 캐릭터만
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
    
    // pointer - delta 를 이용해 Vector2 값을 받아옴
    private void OnLook(InputValue value)
    {
        lookDelta = value.Get<Vector2>();
    }
}
