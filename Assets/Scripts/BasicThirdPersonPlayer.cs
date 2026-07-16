using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class BasicThirdPersonPlayer : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;
    public PlayerBowController bowController;

    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float crouchSpeed = 2f;
    public float turnSmoothTime = 0.08f;

    [Header("Bow Aiming")]
    [Tooltip("How quickly the player turns toward the camera while aiming.")]
    public float aimTurnSmoothTime = 0.04f;

    [Tooltip(
        "Adjust this if the player faces sideways or backward while aiming."
    )]
    public float aimYawOffset = 0f;

    [Header("Jumping / Gravity")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Crouching")]
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 12f;
    public LayerMask ceilingCheckLayers = ~0;

    [Header("Animation")]
    public float animationDampTime = 0.1f;

    private CharacterController characterController;

    private float verticalVelocity;
    private float turnSmoothVelocity;

    private float standingHeight;
    private Vector3 standingCenter;
    private Vector3 crouchingCenter;

    private bool isCrouching;
    private bool isRunning;
    private bool isJumping;
    private bool isFreeFalling;

    private Vector2 currentMoveInput;
    private float currentMoveSpeed;

    private void Awake()
    {
        characterController =
            GetComponent<CharacterController>();

        standingHeight =
            characterController.height;

        standingCenter =
            characterController.center;

        crouchingCenter = standingCenter;

        crouchingCenter.y -=
            (standingHeight - crouchHeight) / 2f;

        if (cameraTransform == null &&
            Camera.main != null)
        {
            cameraTransform =
                Camera.main.transform;
        }

        if (animator == null)
        {
            animator =
                GetComponentInChildren<Animator>();
        }

        if (bowController == null)
        {
            bowController =
                GetComponent<PlayerBowController>();
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (GameUIManager.Instance != null &&
            GameUIManager.Instance.MenuOpen)
        {
            currentMoveInput = Vector2.zero;
            currentMoveSpeed = 0f;
            isRunning = false;

            UpdateAnimator();
            return;
        }

        HandleCrouch();
        HandleMovement();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        currentMoveInput = GetMoveInput();

        bool isGrounded =
            characterController.isGrounded;

        if (isGrounded &&
            verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        bool isBowAiming =
            bowController != null &&
            bowController.IsAiming;

        float cameraYaw =
            GetCameraYaw();

        isRunning =
            Keyboard.current.leftShiftKey.isPressed &&
            !isCrouching &&
            !isBowAiming &&
            currentMoveInput.sqrMagnitude > 0.01f;

        currentMoveSpeed = 0f;

        // While aiming, the player faces the camera's
        // horizontal forward direction.
        if (isBowAiming)
        {
            float aimingAngle =
                cameraYaw + aimYawOffset;

            float smoothedAimAngle =
                Mathf.SmoothDampAngle(
                    transform.eulerAngles.y,
                    aimingAngle,
                    ref turnSmoothVelocity,
                    aimTurnSmoothTime
                );

            transform.rotation =
                Quaternion.Euler(
                    0f,
                    smoothedAimAngle,
                    0f
                );
        }

        if (currentMoveInput.sqrMagnitude > 0.01f)
        {
            if (isCrouching)
            {
                currentMoveSpeed =
                    crouchSpeed;
            }
            else if (isRunning)
            {
                currentMoveSpeed =
                    runSpeed;
            }
            else
            {
                currentMoveSpeed =
                    walkSpeed;
            }

            float targetAngle =
                Mathf.Atan2(
                    currentMoveInput.x,
                    currentMoveInput.y
                ) *
                Mathf.Rad2Deg +
                cameraYaw;

            // Outside aiming mode, rotate toward
            // the direction of movement.
            if (!isBowAiming)
            {
                float smoothedMovementAngle =
                    Mathf.SmoothDampAngle(
                        transform.eulerAngles.y,
                        targetAngle,
                        ref turnSmoothVelocity,
                        turnSmoothTime
                    );

                transform.rotation =
                    Quaternion.Euler(
                        0f,
                        smoothedMovementAngle,
                        0f
                    );
            }

            // Movement remains relative to the camera.
            // While aiming, this lets the player strafe.
            Vector3 moveDirection =
                Quaternion.Euler(
                    0f,
                    targetAngle,
                    0f
                ) *
                Vector3.forward;

            characterController.Move(
                moveDirection.normalized *
                currentMoveSpeed *
                Time.deltaTime
            );
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            isGrounded &&
            !isCrouching)
        {
            verticalVelocity =
                Mathf.Sqrt(
                    jumpHeight *
                    -2f *
                    gravity
                );
        }

        verticalVelocity +=
            gravity * Time.deltaTime;

        characterController.Move(
            Vector3.up *
            verticalVelocity *
            Time.deltaTime
        );

        isJumping =
            !characterController.isGrounded &&
            verticalVelocity > 0.1f;

        isFreeFalling =
            !characterController.isGrounded &&
            verticalVelocity < -0.1f;
    }

    private float GetCameraYaw()
    {
        if (cameraTransform == null)
        {
            return transform.eulerAngles.y;
        }

        Vector3 flatCameraForward =
            cameraTransform.forward;

        flatCameraForward.y = 0f;

        if (flatCameraForward.sqrMagnitude < 0.001f)
        {
            return transform.eulerAngles.y;
        }

        flatCameraForward.Normalize();

        return Mathf.Atan2(
            flatCameraForward.x,
            flatCameraForward.z
        ) * Mathf.Rad2Deg;
    }

    private Vector2 GetMoveInput()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            input.y += 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            input.y -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            input.x += 1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            input.x -= 1f;
        }

        return Vector2.ClampMagnitude(
            input,
            1f
        );
    }

    private void HandleCrouch()
    {
        bool wantsToCrouch =
            Keyboard.current.leftCtrlKey.isPressed ||
            Keyboard.current.cKey.isPressed;

        if (wantsToCrouch)
        {
            isCrouching = true;
        }
        else
        {
            if (!IsBlockedAbove())
            {
                isCrouching = false;
            }
        }

        float targetHeight =
            isCrouching
                ? crouchHeight
                : standingHeight;

        Vector3 targetCenter =
            isCrouching
                ? crouchingCenter
                : standingCenter;

        characterController.height =
            Mathf.Lerp(
                characterController.height,
                targetHeight,
                crouchTransitionSpeed *
                Time.deltaTime
            );

        characterController.center =
            Vector3.Lerp(
                characterController.center,
                targetCenter,
                crouchTransitionSpeed *
                Time.deltaTime
            );
    }

    private bool IsBlockedAbove()
    {
        float heightDifference =
            standingHeight -
            characterController.height;

        if (heightDifference <= 0.05f)
        {
            return false;
        }

        Vector3 sphereStart =
            transform.position +
            characterController.center +
            Vector3.up *
            (
                characterController.height / 2f -
                characterController.radius
            );

        return Physics.SphereCast(
            sphereStart,
            characterController.radius * 0.9f,
            Vector3.up,
            out RaycastHit hit,
            heightDifference + 0.1f,
            ceilingCheckLayers,
            QueryTriggerInteraction.Ignore
        );
    }

    private void UpdateAnimator()
    {
        if (animator == null)
        {
            return;
        }

        float inputAmount =
            currentMoveInput.magnitude;

        SetAnimatorFloat(
            "Speed",
            currentMoveSpeed
        );

        SetAnimatorFloat(
            "MotionSpeed",
            inputAmount
        );

        SetAnimatorBool(
            "Grounded",
            characterController.isGrounded
        );

        SetAnimatorBool(
            "Jump",
            isJumping
        );

        SetAnimatorBool(
            "FreeFall",
            isFreeFalling
        );

        SetAnimatorBool(
            "IsRunning",
            isRunning
        );

        SetAnimatorBool(
            "IsCrouching",
            isCrouching
        );

        SetAnimatorBool(
            "IsGrounded",
            characterController.isGrounded
        );
    }

    private void SetAnimatorFloat(
        string parameterName,
        float value
    )
    {
        if (HasAnimatorParameter(
                parameterName,
                AnimatorControllerParameterType.Float))
        {
            animator.SetFloat(
                parameterName,
                value,
                animationDampTime,
                Time.deltaTime
            );
        }
    }

    private void SetAnimatorBool(
        string parameterName,
        bool value
    )
    {
        if (HasAnimatorParameter(
                parameterName,
                AnimatorControllerParameterType.Bool))
        {
            animator.SetBool(
                parameterName,
                value
            );
        }
    }

    private bool HasAnimatorParameter(
        string parameterName,
        AnimatorControllerParameterType parameterType
    )
    {
        if (animator == null)
        {
            return false;
        }

        foreach (
            AnimatorControllerParameter parameter
            in animator.parameters
        )
        {
            if (parameter.name == parameterName &&
                parameter.type == parameterType)
            {
                return true;
            }
        }

        return false;
    }
}