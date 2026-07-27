using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class DragonEggInteractable : MonoBehaviour
{
    [Header("Egg")]
    [SerializeField]
    private DragonEggType eggType =
        DragonEggType.Emerald;

    [SerializeField]
    private string eggDisplayName =
        "Emerald Egg";

    [Tooltip(
        "Assign the egg model/root that should disappear " +
        "when this egg is chosen. Do not assign the pedestal."
    )]
    [SerializeField]
    private GameObject eggVisualRoot;

    [Header("Interaction")]
    [SerializeField]
    private string interactionPrompt =
        "Press E to inspect the Emerald Egg.";

    [SerializeField]
    private EggSelectionManager selectionManager;

    [SerializeField]
    private GameObjectiveManager objectiveManager;

    private Collider interactionCollider;
    private BasicThirdPersonPlayer currentPlayer;
    private PlayerInventory currentInventory;

    private bool playerInRange;
    private bool choiceResolved;

    public DragonEggType EggType
    {
        get { return eggType; }
    }

    public string EggDisplayName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(
                    eggDisplayName))
            {
                return eggDisplayName;
            }

            return eggType + " Egg";
        }
    }

    private void Awake()
    {
        interactionCollider =
            GetComponent<Collider>();

        interactionCollider.isTrigger = true;

        FindReferences();
    }

    private void Update()
    {
        if (!playerInRange ||
            choiceResolved)
        {
            return;
        }

        FindReferences();

        if (selectionManager != null &&
            selectionManager.IsOpen)
        {
            return;
        }

        if (GameUIManager.Instance != null &&
            GameUIManager.Instance.MenuOpen)
        {
            return;
        }

        if (Keyboard.current == null ||
            !Keyboard.current.eKey.wasPressedThisFrame)
        {
            return;
        }

        TryInspectEgg();
    }

    private void OnTriggerEnter(Collider other)
    {
        BasicThirdPersonPlayer player =
            other.GetComponentInParent<BasicThirdPersonPlayer>();

        if (player == null ||
            choiceResolved)
        {
            return;
        }

        currentPlayer = player;
        currentInventory =
            player.GetComponent<PlayerInventory>();

        if (currentInventory == null)
        {
            Debug.LogWarning(
                "Dragon egg found the player, but PlayerInventory is missing.",
                this
            );

            return;
        }

        playerInRange = true;
        RefreshPrompt();
    }

    private void OnTriggerExit(Collider other)
    {
        BasicThirdPersonPlayer player =
            other.GetComponentInParent<BasicThirdPersonPlayer>();

        if (player == null ||
            player != currentPlayer)
        {
            return;
        }

        playerInRange = false;
        currentPlayer = null;
        currentInventory = null;

        HidePrompt();
    }

    private void FindReferences()
    {
        if (selectionManager == null)
        {
            selectionManager =
                FindAnyObjectByType<EggSelectionManager>();
        }

        if (objectiveManager == null)
        {
            objectiveManager =
                FindAnyObjectByType<GameObjectiveManager>();
        }
    }

    private void RefreshPrompt()
    {
        if (!playerInRange ||
            choiceResolved ||
            currentInventory == null)
        {
            HidePrompt();
            return;
        }

        if (currentInventory.hasDragonEgg)
        {
            HidePrompt();
            return;
        }

        if (objectiveManager == null ||
            !objectiveManager.CanChooseDragonEgg())
        {
            HidePrompt();
            return;
        }

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowPrompt(
                interactionPrompt
            );
        }
    }

    private void TryInspectEgg()
    {
        if (currentInventory == null ||
            currentPlayer == null)
        {
            return;
        }

        if (currentInventory.hasDragonEgg)
        {
            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    "You have already chosen your dragon egg."
                );
            }

            HidePrompt();
            return;
        }

        if (objectiveManager == null ||
            !objectiveManager.CanChooseDragonEgg())
        {
            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    "You must complete your training before choosing an egg."
                );
            }

            return;
        }

        if (selectionManager == null)
        {
            Debug.LogWarning(
                "No EggSelectionManager was found in the scene.",
                this
            );

            return;
        }

        HidePrompt();

        selectionManager.OpenConfirmation(
            this,
            currentPlayer,
            currentInventory
        );
    }

    public void ResolveChoice(bool wasChosen)
    {
        choiceResolved = true;
        playerInRange = false;

        HidePrompt();

        if (interactionCollider != null)
        {
            interactionCollider.enabled = false;
        }

        if (wasChosen &&
            eggVisualRoot != null)
        {
            eggVisualRoot.SetActive(false);
        }

        enabled = false;
    }

    public void ShowPromptAgain()
    {
        RefreshPrompt();
    }

    private void HidePrompt()
    {
        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.HidePrompt();
        }
    }

    private void OnDisable()
    {
        HidePrompt();
    }
}
