using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Tooltip("The normal point above the player that the camera follows.")]
    public Vector3 targetOffset = new Vector3(0f, 2.05f, 0f);

    [Header("Normal Camera")]
    public float distance = 4f;
    public float normalFieldOfView = 60f;

    [Header("Aiming Camera")]
    public float aimDistance = 2.2f;

    [Tooltip("Moves the aiming camera over the player's shoulder. " +
             "Use a negative X value to switch shoulders.")]
    public Vector3 aimShoulderOffset =
        new Vector3(0.65f, 0.15f, 0f);

    [Tooltip("The point above the player used while aiming.")]
    public Vector3 aimTargetOffset =
        new Vector3(0f, 1.8f, 0f);

    public float aimFieldOfView = 48f;
    public float aimLookAheadDistance = 10f;
    public float aimBlendSpeed = 8f;
    public float aimSensitivityMultiplier = 0.75f;

    [Header("Camera Movement")]
    public float mouseSensitivity = 0.12f;
    public float minPitch = -35f;
    public float maxPitch = 70f;
    public float smoothTime = 0.05f;

    [Header("Cursor")]
    public bool lockCursorOnStart = true;

    public bool IsAiming { get; private set; }

    public float Yaw
    {
        get { return yaw; }
    }

    private Camera cameraComponent;

    private float yaw;
    private float pitch = 15f;
    private float aimBlend;

    private Vector3 currentVelocity;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    private void Start()
    {
        if (lockCursorOnStart)
        {
            LockCursor();
        }

        yaw = transform.eulerAngles.y;

        if (cameraComponent != null)
        {
            cameraComponent.fieldOfView = normalFieldOfView;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        HandleCursor();

        bool menuOpen =
            GameUIManager.Instance != null &&
            GameUIManager.Instance.MenuOpen;

        if (!menuOpen)
        {
            HandleMouseLook();
        }

        UpdateAimBlend();
        FollowTarget();
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float sensitivityMultiplier = Mathf.Lerp(
            1f,
            aimSensitivityMultiplier,
            aimBlend
        );

        yaw +=
            mouseDelta.x *
            mouseSensitivity *
            sensitivityMultiplier;

        pitch -=
            mouseDelta.y *
            mouseSensitivity *
            sensitivityMultiplier;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );
    }

    private void UpdateAimBlend()
    {
        float targetBlend = IsAiming ? 1f : 0f;

        aimBlend = Mathf.MoveTowards(
            aimBlend,
            targetBlend,
            aimBlendSpeed * Time.deltaTime
        );
    }

    private void FollowTarget()
    {
        Quaternion cameraRotation =
            Quaternion.Euler(pitch, yaw, 0f);

        Vector3 currentTargetOffset = Vector3.Lerp(
            targetOffset,
            aimTargetOffset,
            aimBlend
        );

        Vector3 currentShoulderOffset = Vector3.Lerp(
            Vector3.zero,
            aimShoulderOffset,
            aimBlend
        );

        float currentDistance = Mathf.Lerp(
            distance,
            aimDistance,
            aimBlend
        );

        Vector3 targetPosition =
            target.position + currentTargetOffset;

        Vector3 cameraOffset =
            currentShoulderOffset +
            Vector3.back * currentDistance;

        Vector3 desiredPosition =
            targetPosition +
            cameraRotation * cameraOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothTime
        );

        float currentLookAhead = Mathf.Lerp(
            0f,
            aimLookAheadDistance,
            aimBlend
        );

        Vector3 lookTarget =
            targetPosition +
            cameraRotation *
            Vector3.forward *
            currentLookAhead;

        Vector3 lookDirection =
            lookTarget - transform.position;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(
                lookDirection,
                Vector3.up
            );
        }

        if (cameraComponent != null)
        {
            cameraComponent.fieldOfView = Mathf.Lerp(
                normalFieldOfView,
                aimFieldOfView,
                aimBlend
            );
        }
    }

    public void SetAiming(bool aiming)
    {
        IsAiming = aiming;
    }

    private void HandleCursor()
    {
        if (Keyboard.current == null ||
            Mouse.current == null)
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
        if (GameUIManager.Instance == null ||
            !GameUIManager.Instance.MenuOpen)
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