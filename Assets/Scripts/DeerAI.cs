using UnityEngine;
using ithappy.Animals_FREE;

[RequireComponent(typeof(CreatureMover))]
public class DeerAI : MonoBehaviour
{
    public enum DeerState
    {
        Idle,
        Wandering,
        Fleeing,
        Defeated
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private DeerSpawnZone spawnZone;

    [Header("Wandering")]
    [SerializeField] private float wanderRadius = 12f;
    [SerializeField] private float minimumIdleTime = 2f;
    [SerializeField] private float maximumIdleTime = 5f;
    [SerializeField] private float targetReachDistance = 1.2f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayers = ~0;
    [SerializeField] private float groundCheckHeight = 15f;
    [SerializeField] private float groundCheckDistance = 60f;

    [Header("Fleeing")]
    [SerializeField] private float fleeDistance = 10f;
    [SerializeField] private float calmDistance = 18f;
    [SerializeField] private float fleeTargetDistance = 12f;
    [SerializeField] private float fleeRetargetInterval = 0.5f;

    [Header("Debug")]
    [SerializeField] private DeerState currentState;

    private CreatureMover creatureMover;

    private Vector3 homePosition;
    private Vector3 movementTarget;

    private float stateTimer;
    private float nextFleeRetargetTime;

    public DeerState CurrentState
    {
        get { return currentState; }
    }

    public bool IsDefeated
    {
        get
        {
            return currentState ==
                   DeerState.Defeated;
        }
    }

    private void Awake()
    {
        creatureMover =
            GetComponent<CreatureMover>();

        homePosition =
            transform.position;
    }

    private void Start()
    {
        FindPlayer();
        EnterIdleState();
    }

    private void Update()
    {
        if (currentState ==
            DeerState.Defeated)
        {
            StopMoving();
            return;
        }

        if (player == null)
        {
            FindPlayer();
        }

        if (spawnZone != null &&
            !spawnZone.ContainsXZ(transform.position))
        {
            ReturnToSpawnZone();
            return;
        }

        CheckForPlayer();

        switch (currentState)
        {
            case DeerState.Idle:
                UpdateIdleState();
                break;

            case DeerState.Wandering:
                UpdateWanderingState();
                break;

            case DeerState.Fleeing:
                UpdateFleeingState();
                break;
        }
    }

    public void SetSpawnZone(
        DeerSpawnZone newSpawnZone)
    {
        spawnZone = newSpawnZone;
    }

    private void FindPlayer()
    {
        BasicThirdPersonPlayer foundPlayer =
            FindAnyObjectByType<BasicThirdPersonPlayer>();

        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
    }

    private void CheckForPlayer()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer =
            FlatDistance(
                transform.position,
                player.position
            );

        if (currentState != DeerState.Fleeing &&
            distanceToPlayer <= fleeDistance)
        {
            EnterFleeingState();
        }
    }

