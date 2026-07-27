using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EggSelectionManager : MonoBehaviour
{
    [Header("Confirmation Panel")]
    [SerializeField]
    private GameObject confirmationPanel;

    [SerializeField]
    private TextMeshProUGUI questionText;

    [SerializeField]
    private TextMeshProUGUI eggNameText;

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;

    [Header("Text")]
    [TextArea]
    [SerializeField]
    private string question =
        "Is this the dragon you would like?";

    [Header("References")]
    [SerializeField]
    private GameObjectiveManager objectiveManager;

    private DragonEggInteractable pendingEgg;
    private BasicThirdPersonPlayer pendingPlayer;
    private PlayerInventory pendingInventory;

    private PlayerBowController bowController;
    private ThirdPersonCameraFollow cameraController;
    private GameUIManager uiManager;

    private bool playerWasEnabled;
    private bool bowWasEnabled;
    private bool cameraWasEnabled;
    private bool uiManagerWasEnabled;

    private float previousTimeScale = 1f;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }

        if (yesButton != null)
        {
            yesButton.onClick.AddListener(
                ConfirmChoice
            );
        }

        if (noButton != null)
        {
            noButton.onClick.AddListener(
                CancelChoice
            );
        }

        if (objectiveManager == null)
        {
            objectiveManager =
                FindAnyObjectByType<GameObjectiveManager>();
        }
    }

    private void Update()
    {
        if (!IsOpen ||
            Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CancelChoice();
        }
    }

    public void OpenConfirmation(
        DragonEggInteractable egg,
        BasicThirdPersonPlayer player,
        PlayerInventory inventory)
    {
        if (IsOpen ||
            egg == null ||
            player == null ||
            inventory == null)
        {
            return;
        }

        if (inventory.hasDragonEgg)
        {
            return;
        }

        if (objectiveManager == null)
        {
            objectiveManager =
                FindAnyObjectByType<GameObjectiveManager>();
        }

        if (objectiveManager == null ||
            !objectiveManager.CanChooseDragonEgg())
        {
            return;
        }

        pendingEgg = egg;
        pendingPlayer = player;
        pendingInventory = inventory;

        if (questionText != null)
        {
            questionText.text = question;
        }

        if (eggNameText != null)
        {
            eggNameText.text =
                pendingEgg.EggDisplayName;
        }

        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true);
        }

        IsOpen = true;

        PausePlayerForConfirmation();
    }

    public void ConfirmChoice()
    {
        if (!IsOpen ||
            pendingEgg == null ||
            pendingInventory == null)
        {
            return;
        }

        bool chosen =
            pendingInventory.TryChooseDragonEgg(
                pendingEgg.EggType
            );

        if (!chosen)
        {
            CloseConfirmation(false);
            return;
        }

        DragonEggInteractable[] allEggs =
            FindObjectsByType<DragonEggInteractable>(
                FindObjectsSortMode.None
            );

        foreach (DragonEggInteractable egg in allEggs)
        {
            if (egg == null)
            {
                continue;
            }

            egg.ResolveChoice(
                egg == pendingEgg
            );
        }

        if (objectiveManager != null)
        {
            objectiveManager.CompleteGame();
        }

        string chosenEggName =
            pendingEgg.EggDisplayName;

        CloseConfirmation(false);

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(
                "You chose the " +
                chosenEggName +
                "."
            );

            GameUIManager.Instance.RefreshInventoryUI();
        }
    }

    public void CancelChoice()
    {
        if (!IsOpen)
        {
            return;
        }

        DragonEggInteractable eggToRestore =
            pendingEgg;

        CloseConfirmation(true);

        if (eggToRestore != null)
        {
            eggToRestore.ShowPromptAgain();
        }
    }

    private void PausePlayerForConfirmation()
    {
        previousTimeScale =
            Time.timeScale;

        Time.timeScale = 0f;

        bowController =
            pendingPlayer.GetComponent<PlayerBowController>();

        cameraController =
            Camera.main != null
                ? Camera.main.GetComponent<ThirdPersonCameraFollow>()
                : null;

        uiManager =
            GameUIManager.Instance;

        playerWasEnabled =
            pendingPlayer.enabled;

        pendingPlayer.enabled = false;

        if (bowController != null)
        {
            bowWasEnabled =
                bowController.enabled;

            bowController.enabled = false;
        }

        if (cameraController != null)
        {
            cameraWasEnabled =
                cameraController.enabled;

            cameraController.enabled = false;
        }

        if (uiManager != null)
        {
            uiManager.HidePrompt();

            uiManagerWasEnabled =
                uiManager.enabled;

            uiManager.enabled = false;
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    private void CloseConfirmation(
        bool restoreInteraction)
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(false);
        }

        ResumePlayerAfterConfirmation();

        IsOpen = false;

        if (!restoreInteraction)
        {
            pendingEgg = null;
        }

        pendingPlayer = null;
        pendingInventory = null;
        bowController = null;
        cameraController = null;
        uiManager = null;

        if (restoreInteraction)
        {
            // pendingEgg is cleared after CancelChoice has had
            // a chance to restore its prompt.
            pendingEgg = null;
        }
    }

    private void ResumePlayerAfterConfirmation()
    {
        Time.timeScale =
            previousTimeScale;

        if (pendingPlayer != null)
        {
            pendingPlayer.enabled =
                playerWasEnabled;
        }

        if (bowController != null)
        {
            bowController.enabled =
                bowWasEnabled;
        }

        if (cameraController != null)
        {
            cameraController.enabled =
                cameraWasEnabled;
        }

        if (uiManager != null)
        {
            uiManager.enabled =
                uiManagerWasEnabled;

            uiManager.RefreshInventoryUI();
        }

        Cursor.visible = false;
        Cursor.lockState =
            CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        if (yesButton != null)
        {
            yesButton.onClick.RemoveListener(
                ConfirmChoice
            );
        }

        if (noButton != null)
        {
            noButton.onClick.RemoveListener(
                CancelChoice
            );
        }
    }
}
