using System.Collections.Generic;
using UnityEngine;

public class DeerSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeerHuntTarget deerPrefab;
    [SerializeField] private DeerSpawnZone spawnZone;
    [SerializeField] private Transform deerParent;

    [Header("Population")]
    [SerializeField] private int minimumLivingDeer = 5;
    [SerializeField] private float populationCheckInterval = 2f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayers = ~0;
    [SerializeField] private float groundRayHeight = 20f;
    [SerializeField] private float groundRayDistance = 80f;

    private readonly List<DeerHuntTarget> spawnedDeer =
        new List<DeerHuntTarget>();

    private float nextPopulationCheck;

    private void Start()
    {
        RegisterExistingDeer();
        MaintainPopulation();
    }

    private void Update()
    {
        if (Time.time < nextPopulationCheck)
        {
            return;
        }

        nextPopulationCheck =
            Time.time + populationCheckInterval;

        MaintainPopulation();
    }

    private void RegisterExistingDeer()
    {
        DeerHuntTarget[] existingDeer =
            FindObjectsByType<DeerHuntTarget>(
                FindObjectsSortMode.None
            );

        foreach (DeerHuntTarget deer in existingDeer)
        {
            if (deer == null)
            {
                continue;
            }

            if (spawnZone != null &&
                !spawnZone.ContainsXZ(deer.transform.position))
            {
                continue;
            }

            if (!spawnedDeer.Contains(deer))
            {
                spawnedDeer.Add(deer);
            }

            DeerAI deerAI =
                deer.GetComponent<DeerAI>();

            if (deerAI != null)
            {
                deerAI.SetSpawnZone(spawnZone);
            }
        }
    }

    private void MaintainPopulation()
    {
        spawnedDeer.RemoveAll(
            deer => deer == null
        );

        int livingDeerCount = 0;

        foreach (DeerHuntTarget deer in spawnedDeer)
        {
            if (deer != null &&
                !deer.IsDefeated)
            {
                livingDeerCount++;
            }
        }

        int deerNeeded =
            minimumLivingDeer - livingDeerCount;

        for (int index = 0;
             index < deerNeeded;
             index++)
        {
            TrySpawnDeer();
        }
    }

    private bool TrySpawnDeer()
    {
        if (deerPrefab == null)
        {
            Debug.LogWarning(
                "DeerSpawner is missing its deer prefab.",
                this
            );

            return false;
        }

        if (spawnZone == null)
        {
            Debug.LogWarning(
                "DeerSpawner is missing its spawn zone.",
                this
            );

            return false;
        }

        bool foundGround =
            spawnZone.TryGetRandomGroundPoint(
                groundLayers,
                groundRayHeight,
                groundRayDistance,
                out Vector3 spawnPosition
            );

        if (!foundGround)
        {
            Debug.LogWarning(
                "DeerSpawner could not find valid ground.",
                this
            );

            return false;
        }

        Quaternion spawnRotation =
            Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

        DeerHuntTarget newDeer = Instantiate(
            deerPrefab,
            spawnPosition,
            spawnRotation,
            deerParent
        );

        spawnedDeer.Add(newDeer);

        DeerAI deerAI =
            newDeer.GetComponent<DeerAI>();

        if (deerAI != null)
        {
            deerAI.SetSpawnZone(spawnZone);
        }

        return true;
    }

    private void OnValidate()
    {
        minimumLivingDeer =
            Mathf.Max(1, minimumLivingDeer);

        populationCheckInterval =
            Mathf.Max(0.25f, populationCheckInterval);
    }
}