    private void UpdateIdleState()
    {
        StopMoving();

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            ChooseWanderTarget();
        }
    }

    private void UpdateWanderingState()
    {
        MoveTowardTarget(false);

        float distanceToTarget =
            FlatDistance(
                transform.position,
                movementTarget
            );

        if (distanceToTarget <=
            targetReachDistance)
        {
            EnterIdleState();
        }
    }

    private void UpdateFleeingState()
    {
        if (player == null)
        {
            EnterIdleState();
            return;
        }

        float distanceToPlayer =
            FlatDistance(
                transform.position,
                player.position
            );

        if (distanceToPlayer >= calmDistance)
        {
            EnterIdleState();
            return;
        }

        if (Time.time >=
            nextFleeRetargetTime)
        {
            ChooseFleeTarget();

            nextFleeRetargetTime =
                Time.time +
                fleeRetargetInterval;
        }

        MoveTowardTarget(true);
    }

    private void EnterIdleState()
    {
        currentState =
            DeerState.Idle;

        stateTimer =
            Random.Range(
                minimumIdleTime,
                maximumIdleTime
            );

        StopMoving();
    }

    private void EnterWanderingState()
    {
        currentState =
            DeerState.Wandering;
    }

    private void EnterFleeingState()
    {
        currentState =
            DeerState.Fleeing;

        nextFleeRetargetTime = 0f;

        ChooseFleeTarget();
    }

    private void ChooseWanderTarget()
    {
        if (spawnZone != null)
        {
            bool foundPoint =
                spawnZone.TryGetRandomGroundPoint(
                    groundLayers,
                    groundCheckHeight,
                    groundCheckDistance,
                    out Vector3 zoneTarget
                );

            if (foundPoint)
            {
                movementTarget = zoneTarget;
                EnterWanderingState();
                return;
            }
        }

        Vector2 randomOffset =
            Random.insideUnitCircle *
            wanderRadius;

        Vector3 possibleTarget =
            homePosition +
            new Vector3(
                randomOffset.x,
                groundCheckHeight,
                randomOffset.y
            );

        if (TryFindGround(
                possibleTarget,
                out Vector3 groundedTarget))
        {
            movementTarget =
                groundedTarget;

            EnterWanderingState();
        }
        else
        {
            EnterIdleState();
        }
    }

    private void ChooseFleeTarget()
    {
        if (player == null)
        {
            return;
        }

        Vector3 fleeDirection =
            transform.position -
            player.position;

        fleeDirection.y = 0f;

        if (fleeDirection.sqrMagnitude <
            0.01f)
        {
            fleeDirection =
                -transform.forward;
        }

        fleeDirection.Normalize();

        Vector3 desiredTarget =
            transform.position +
            fleeDirection *
            fleeTargetDistance;

        if (spawnZone != null)
        {
            spawnZone.TryGetGroundPointInside(
                desiredTarget,
                groundLayers,
                groundCheckHeight,
                groundCheckDistance,
                out movementTarget
            );

            return;
        }

        desiredTarget.y +=
            groundCheckHeight;

        if (TryFindGround(
                desiredTarget,
                out Vector3 groundedTarget))
        {
            movementTarget =
                groundedTarget;
        }
        else
        {
            movementTarget =
                transform.position +
                fleeDirection *
                fleeTargetDistance;
        }
    }

    private void ReturnToSpawnZone()
    {
        if (spawnZone == null)
        {
            return;
        }

        Vector3 desiredTarget =
            spawnZone.WorldBounds.center;

        spawnZone.TryGetGroundPointInside(
            desiredTarget,
            groundLayers,
            groundCheckHeight,
            groundCheckDistance,
            out movementTarget
        );

        currentState =
            DeerState.Wandering;

        MoveTowardTarget(true);
    }

    private bool TryFindGround(
        Vector3 rayStart,
        out Vector3 groundPosition)
    {
        if (Physics.Raycast(
                rayStart,
                Vector3.down,
                out RaycastHit hit,
                groundCheckDistance,
                groundLayers,
                QueryTriggerInteraction.Ignore))
        {
            groundPosition =
                hit.point;

            return true;
        }

        groundPosition =
            transform.position;

        return false;
    }

    private void MoveTowardTarget(
        bool running)
    {
        if (creatureMover == null)
        {
            return;
        }

        creatureMover.SetInput(
            Vector2.up,
            movementTarget,
            running,
            false
        );
    }

    private void StopMoving()
    {
        if (creatureMover == null)
        {
            return;
        }

        Vector3 lookTarget =
            transform.position +
            transform.forward;

        creatureMover.SetInput(
            Vector2.zero,
            lookTarget,
            false,
            false
        );
    }

    private float FlatDistance(
        Vector3 firstPosition,
        Vector3 secondPosition)
    {
        firstPosition.y = 0f;
        secondPosition.y = 0f;

        return Vector3.Distance(
            firstPosition,
            secondPosition
        );
    }

    public void Defeat()
    {
        if (IsDefeated)
        {
            return;
        }

        currentState =
            DeerState.Defeated;

        StopMoving();

        Debug.Log(
            name +
            " has been defeated."
        );
    }

    private void OnDisable()
    {
        StopMoving();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center =
            Application.isPlaying
                ? homePosition
                : transform.position;

        Gizmos.DrawWireSphere(
            center,
            wanderRadius
        );

        Gizmos.DrawWireSphere(
            transform.position,
            fleeDistance
        );
    }
}