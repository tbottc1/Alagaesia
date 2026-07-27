using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Currency")]
    public int gold = 100;

    [Header("Archery")]
    public bool hasBow = false;
    public bool hasArrows = false;
    public int arrowCount = 0;
    public int arrowsReceivedWithBow = 10;
    public int arrowsPerRefill = 10;

    [Header("Story Items")]
    public bool hasBackpack = false;
    public bool hasSaddle = false;
    public bool hasDragonEgg = false;
    public DragonEggType selectedDragonEgg = DragonEggType.None;

    [Header("Progress")]
    public int totalPickups = 0;

    public string SelectedDragonEggName
    {
        get
        {
            switch (selectedDragonEgg)
            {
                case DragonEggType.Emerald:
                    return "Emerald Egg";

                case DragonEggType.Ruby:
                    return "Ruby Egg";

                case DragonEggType.Sapphire:
                    return "Sapphire Egg";

                default:
                    return "None";
            }
        }
    }

    public void AddPickup(StoryPickup.PickupType pickupType)
    {
        switch (pickupType)
        {
            case StoryPickup.PickupType.Backpack:
                hasBackpack = true;
                Debug.Log("Inventory updated: Backpack collected.");
                break;

            case StoryPickup.PickupType.Saddle:
                hasSaddle = true;
                Debug.Log("Inventory updated: Saddle collected.");
                break;

            case StoryPickup.PickupType.DragonEgg:
                // Kept for compatibility with any older StoryPickup objects.
                // The new hatchery system should use TryChooseDragonEgg instead.
                if (!hasDragonEgg)
                {
                    hasDragonEgg = true;
                    totalPickups++;
                }

                Debug.Log("Inventory updated: Dragon egg collected.");
                return;
        }

        totalPickups++;
    }

    public bool TryChooseDragonEgg(DragonEggType eggType)
    {
        if (hasDragonEgg)
        {
            Debug.Log(
                "A dragon egg has already been chosen: " +
                SelectedDragonEggName
            );

            return false;
        }

        if (eggType == DragonEggType.None)
        {
            Debug.LogWarning(
                "Cannot choose a dragon egg with the type None.",
                this
            );

            return false;
        }

        selectedDragonEgg = eggType;
        hasDragonEgg = true;
        totalPickups++;

        Debug.Log(
            "Dragon egg chosen: " +
            SelectedDragonEggName
        );

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.RefreshInventoryUI();
        }

        return true;
    }

    public bool CanAfford(int cost)
    {
        return gold >= cost;
    }

    public bool SpendGold(int amount)
    {
        if (!CanAfford(amount))
        {
            Debug.Log("Not enough gold.");
            return false;
        }

        gold -= amount;

        Debug.Log("Gold remaining: " + gold);
        return true;
    }

    public bool BuySaddle(int saddleCost)
    {
        if (hasSaddle)
        {
            Debug.Log("You already own a saddle.");
            return false;
        }

        if (!SpendGold(saddleCost))
        {
            Debug.Log("You cannot afford the saddle.");
            return false;
        }

        hasSaddle = true;
        totalPickups++;

        Debug.Log("Purchased saddle.");
        return true;
    }

    public bool BuyBowAndArrows(int bowAndArrowsCost)
    {
        if (hasBow)
        {
            Debug.Log("You already own the bow.");
            return false;
        }

        if (!SpendGold(bowAndArrowsCost))
        {
            Debug.Log("You cannot afford the bow and arrows.");
            return false;
        }

        hasBow = true;
        arrowCount += arrowsReceivedWithBow;
        hasArrows = arrowCount > 0;
        totalPickups++;

        Debug.Log(
            "Purchased bow and arrows. Arrows available: " +
            arrowCount
        );

        return true;
    }

    public bool BuyArrows(int arrowCost)
    {
        if (!hasBow)
        {
            Debug.Log(
                "You need to own a bow before buying replacement arrows."
            );

            return false;
        }

        if (!SpendGold(arrowCost))
        {
            Debug.Log("You cannot afford more arrows.");
            return false;
        }

        arrowCount += arrowsPerRefill;
        hasArrows = arrowCount > 0;

        Debug.Log(
            "Purchased more arrows. Arrows available: " +
            arrowCount
        );

        return true;
    }

    public bool UseArrow()
    {
        if (arrowCount <= 0)
        {
            arrowCount = 0;
            hasArrows = false;

            Debug.Log("You are out of arrows.");
            return false;
        }

        arrowCount--;
        hasArrows = arrowCount > 0;

        Debug.Log("Arrows remaining: " + arrowCount);

        if (arrowCount == 0)
        {
            Debug.Log(
                "Return to the Fletcher for more arrows."
            );

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    "You are out of arrows. Return to the Fletcher."
                );

                GameUIManager.Instance.RefreshInventoryUI();
            }
        }

        return true;
    }
}
