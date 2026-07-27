using UnityEngine;

public class GameObjectiveManager : MonoBehaviour
{
    public enum ObjectiveState
    {
        GetBackpack,
        BuySaddle,
        BuyBowAndArrows,
        RestockArrows,
        CompleteArcheryTraining,
        HuntDeer,
        ReadyForDragon,
        ChooseDragonEgg,

        // Kept so older scripts referencing this state still compile.
        BoardBoat,

        Complete
    }

    [Header("Current Objective")]
    public ObjectiveState currentObjective =
        ObjectiveState.GetBackpack;

    [Header("Archery Progress")]
    public bool archeryTrainingComplete = false;

    [Header("Hunting Progress")]
    public int deerCollected = 0;
    public int deerRequired = 2;

    private PlayerInventory playerInventory;
    private bool gameComplete;

    public bool IsGameComplete
    {
        get
        {
            return
                gameComplete ||
                currentObjective == ObjectiveState.Complete;
        }
    }

    private void Start()
    {
        FindPlayerInventory();
        RefreshCurrentObjective();
    }

    private void Update()
    {
        if (playerInventory == null)
        {
            FindPlayerInventory();

            if (playerInventory == null)
            {
                return;
            }
        }

        UpdateObjectiveFromProgress();
    }

    private void FindPlayerInventory()
    {
        BasicThirdPersonPlayer player =
            FindAnyObjectByType<BasicThirdPersonPlayer>();

        if (player != null)
        {
            playerInventory =
                player.GetComponent<PlayerInventory>();
        }
    }

    private void UpdateObjectiveFromProgress()
    {
        ObjectiveState newObjective =
            GetObjectiveFromProgress();

        if (newObjective == currentObjective)
        {
            return;
        }

        currentObjective = newObjective;

        Debug.Log(
            "Objective updated: " +
            currentObjective
        );

        SendObjectiveToUI();
    }

    private ObjectiveState GetObjectiveFromProgress()
    {
        if (gameComplete)
        {
            return ObjectiveState.Complete;
        }

        if (playerInventory == null)
        {
            return currentObjective;
        }

        if (!playerInventory.hasBackpack)
        {
            return ObjectiveState.GetBackpack;
        }

        if (!playerInventory.hasSaddle)
        {
            return ObjectiveState.BuySaddle;
        }

        if (!playerInventory.hasBow)
        {
            return ObjectiveState.BuyBowAndArrows;
        }

        if (!archeryTrainingComplete)
        {
            if (playerInventory.arrowCount <= 0)
            {
                return ObjectiveState.RestockArrows;
            }

            return ObjectiveState.CompleteArcheryTraining;
        }

        if (deerCollected < deerRequired)
        {
            if (playerInventory.arrowCount <= 0)
            {
                return ObjectiveState.RestockArrows;
            }

            return ObjectiveState.HuntDeer;
        }

        if (!playerInventory.hasDragonEgg)
        {
            return ObjectiveState.ChooseDragonEgg;
        }

        return ObjectiveState.Complete;
    }

    private void RefreshCurrentObjective()
    {
        currentObjective =
            GetObjectiveFromProgress();

        SendObjectiveToUI();
    }

    private void SendObjectiveToUI()
    {
        if (GameUIManager.Instance == null)
        {
            return;
        }

        GameUIManager.Instance.SetObjective(
            GetObjectiveText()
        );
    }

    private string GetObjectiveText()
    {
        switch (currentObjective)
        {
            case ObjectiveState.GetBackpack:
                return "Pick up your backpack.";

            case ObjectiveState.BuySaddle:
                return "Buy a saddle from the saddler.";

            case ObjectiveState.BuyBowAndArrows:
                return "Buy a bow and arrows from the fletcher.";

            case ObjectiveState.RestockArrows:
                return
                    "You are out of arrows. " +
                    "Return to the fletcher.";

            case ObjectiveState.CompleteArcheryTraining:
                return
                    "Go to the archery range and hit all five targets.";

            case ObjectiveState.HuntDeer:
                return
                    "Hunt and collect two deer in the grassy mountains.\n" +
                    "Deer Collected: " +
                    deerCollected +
                    " / " +
                    deerRequired;

            case ObjectiveState.ReadyForDragon:
            case ObjectiveState.ChooseDragonEgg:
                return
                    "Return to the hatchery and choose your dragon egg.";

            case ObjectiveState.BoardBoat:
                return
                    "Board the boat and leave Vroengard.";

            case ObjectiveState.Complete:
                return
                    "You have chosen your dragon egg. " +
                    "Your training is complete.";

            default:
                return "No current objective.";
        }
    }

    public void CompleteArcheryTraining()
    {
        if (archeryTrainingComplete)
        {
            return;
        }

        archeryTrainingComplete = true;

        Debug.Log(
            "Archery training marked complete."
        );

        RefreshCurrentObjective();
    }

    public void RegisterDeerCollected()
    {
        if (!archeryTrainingComplete)
        {
            Debug.Log(
                "The player cannot complete the hunting objective yet."
            );

            return;
        }

        if (deerCollected >= deerRequired)
        {
            return;
        }

        deerCollected++;

        Debug.Log(
            "Deer collected: " +
            deerCollected +
            " / " +
            deerRequired
        );

        RefreshCurrentObjective();
    }

    public bool CanChooseDragonEgg()
    {
        if (playerInventory == null)
        {
            FindPlayerInventory();
        }

        return
            playerInventory != null &&
            !playerInventory.hasDragonEgg &&
            archeryTrainingComplete &&
            deerCollected >= deerRequired;
    }

    public bool CanBoardBoat()
    {
        return
            currentObjective ==
            ObjectiveState.BoardBoat;
    }

    public void CompleteGame()
    {
        gameComplete = true;
        currentObjective =
            ObjectiveState.Complete;

        SendObjectiveToUI();

        Debug.Log("Game complete.");
    }
}
