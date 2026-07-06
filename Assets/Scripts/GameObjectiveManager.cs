using UnityEngine;

public class GameObjectiveManager : MonoBehaviour
{
    public enum ObjectiveState
    {
        GetBackpack,
        BuySaddle,
        BuyBowAndArrows,
        ChooseDragonEgg,
        BoardBoat,
        Complete
    }

    [Header("Current Objective")]
    public ObjectiveState currentObjective = ObjectiveState.GetBackpack;

    private PlayerInventory playerInventory;
    private bool gameComplete = false;

    private void Start()
    {
        FindPlayerInventory();
        UpdateObjectiveFromInventory();
        SendObjectiveToUI();
    }

    private void Update()
    {
        if (playerInventory == null)
        {
            FindPlayerInventory();
            return;
        }

        UpdateObjectiveFromInventory();
    }

    private void FindPlayerInventory()
    {
        BasicThirdPersonPlayer player = FindAnyObjectByType<BasicThirdPersonPlayer>();

        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }
    }

    private void UpdateObjectiveFromInventory()
    {
        ObjectiveState newObjective = GetObjectiveFromInventory();

        if (newObjective == currentObjective)
        {
            return;
        }

        currentObjective = newObjective;

        Debug.Log("Objective updated: " + currentObjective);

        SendObjectiveToUI();
    }

    private ObjectiveState GetObjectiveFromInventory()
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

        if (!playerInventory.hasBow || !playerInventory.hasArrows)
        {
            return ObjectiveState.BuyBowAndArrows;
        }

        if (!playerInventory.hasDragonEgg)
        {
            return ObjectiveState.ChooseDragonEgg;
        }

        return ObjectiveState.BoardBoat;
    }

    private void SendObjectiveToUI()
    {
        if (GameUIManager.Instance == null)
        {
            return;
        }

        GameUIManager.Instance.SetObjective(GetObjectiveText());
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

            case ObjectiveState.ChooseDragonEgg:
                return "Return to the egg chamber and choose your dragon egg.";

            case ObjectiveState.BoardBoat:
                return "Board the boat and leave Vroengard.";

            case ObjectiveState.Complete:
                return "You have left Vroengard with your dragon egg.";

            default:
                return "No current objective.";
        }
    }

    public bool CanChooseDragonEgg()
    {
        return currentObjective == ObjectiveState.ChooseDragonEgg;
    }

    public bool CanBoardBoat()
    {
        return currentObjective == ObjectiveState.BoardBoat;
    }

    public void CompleteGame()
    {
        gameComplete = true;
        currentObjective = ObjectiveState.Complete;
        SendObjectiveToUI();

        Debug.Log("Game complete.");
    }
}