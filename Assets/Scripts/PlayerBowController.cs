using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBowController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Bow")]
    [SerializeField] private GameObject bowInHand;
    [SerializeField] private GameObject nockedArrow;
    [SerializeField] private Transform arrowSpawnPoint;

    [Header("Arrow")]
    [SerializeField] private ArrowProjectile arrowPrefab;
    [SerializeField] private float arrowSpeed = 35f;
    [SerializeField] private float fireCooldown = 0.5f;

    [Header("Aiming")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ThirdPersonCameraFollow cameraController;
    [SerializeField] private float maximumAimDistance = 200f;
    [SerializeField] private GameObject aimingCrosshair;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Input")]
    [SerializeField] private Key equipKey = Key.B;

    public bool IsBowEquipped { get; private set; }
    public bool IsAiming { get; private set; }

    private float nextFireTime;
    private float nextNockedArrowTime;

    private static readonly int BowAimingParameter =
        Animator.StringToHash("BowAiming");

    private static readonly int BowFireParameter =
        Animator.StringToHash("BowFire");

    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponent<PlayerInventory>();
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (cameraController == null && playerCamera != null)
        {
            cameraController =
                playerCamera.GetComponent<ThirdPersonCameraFollow>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogWarning(
                "PlayerBowController could not find the player's Animator.",
                this
            );
        }
        else
        {
            animator.SetBool(BowAimingParameter, false);
        }

        SetBowEquipped(false);
    }

    private void Update()
    {
        bool menuOpen =
            GameUIManager.Instance != null &&
            GameUIManager.Instance.MenuOpen;

        if (menuOpen)
        {
            SetAiming(false);
            return;
        }

        CheckEquipInput();
        CheckAimAndFireInput();
        RefreshNockedArrow();
    }

    private void CheckEquipInput()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current[equipKey].wasPressedThisFrame)
        {
            ToggleBow();
        }
    }

    private void CheckAimAndFireInput()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (!IsBowEquipped)
        {
            SetAiming(false);
            return;
        }

        SetAiming(Mouse.current.rightButton.isPressed);

        if (IsAiming &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryFireArrow();
        }
    }

    public void ToggleBow()
    {
        if (playerInventory == null)
        {
            Debug.LogError(
                "PlayerBowController could not find PlayerInventory.",
                this
            );

            return;
        }

        if (!playerInventory.hasBow)
        {
            Debug.Log("You do not own a bow yet.");
            return;
        }

        SetBowEquipped(!IsBowEquipped);
    }

    public void SetBowEquipped(bool equipped)
    {
        IsBowEquipped = equipped;

        if (bowInHand != null)
        {
            bowInHand.SetActive(equipped);
        }

        if (!equipped)
        {
            SetAiming(false);
        }

        RefreshNockedArrow();
    }

    private void SetAiming(bool aiming)
    {
        IsAiming = aiming;

        if (aimingCrosshair != null)
        {
            aimingCrosshair.SetActive(aiming);
        }

        if (cameraController != null)
        {
            cameraController.SetAiming(aiming);
        }

        if (animator != null)
        {
            animator.SetBool(
                BowAimingParameter,
                aiming
            );
        }

        RefreshNockedArrow();
    }

    private void RefreshNockedArrow()
    {
        if (nockedArrow == null)
        {
            return;
        }

        bool hasArrowAvailable =
            playerInventory != null &&
            playerInventory.arrowCount > 0;

        bool reloadFinished =
            Time.time >= nextNockedArrowTime;

        bool shouldShowArrow =
            IsBowEquipped &&
            IsAiming &&
            hasArrowAvailable &&
            reloadFinished;

        nockedArrow.SetActive(shouldShowArrow);
    }

    private void TryFireArrow()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        if (playerInventory == null)
        {
            Debug.LogError(
                "PlayerBowController is missing PlayerInventory.",
                this
            );

            return;
        }

        if (arrowPrefab == null)
        {
            Debug.LogError(
                "PlayerBowController is missing an arrow prefab.",
                this
            );

            return;
        }

        if (arrowSpawnPoint == null)
        {
            Debug.LogError(
                "PlayerBowController is missing an Arrow Spawn Point.",
                this
            );

            return;
        }

        if (playerCamera == null)
        {
            Debug.LogError(
                "PlayerBowController is missing the player camera.",
                this
            );

            return;
        }

        if (!playerInventory.UseArrow())
        {
            RefreshNockedArrow();
            return;
        }

        if (animator != null)
        {
            animator.ResetTrigger(BowFireParameter);
            animator.SetTrigger(BowFireParameter);
        }

        if (nockedArrow != null)
        {
            nockedArrow.SetActive(false);
        }

        Vector3 aimPoint = FindAimPoint();

        Vector3 fireDirection =
            (aimPoint - arrowSpawnPoint.position).normalized;

        ArrowProjectile arrow = Instantiate(
            arrowPrefab,
            arrowSpawnPoint.position,
            Quaternion.LookRotation(fireDirection)
        );

        arrow.SetOwner(gameObject);
        arrow.Launch(fireDirection, arrowSpeed);

        nextFireTime =
            Time.time + fireCooldown;

        nextNockedArrowTime =
            Time.time + fireCooldown;

        RefreshNockedArrow();
    }

    private Vector3 FindAimPoint()
    {
        Ray aimRay = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 aimPoint =
            aimRay.origin +
            aimRay.direction * maximumAimDistance;

        RaycastHit[] hits = Physics.RaycastAll(
            aimRay,
            maximumAimDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        Array.Sort(
            hits,
            (firstHit, secondHit) =>
                firstHit.distance.CompareTo(secondHit.distance)
        );

        foreach (RaycastHit hit in hits)
        {
            // Ignore the player and all objects attached to the player.
            if (hit.transform.root == transform.root)
            {
                continue;
            }

            aimPoint = hit.point;
            break;
        }

        return aimPoint;
    }

    private void OnDisable()
    {
        SetAiming(false);
    }
}