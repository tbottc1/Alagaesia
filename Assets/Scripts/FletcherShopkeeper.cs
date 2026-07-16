using UnityEngine;
using UnityEngine.InputSystem;

public class FletcherShopkeeper : MonoBehaviour
{
    [Header("Shop Settings")]
    public string shopkeeperName = "Fletcher";
    public int bowAndArrowsCost = 45;
    public int arrowRefillCost = 5;

    [Header("First Purchase Dialogue")]
    [TextArea]
    public string greetingDialogue =
        "A hatchling will need meat, and you will need a steady hand. " +
        "I can sell you a bow and a bundle of arrows.";

    [TextArea]
    public string purchasePrompt =
        "Press E again to buy the bow and arrows.";

    [TextArea]
    public string purchasedDialogue =
        "Good. Keep the string dry, and do not waste your arrows.";

    [Header("Arrow Refill Dialogue")]
    [TextArea]
    public string refillGreetingDialogue =
        "Back already? I can sell you another bundle of arrows.";

    [TextArea]
    public string refillPrompt =
        "Press E again to buy more arrows.";

    [TextArea]
    public string refillPurchasedDialogue =
        "Here is another bundle. Make these shots count.";

    [TextArea]
    public string alreadyOwnedDialogue =
        "You still have arrows. Use them wisely.";

    [TextArea]
    public string notEnoughGoldDialogue =
        "You do not have enough gold.";

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

        if (GameUIManager.Instance != null &&
            GameUIManager.Instance.MenuOpen)
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
        BasicThirdPersonPlayer player =
            other.GetComponentInParent<BasicThirdPersonPlayer>();

        if (player == null)
        {
            return;
        }

        currentInventory =
            player.GetComponent<PlayerInventory>();

        if (currentInventory == null)
        {
            Debug.Log(
                "Player was found, but PlayerInventory is missing."
            );

            return;
        }

        playerInRange = true;
        shopConversationOpen = false;

        string prompt =
            "Press E to talk to " +
            shopkeeperName +
            ".";

        Debug.Log(prompt);

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowPrompt(prompt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BasicThirdPersonPlayer player =
            other.GetComponentInParent<BasicThirdPersonPlayer>();

        if (player == null)
        {
            return;
        }

        playerInRange = false;
        shopConversationOpen = false;
        currentInventory = null;

        Debug.Log(
            "Left " +
            shopkeeperName +
            "'s shop."
        );

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

            return;
        }

        if (currentInventory.hasBow &&
            currentInventory.arrowCount > 0)
        {
            Debug.Log(
                shopkeeperName +
                ": " +
                alreadyOwnedDialogue
            );

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    alreadyOwnedDialogue
                );
            }

            return;
        }

        if (!shopConversationOpen)
        {
            OpenConversation();
            return;
        }

        TryMakePurchase();
    }

    private void OpenConversation()
    {
        shopConversationOpen = true;

        if (!currentInventory.hasBow)
        {
            Debug.Log(
                shopkeeperName +
                ": " +
                greetingDialogue
            );

            Debug.Log(
                "Bow and Arrows Cost: " +
                bowAndArrowsCost +
                " gold."
            );

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    greetingDialogue
                );

                GameUIManager.Instance.ShowPrompt(
                    purchasePrompt
                );
            }
        }
        else
        {
            Debug.Log(
                shopkeeperName +
                ": " +
                refillGreetingDialogue
            );

            Debug.Log(
                "Arrow Refill Cost: " +
                arrowRefillCost +
                " gold."
            );

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    refillGreetingDialogue
                );

                GameUIManager.Instance.ShowPrompt(
                    refillPrompt
                );
            }
        }
    }

    private void TryMakePurchase()
    {
        if (!currentInventory.hasBow)
        {
            TryBuyBowAndArrows();
        }
        else
        {
            TryBuyArrowRefill();
        }
    }

    private void TryBuyBowAndArrows()
    {
        if (!currentInventory.CanAfford(
                bowAndArrowsCost))
        {
            ShowNotEnoughGold();
            return;
        }

        bool purchaseSucceeded =
            currentInventory.BuyBowAndArrows(
                bowAndArrowsCost
            );

        if (!purchaseSucceeded)
        {
            return;
        }

        if (bowDisplayObject != null)
        {
            bowDisplayObject.SetActive(false);
        }

        if (arrowDisplayObject != null)
        {
            arrowDisplayObject.SetActive(false);
        }

        FinishPurchase(purchasedDialogue);
    }

    private void TryBuyArrowRefill()
    {
        if (!currentInventory.CanAfford(
                arrowRefillCost))
        {
            ShowNotEnoughGold();
            return;
        }

        bool purchaseSucceeded =
            currentInventory.BuyArrows(
                arrowRefillCost
            );

        if (!purchaseSucceeded)
        {
            return;
        }

        FinishPurchase(
            refillPurchasedDialogue
        );
    }

    private void ShowNotEnoughGold()
    {
        Debug.Log(
            shopkeeperName +
            ": " +
            notEnoughGoldDialogue
        );

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(
                notEnoughGoldDialogue
            );

            GameUIManager.Instance.ShowPrompt(
                currentInventory.hasBow
                    ? refillPrompt
                    : purchasePrompt
            );
        }
    }

    private void FinishPurchase(string dialogue)
    {
        Debug.Log(
            shopkeeperName +
            ": " +
            dialogue
        );

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(
                dialogue
            );

            GameUIManager.Instance.HidePrompt();
            GameUIManager.Instance.RefreshInventoryUI();
        }

        shopConversationOpen = false;
    }
}