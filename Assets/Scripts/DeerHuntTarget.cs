using UnityEngine;
using UnityEngine.InputSystem;
using ithappy.Animals_FREE;

public class DeerHuntTarget : MonoBehaviour
{
    [Header("Deer References")]
    [SerializeField] private DeerAI deerAI;
    [SerializeField] private CreatureMover creatureMover;
    [SerializeField] private Animator deerAnimator;
    [SerializeField] private CharacterController deerController;

    [Tooltip(
        "The visual model that should rotate when the deer falls. " +
        "Do not use the CharacterController root if a visual child is available."
    )]
    [SerializeField] private Transform visualRoot;

    [Header("Defeated Appearance")]
    [SerializeField] private Vector3 defeatedLocalEulerAngles =
        new Vector3(0f, 0f, 90f);

    [SerializeField] private float fallSpeed = 5f;

    [Header("Collection")]
    [SerializeField] private float collectDistance = 2.5f;
    [SerializeField] private string collectPrompt =
        "Press E to collect deer.";

    [Header("Progress")]
    [SerializeField] private GameObjectiveManager gameManager;

    [Header("Optional Audio")]
    [SerializeField] private AudioClip deerHitSound;
    [SerializeField] private AudioClip deerCollectSound;

    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 0.8f;

    private BasicThirdPersonPlayer player;

    private Quaternion standingRotation;
    private Quaternion defeatedRotation;

    private bool isDefeated;
    private bool isCollected;
    private bool promptVisible;

    public bool IsDefeated
    {
        get { return isDefeated; }
    }

    private void Awake()
    {
        if (deerAI == null)
        {
            deerAI = GetComponent<DeerAI>();
        }

        if (creatureMover == null)
        {
            creatureMover =
                GetComponent<CreatureMover>();
        }

        if (deerAnimator == null)
        {
            deerAnimator =
                GetComponent<Animator>();
        }

        if (deerController == null)
        {
            deerController =
                GetComponent<CharacterController>();
        }

        if (gameManager == null)
        {
            gameManager =
                FindAnyObjectByType<GameObjectiveManager>();
        }

        if (visualRoot == null)
        {
            visualRoot = transform;
        }

        standingRotation =
            visualRoot.localRotation;

        defeatedRotation =
            standingRotation *
            Quaternion.Euler(
                defeatedLocalEulerAngles
            );
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (!isDefeated || isCollected)
        {
            return;
        }

        RotateIntoDefeatedPose();
        HandleCollection();
    }

    public void HitByArrow(ArrowProjectile arrow)
    {
        if (isDefeated || isCollected)
        {
            return;
        }

        isDefeated = true;

        if (deerAI != null)
        {
            deerAI.Defeat();
            deerAI.enabled = false;
        }

        if (creatureMover != null)
        {
            creatureMover.enabled = false;
        }

        // Freeze the current animated pose.
        if (deerAnimator != null)
        {
            deerAnimator.enabled = false;
        }

        // The deer no longer needs its movement collider.
        if (deerController != null)
        {
            deerController.enabled = false;
        }

        if (deerHitSound != null)
        {
            AudioSource.PlayClipAtPoint(
                deerHitSound,
                transform.position,
                soundVolume
            );
        }

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowMessage(
                "Deer defeated. Walk up and collect it."
            );
        }

        Debug.Log(name + " was defeated by an arrow.");
    }

    private void RotateIntoDefeatedPose()
    {
        if (visualRoot == null)
        {
            return;
        }

        visualRoot.localRotation =
            Quaternion.Slerp(
                visualRoot.localRotation,
                defeatedRotation,
                fallSpeed * Time.deltaTime
            );
    }

    private void HandleCollection()
    {
        if (player == null)
        {
            FindPlayer();

            if (player == null)
            {
                return;
            }
        }

        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.transform.position
            );

        bool menuOpen =
            GameUIManager.Instance != null &&
            GameUIManager.Instance.MenuOpen;

        bool canCollect =
            distanceToPlayer <= collectDistance &&
            !menuOpen;

        if (canCollect)
        {
            ShowCollectionPrompt();

            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                CollectDeer();
            }
        }
        else
        {
            HideCollectionPrompt();
        }
    }

    private void FindPlayer()
    {
        player =
            FindAnyObjectByType<BasicThirdPersonPlayer>();
    }

    private void ShowCollectionPrompt()
    {
        if (promptVisible)
        {
            return;
        }

        promptVisible = true;

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.ShowPrompt(
                collectPrompt
            );
        }
    }

    private void HideCollectionPrompt()
    {
        if (!promptVisible)
        {
            return;
        }

        promptVisible = false;

        if (GameUIManager.Instance != null)
        {
            GameUIManager.Instance.HidePrompt();
        }
    }

    private void CollectDeer()
    {
        if (isCollected)
        {
            return;
        }

        isCollected = true;

        HideCollectionPrompt();

        if (gameManager == null)
        {
            gameManager =
                FindAnyObjectByType<GameObjectiveManager>();
        }

        if (gameManager != null)
        {
            gameManager.RegisterDeerCollected();

            if (GameUIManager.Instance != null)
            {
                GameUIManager.Instance.ShowMessage(
                    "Deer collected: " +
                    gameManager.deerCollected +
                    " / " +
                    gameManager.deerRequired
                );
            }
        }
        else
        {
            Debug.LogWarning(
                "Deer was collected, but no " +
                "GameObjectiveManager was found.",
                this
            );
        }

        if (deerCollectSound != null)
        {
            AudioSource.PlayClipAtPoint(
                deerCollectSound,
                transform.position,
                soundVolume
            );
        }

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        HideCollectionPrompt();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            collectDistance
        );
    }
}