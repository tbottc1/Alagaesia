using UnityEngine;
using UnityEngine.InputSystem;

public class StoryPickup : MonoBehaviour
{
    public enum PickupType
    {
        Backpack,
        Saddle,
        DragonEgg
    }

    [Header("Pickup Settings")]
    public PickupType pickupType;
    public string itemName = "Backpack";
    public bool destroyAfterPickup = true;

    [Header("UI Text")]
    public string interactionPrompt = "Press E to pick up.";
    public string pickupMessage = "Picked up item.";

    [Header("Audio")]
    public AudioClip pickupSound;

    private bool playerInRange = false;
    private bool hasBeenPickedUp = false;

    private BasicThirdPersonPlayer currentPlayer;
    private PlayerInventory currentInventory;

    private void Update()
    {
        if (hasBeenPickedUp)
        {
            return;
        }

        if (!playerInRange)
        {
            return;
        }

        if (GameUIManager.Instance != null && GameUIManager.Instance.MenuOpen)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUpItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenPickedUp)
        {
            return;
        }

        BasicThirdPersonPlayer player = other.GetComponentInParent<BasicThirdPersonPlayer>();

        if (player == null)
        {
            return;
        }

        currentPlayer = player;
        currentInventory = currentPlayer.GetComponent<PlayerInventory>();

        if (currentInventory == null)
        {
            Debug.Log("Player was found, but PlayerInventory is missing.");
            return;
        }

        playerInRange = true;

        string prompt = interactionPrompt;

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

        if (player != currentPlayer)
        {
            return;
        }

        playerInRange = false;
        currentPlayer = null;
        currentInventory = null;

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.HidePrompt();
        }
    }

    private void PickUpItem()
    {
        if (currentInventory == null)
        {
            Debug.Log("No inventory found for pickup.");
            return;
        }

        hasBeenPickedUp = true;

        currentInventory.AddPickup(pickupType);

        Debug.Log(pickupMessage);

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(pickupMessage);
            GameUIManager.Instance.HidePrompt();
            GameUIManager.Instance.RefreshInventoryUI();
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        if (destroyAfterPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}