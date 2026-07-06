using UnityEngine;
using UnityEngine.InputSystem;

public class SaddleShopkeeper : MonoBehaviour
{
    [Header("Shop Settings")]
    public string shopkeeperName = "Saddler";
    public int saddleCost = 40;

    [Header("Dialogue")]
    [TextArea]
    public string greetingDialogue = "A young rider needs more than courage. You'll need a proper saddle for the road ahead.";

    [TextArea]
    public string purchasePrompt = "Press E again to buy the saddle.";

    [TextArea]
    public string purchasedDialogue = "A fine choice. This saddle should serve you well.";

    [TextArea]
    public string alreadyOwnedDialogue = "You've already bought the saddle. Take good care of it.";

    [TextArea]
    public string notEnoughGoldDialogue = "You don't have enough gold for this saddle.";

    [Header("Optional Scene Objects")]
    public GameObject saddleDisplayObject;

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

        if (currentInventory.hasSaddle)
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

        TryBuySaddle();
    }

    private void OpenConversation()
    {
        shopConversationOpen = true;

        Debug.Log(shopkeeperName + ": " + greetingDialogue);
        Debug.Log("Saddle Cost: " + saddleCost + " gold.");
        Debug.Log(purchasePrompt);

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(greetingDialogue);
            GameUIManager.Instance.ShowPrompt(purchasePrompt);
        }
    }

    private void TryBuySaddle()
    {
        if (!currentInventory.CanAfford(saddleCost))
        {
            Debug.Log(shopkeeperName + ": " + notEnoughGoldDialogue);

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(notEnoughGoldDialogue);
                GameUIManager.Instance.ShowPrompt(purchasePrompt);
            }

            return;
        }

        bool purchaseSucceeded = currentInventory.BuySaddle(saddleCost);

        if (!purchaseSucceeded)
        {
            return;
        }

        Debug.Log(shopkeeperName + ": " + purchasedDialogue);

        if (saddleDisplayObject != null)
        {
            saddleDisplayObject.SetActive(false);
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