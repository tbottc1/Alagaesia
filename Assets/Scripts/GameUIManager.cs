using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI interactionPromptText;
    public TextMeshProUGUI messageText;

    [Header("Player Menu")]
    public GameObject playerMenuPanel;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI objectiveText;

    [Header("Message Settings")]
    public float messageDuration = 3f;

    [Header("Menu Settings")]
    public bool pauseGameWhenMenuOpen = true;

    private PlayerInventory playerInventory;
    private Coroutine messageRoutine;

    private bool menuOpen = false;
    private string currentObjectiveText = "Pick up your backpack.";

    public bool MenuOpen
    {
        get { return menuOpen; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        FindPlayerInventory();

        HidePrompt();
        ClearMessage();

        if (playerMenuPanel != null)
        {
            playerMenuPanel.SetActive(false);
        }

        if (inventoryText != null)
        {
            inventoryText.gameObject.SetActive(true);
        }

        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false);
        }

        RefreshInventoryUI();
        RefreshObjectiveUI();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            TogglePlayerMenu();
        }

        if (playerInventory == null)
        {
            FindPlayerInventory();
        }

        RefreshInventoryUI();
        RefreshObjectiveUI();

        if (menuOpen)
        {
            KeepCursorFreeForMenu();
        }
    }

    private void LateUpdate()
    {
        if (menuOpen)
        {
            KeepCursorFreeForMenu();
        }
    }

    private void FindPlayerInventory()
    {
        BasicThirdPersonPlayer player = FindAnyObjectByType<BasicThirdPersonPlayer>();

        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }
    }

    private void KeepCursorFreeForMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TogglePlayerMenu()
    {
        if (menuOpen)
        {
            ClosePlayerMenu();
        }
        else
        {
            OpenPlayerMenu();
        }
    }

    public void OpenPlayerMenu()
    {
        menuOpen = true;

        if (playerMenuPanel != null)
        {
            playerMenuPanel.SetActive(true);
        }
        else
        {
            Debug.Log("PlayerMenuPanel is not assigned.");
        }

        ShowInventoryText();
        KeepCursorFreeForMenu();

        if (pauseGameWhenMenuOpen)
        {
            Time.timeScale = 0f;
        }
    }

    public void ClosePlayerMenu()
    {
        menuOpen = false;

        if (playerMenuPanel != null)
        {
            playerMenuPanel.SetActive(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (pauseGameWhenMenuOpen)
        {
            Time.timeScale = 1f;
        }
    }

    public void ShowInventoryText()
    {
        Debug.Log("Showing inventory text.");

        if (inventoryText != null)
        {
            inventoryText.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Inventory text is not assigned.");
        }

        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Objective text is not assigned.");
        }

        RefreshInventoryUI();
    }

    public void ShowObjectiveText()
    {
        Debug.Log("Showing objective text.");

        if (inventoryText != null)
        {
            inventoryText.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Inventory text is not assigned.");
        }

        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Objective text is not assigned.");
        }

        RefreshObjectiveUI();
    }

    // Extra aliases in case any older button setup is still pointing to these names.
    public void ShowInventoryPanel()
    {
        ShowInventoryText();
    }

    public void ShowTasksPanel()
    {
        ShowObjectiveText();
    }

    public void SetObjective(string objective)
    {
        currentObjectiveText = objective;
        RefreshObjectiveUI();
    }

    public void RefreshInventoryUI()
    {
        if (playerInventory == null)
        {
            return;
        }

        if (goldText != null)
        {
            goldText.text = "Gold: " + playerInventory.gold;
        }

        if (inventoryText != null)
        {
            inventoryText.text =
                "Inventory\n\n" +
                "Gold: " + playerInventory.gold + "\n\n" +
                "Backpack: " + YesNo(playerInventory.hasBackpack) + "\n" +
                "Saddle: " + YesNo(playerInventory.hasSaddle) + "\n" +
                "Bow: " + YesNo(playerInventory.hasBow) + "\n" +
                "Arrows: " + YesNo(playerInventory.hasArrows) + "\n" +
                "Dragon Egg: " + YesNo(playerInventory.hasDragonEgg);
        }
    }

    public void RefreshObjectiveUI()
    {
        if (objectiveText == null)
        {
            return;
        }

        objectiveText.text =
            "Tasks\n\n" +
            "Current Objective:\n" +
            currentObjectiveText;
    }

    public void ShowPrompt(string prompt)
    {
        if (interactionPromptText == null)
        {
            return;
        }

        interactionPromptText.gameObject.SetActive(true);
        interactionPromptText.text = prompt;
    }

    public void HidePrompt()
    {
        if (interactionPromptText == null)
        {
            return;
        }

        interactionPromptText.text = "";
        interactionPromptText.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (messageText == null)
        {
            return;
        }

        if (messageRoutine != null)
        {
            StopCoroutine(messageRoutine);
        }

        messageRoutine = StartCoroutine(MessageRoutine(message));
    }

    private IEnumerator MessageRoutine(string message)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;

        yield return new WaitForSecondsRealtime(messageDuration);

        ClearMessage();
    }

    private void ClearMessage()
    {
        if (messageText == null)
        {
            return;
        }

        messageText.text = "";
        messageText.gameObject.SetActive(false);
    }

    private string YesNo(bool value)
    {
        if (value)
        {
            return "Yes";
        }

        return "No";
    }
}