using System.Collections.Generic;
using UnityEngine;

public class ArcheryRangeManager : MonoBehaviour
{
    [Header("Range Settings")]
    [SerializeField] private int requiredTargets = 5;

    [Header("References")]
    [SerializeField] private GameObjectiveManager objectiveManager;

    private readonly HashSet<ArcheryTarget> hitTargets =
        new HashSet<ArcheryTarget>();

    private bool initialProgressShown;

    public int TargetsHit
    {
        get { return hitTargets.Count; }
    }

    public int RequiredTargets
    {
        get { return requiredTargets; }
    }

    public bool IsComplete
    {
        get { return TargetsHit >= requiredTargets; }
    }

    private void Awake()
    {
        if (objectiveManager == null)
        {
            objectiveManager =
                FindAnyObjectByType<GameObjectiveManager>();
        }
    }

    private void Update()
    {
        if (objectiveManager == null)
        {
            objectiveManager =
                FindAnyObjectByType<GameObjectiveManager>();

            return;
        }

        bool archeryObjectiveActive =
            objectiveManager.currentObjective ==
            GameObjectiveManager.ObjectiveState.CompleteArcheryTraining;

        if (archeryObjectiveActive &&
            !initialProgressShown &&
            !IsComplete)
        {
            initialProgressShown = true;
            UpdateProgressText();
        }
    }

    public bool RegisterTargetHit(ArcheryTarget target)
    {
        if (target == null || IsComplete)
        {
            return false;
        }

        if (objectiveManager == null)
        {
            Debug.LogWarning(
                "ArcheryRangeManager cannot find GameObjectiveManager.",
                this
            );

            return false;
        }

        if (objectiveManager.currentObjective !=
            GameObjectiveManager.ObjectiveState.CompleteArcheryTraining)
        {
            Debug.Log(
                "The archery training objective is not active yet."
            );

            return false;
        }

        if (!hitTargets.Add(target))
        {
            return false;
        }

        Debug.Log(
            "Archery target hit: " +
            TargetsHit +
            " / " +
            requiredTargets
        );

        if (IsComplete)
        {
            CompleteArcheryTraining();
        }
        else
        {
            UpdateProgressText();

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    "Target hit! " +
                    TargetsHit +
                    " of " +
                    requiredTargets +
                    " targets completed."
                );
            }
        }

        return true;
    }

    private void UpdateProgressText()
    {
        if (GameUIManager.Instance == null)
        {
            return;
        }

        GameUIManager.Instance.SetObjective(
            "Complete your archery training.\n" +
            "Targets Hit: " +
            TargetsHit +
            " / " +
            requiredTargets
        );
    }

    private void CompleteArcheryTraining()
    {
        Debug.Log("Archery training complete.");

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(
                "Archery training complete! " +
                "Your next task is to hunt and collect two deer."
            );
        }

        objectiveManager.CompleteArcheryTraining();
    }
}