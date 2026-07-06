using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector3 targetOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Camera")]
    public float distance = 4f;
    public float mouseSensitivity = 0.12f;
    public float minPitch = -35f;
    public float maxPitch = 70f;
    public float smoothTime = 0.05f;

    [Header("Cursor")]
    public bool lockCursorOnStart = true;

    private float yaw;
    private float pitch = 15f;

    private Vector3 currentVelocity;

    private void Start()
    {
        if (lockCursorOnStart)
        {
            LockCursor();
        }

        yaw = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        HandleCursor();
        HandleMouseLook();
        FollowTarget();
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void FollowTarget()
    {
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = target.position + targetOffset;
        Vector3 desiredPosition = targetPosition + cameraRotation * new Vector3(0f, 0f, -distance);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothTime
        );

        transform.LookAt(targetPosition);
    }

    private void HandleCursor()
    {
        if (Keyboard.current == null || Mouse.current == null)
        {
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            LockCursor();
        }
    }

    private void LockCursor()
    {
        if (GameUIManager.Instance == null || !GameUIManager.Instance.MenuOpen)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}