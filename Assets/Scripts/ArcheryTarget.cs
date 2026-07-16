using UnityEngine;

public class ArcheryTarget : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ArcheryRangeManager rangeManager;

    [Tooltip(
        "Assign a target pivot here. The pivot should be positioned " +
        "near the bottom of the target."
    )]
    [SerializeField] private Transform targetPivot;

    [Header("Hit Reaction")]
    [SerializeField] private Vector3 knockedLocalEulerAngles =
        new Vector3(-75f, 0f, 0f);

    [SerializeField] private float knockdownSpeed = 8f;
    [SerializeField] private bool disableCollidersAfterHit = true;

    [Header("Optional Audio")]
    [SerializeField] private AudioClip targetHitSound;
    [Range(0f, 1f)]
    [SerializeField] private float targetHitVolume = 0.8f;

    private Collider[] targetColliders;

    private Quaternion standingRotation;
    private Quaternion knockedRotation;

    private bool hasBeenHit;
    private bool isKnockingDown;

    public bool HasBeenHit
    {
        get { return hasBeenHit; }
    }

    private void Awake()
    {
        if (rangeManager == null)
        {
            rangeManager =
                FindAnyObjectByType<ArcheryRangeManager>();
        }

        if (targetPivot == null)
        {
            targetPivot = transform;
        }

        targetColliders =
            GetComponentsInChildren<Collider>();

        standingRotation =
            targetPivot.localRotation;

        knockedRotation =
            standingRotation *
            Quaternion.Euler(knockedLocalEulerAngles);
    }

    private void Update()
    {
        if (!isKnockingDown || targetPivot == null)
        {
            return;
        }

        targetPivot.localRotation =
            Quaternion.Slerp(
                targetPivot.localRotation,
                knockedRotation,
                knockdownSpeed * Time.deltaTime
            );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenHit)
        {
            return;
        }

        ArrowProjectile arrow =
            collision.gameObject.GetComponentInParent<ArrowProjectile>();

        if (arrow == null)
        {
            return;
        }

        TryRegisterHit();
    }

    private void TryRegisterHit()
    {
        if (rangeManager == null)
        {
            Debug.LogWarning(
                "ArcheryTarget cannot find ArcheryRangeManager.",
                this
            );

            return;
        }

        bool hitAccepted =
            rangeManager.RegisterTargetHit(this);

        if (!hitAccepted)
        {
            return;
        }

        hasBeenHit = true;
        isKnockingDown = true;

        if (disableCollidersAfterHit)
        {
            foreach (Collider targetCollider in targetColliders)
            {
                targetCollider.enabled = false;
            }
        }

        if (targetHitSound != null)
        {
            AudioSource.PlayClipAtPoint(
                targetHitSound,
                transform.position,
                targetHitVolume
            );
        }
    }
}