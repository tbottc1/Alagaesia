using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeerSpawnZone : MonoBehaviour
{
    [Header("Zone")]
    [SerializeField] private BoxCollider zoneCollider;
    [SerializeField] private float edgePadding = 1f;

    public Bounds WorldBounds
    {
        get { return zoneCollider.bounds; }
    }

    private void Awake()
    {
        if (zoneCollider == null)
        {
            zoneCollider = GetComponent<BoxCollider>();
        }

        zoneCollider.isTrigger = true;
    }

    private void Reset()
    {
        zoneCollider = GetComponent<BoxCollider>();
        zoneCollider.isTrigger = true;
    }

    public bool ContainsXZ(Vector3 position)
    {
        Bounds bounds = WorldBounds;

        return
            position.x >= bounds.min.x + edgePadding &&
            position.x <= bounds.max.x - edgePadding &&
            position.z >= bounds.min.z + edgePadding &&
            position.z <= bounds.max.z - edgePadding;
    }

    public bool TryGetRandomGroundPoint(
        LayerMask groundLayers,
        float rayHeight,
        float rayDistance,
        out Vector3 groundPoint)
    {
        Bounds bounds = WorldBounds;

        for (int attempt = 0; attempt < 15; attempt++)
        {
            float x = Random.Range(
                bounds.min.x + edgePadding,
                bounds.max.x - edgePadding
            );

            float z = Random.Range(
                bounds.min.z + edgePadding,
                bounds.max.z - edgePadding
            );

            Vector3 rayStart = new Vector3(
                x,
                bounds.max.y + rayHeight,
                z
            );

            if (Physics.Raycast(
                    rayStart,
                    Vector3.down,
                    out RaycastHit hit,
                    rayDistance,
                    groundLayers,
                    QueryTriggerInteraction.Ignore))
            {
                if (ContainsXZ(hit.point))
                {
                    groundPoint = hit.point;
                    return true;
                }
            }
        }

        groundPoint = transform.position;
        return false;
    }

    public bool TryGetGroundPointInside(
        Vector3 desiredPosition,
        LayerMask groundLayers,
        float rayHeight,
        float rayDistance,
        out Vector3 groundPoint)
    {
        Bounds bounds = WorldBounds;

        float x = Mathf.Clamp(
            desiredPosition.x,
            bounds.min.x + edgePadding,
            bounds.max.x - edgePadding
        );

        float z = Mathf.Clamp(
            desiredPosition.z,
            bounds.min.z + edgePadding,
            bounds.max.z - edgePadding
        );

        Vector3 rayStart = new Vector3(
            x,
            bounds.max.y + rayHeight,
            z
        );

        if (Physics.Raycast(
                rayStart,
                Vector3.down,
                out RaycastHit hit,
                rayDistance,
                groundLayers,
                QueryTriggerInteraction.Ignore))
        {
            groundPoint = hit.point;
            return true;
        }

        groundPoint = desiredPosition;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        BoxCollider currentCollider = zoneCollider;

        if (currentCollider == null)
        {
            currentCollider = GetComponent<BoxCollider>();
        }

        if (currentCollider == null)
        {
            return;
        }

        Gizmos.DrawWireCube(
            currentCollider.bounds.center,
            currentCollider.bounds.size
        );
    }
}