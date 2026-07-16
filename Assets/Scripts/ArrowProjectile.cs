using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArrowProjectile : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float lifetime = 12f;
    [SerializeField] private float minimumRotationSpeed = 0.5f;
    [SerializeField] private bool stickIntoObjects = true;

    private Rigidbody arrowRigidbody;
    private Collider[] arrowColliders;
    private bool hasHitSomething;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody>();
        arrowColliders = GetComponentsInChildren<Collider>();

        Destroy(gameObject, lifetime);
    }

    public void SetOwner(GameObject owner)
    {
        if (owner == null)
        {
            return;
        }

        Collider[] ownerColliders =
            owner.GetComponentsInChildren<Collider>();

        foreach (Collider arrowCollider in arrowColliders)
        {
            foreach (Collider ownerCollider in ownerColliders)
            {
                Physics.IgnoreCollision(
                    arrowCollider,
                    ownerCollider,
                    true
                );
            }
        }
    }

    public void Launch(Vector3 direction, float speed)
    {
        if (arrowRigidbody == null)
        {
            return;
        }

        transform.rotation =
            Quaternion.LookRotation(direction);

        arrowRigidbody.AddForce(
            direction.normalized * speed,
            ForceMode.VelocityChange
        );
    }

    private void FixedUpdate()
    {
        if (hasHitSomething ||
            arrowRigidbody == null)
        {
            return;
        }

        Vector3 movementDirection =
            arrowRigidbody.linearVelocity;

        if (movementDirection.sqrMagnitude >
            minimumRotationSpeed * minimumRotationSpeed)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    movementDirection.normalized
                );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHitSomething)
        {
            return;
        }

        hasHitSomething = true;

        // Check whether the arrow struck a deer.
        DeerHuntTarget deer =
            collision.collider
                .GetComponentInParent<DeerHuntTarget>();

        if (deer != null)
        {
            deer.HitByArrow(this);
        }

        if (!stickIntoObjects)
        {
            return;
        }

        arrowRigidbody.linearVelocity =
            Vector3.zero;

        arrowRigidbody.angularVelocity =
            Vector3.zero;

        arrowRigidbody.useGravity = false;
        arrowRigidbody.isKinematic = true;

        foreach (Collider arrowCollider in arrowColliders)
        {
            arrowCollider.enabled = false;
        }

        transform.SetParent(
            collision.transform,
            true
        );
    }
}