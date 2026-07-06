using UnityEngine;
using UnityEngine.InputSystem;

public class FletcherShopkeeper : MonoBehaviour
{
    [Header("Shop Settings")]
    public string shopkeeperName = "Fletcher";
    public int bowAndArrowsCost = 45;

    [Header("Dialogue")]
    [TextArea]
    public string greetingDialogue = "A hatchling will need meat, and you will need a steady hand. I can sell you a bow and a bundle of arrows.";

    [TextArea]
    public string purchasePrompt = "Press E again to buy the bow and arrows.";

    [TextArea]
    public string purchasedDialogue = "Good. Keep the string dry, and do not waste your arrows.";

    [TextArea]
    public string alreadyOwnedDialogue = "You already have a bow and arrows. Use them wisely.";

    [TextArea]
    public string notEnoughGoldDialogue = "You do not have enough gold for the bow and arrows.";

    [Header("Optional Scene Objects")]
    public GameObject bowDisplayObject;
    public GameObject arrowDisplayObject;

    private bool playerInRange = false;
    private bool shopConversationOpen = false;

    private PlayerInventory currentInventory;

    private void Update()
    {
        if (!playerInRange)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            HandleInteraction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BasicThirdPersonPlayer player = other.GetComponentInParent<BasicThirdPersonPlayer>();

        if (player == null)
        {
            return;
        }

        currentInventory = player.GetComponent<PlayerInventory>();

        if (currentInventory == null)
        {
            Debug.Log("Player was found, but PlayerInventory is missing.");
            return;
        }

        playerInRange = true;
        shopConversationOpen = false;

        string prompt = "Press E to talk to " + shopkeeperName + ".";

        Debug.Log(prompt);

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowPrompt(prompt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BasicThirdPersonPlayer player = other.GetComponentInParent<BasicThirdPersonPlayer>();

        if (player == null)
        {
            return;
        }

        playerInRange = false;
        shopConversationOpen = false;
        currentInventory = null;

        Debug.Log("Left " + shopkeeperName + "'s shop.");

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.HidePrompt();
        }
    }

    private void HandleInteraction()
    {
        if (currentInventory == null)
        {
            Debug.Log("No player inventory found.");

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage("No player inventory found.");
            }

            return;
        }

        if (currentInventory.hasBow && currentInventory.hasArrows)
        {
            Debug.Log(shopkeeperName + ": " + alreadyOwnedDialogue);

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(alreadyOwnedDialogue);
            }

            return;
        }

        if (!shopConversationOpen)
        {
            OpenConversation();
            return;
        }

        TryBuyBowAndArrows();
    }

    private void OpenConversation()
    {
        shopConversationOpen = true;

        Debug.Log(shopkeeperName + ": " + greetingDialogue);
        Debug.Log("Bow and Arrows Cost: " + bowAndArrowsCost + " gold.");
        Debug.Log(purchasePrompt);

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(greetingDialogue);
            GameUIManager.Instance.ShowPrompt(purchasePrompt);
        }
    }

    private void TryBuyBowAndArrows()
    {
        if (!currentInventory.CanAfford(bowAndArrowsCost))
        {
            Debug.Log(shopkeeperName + ": " + notEnoughGoldDialogue);

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(notEnoughGoldDialogue);
                GameUIManager.Instance.ShowPrompt(purchasePrompt);
            }

            return;
        }

        bool purchaseSucceeded = currentInventory.BuyBowAndArrows(bowAndArrowsCost);

        if (!purchaseSucceeded)
        {
            return;
        }

        Debug.Log(shopkeeperName + ": " + purchasedDialogue);

        if (bowDisplayObject != null)
        {
            bowDisplayObject.SetActive(false);
        }

        if (arrowDisplayObject != null)
        {
            arrowDisplayObject.SetActive(false);
        }

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(purchasedDialogue);
            GameUIManager.Instance.HidePrompt();
            GameUIManager.Instance.RefreshInventoryUI();
        }

        shopConversationOpen = false;
    }
}