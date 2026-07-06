using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Currency")]
    public int gold = 100;

    [Header("Story Items")]
    public bool hasBackpack = false;
    public bool hasSaddle = false;
    public bool hasBow = false;
    public bool hasArrows = false;
    public bool hasDragonEgg = false;


    public int totalPickups = 0;

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
                hasDragonEgg = true;
                Debug.Log("Inventory updated: Dragon egg collected.");
                break;
        }

        totalPickups++;
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
        if (hasBow && hasArrows)
        {
            Debug.Log("You already own a bow and arrows.");
            return false;
        }

        if (!SpendGold(bowAndArrowsCost))
        {
            Debug.Log("You cannot afford the bow and arrows.");
            return false;
        }

        hasBow = true;
        hasArrows = true;
        totalPickups++;

        Debug.Log("Purchased bow and arrows.");
        return true;
    }
}