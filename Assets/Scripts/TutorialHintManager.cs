using TMPro;
using UnityEngine;

public class TutorialHintManager : MonoBehaviour
{
    public static TutorialHintManager Instance;

    [Header("UI")]
    public TextMeshProUGUI tutorialText;

    [Header("References")]
    public PlayerInventory playerInventory;

    private bool playerOpenedMenu = false;
    private bool menuTutorialComplete = false;

    private string currentMessage = "";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetHint("Press TAB to open your menu.");
    }

    private void Update()
    {
        if (GameUIManager.Instance == null || playerInventory == null)
        {
            return;
        }

        HandleMenuTutorialStep();

        if (menuTutorialComplete)
        {
            HandleProgressTutorialSteps();
        }
    }

    private void HandleMenuTutorialStep()
    {
        bool menuOpen = GameUIManager.Instance.MenuOpen;

        if (!playerOpenedMenu)
        {
            if (menuOpen)
            {
                playerOpenedMenu = true;
                SetHint("");
            }

            return;
        }

        if (!menuTutorialComplete && !menuOpen)
        {
            menuTutorialComplete = true;
        }
    }

    private void HandleProgressTutorialSteps()
    {
        if (!playerInventory.hasBackpack)
        {
            SetHint("Find your backpack near the starting area.");
        }
        else if (!playerInventory.hasSaddle)
        {
            SetHint("Visit the Saddler inside his shop.");
        }
        else if (!playerInventory.hasBow)
        {
            SetHint("Visit the Fletcher inside his shop.");
        }
        else if (playerInventory.arrowCount <= 0)
        {
            SetHint("You are out of arrows. Return to the Fletcher.");
        }
        else
        {
            SetHint("");
        }
    }

    private void SetHint(string message)
    {
        if (currentMessage == message)
        {
            return;
        }

        currentMessage = message;

        if (tutorialText != null)
        {
            tutorialText.text = message;
            tutorialText.gameObject.SetActive(!string.IsNullOrEmpty(message));
        }
    }
}