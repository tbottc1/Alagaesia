using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialCompletionScreen : MonoBehaviour
{
    private enum TutorialEndState
    {
        None,
        Completed,
        Failed
    }

    [Header("References")]
    [SerializeField]
    private GameObjectiveManager gameManager;

    [SerializeField]
    private PlayerInventory playerInventory;

    [SerializeField]
    private FletcherShopkeeper fletcherShopkeeper;

    [SerializeField]
    private GameObject completionPanel;

    [SerializeField]
    private CanvasGroup completionCanvasGroup;

    [SerializeField]
    private TextMeshProUGUI completionText;

    [SerializeField]
    private TextMeshProUGUI restartText;

    [Header("UI Cleanup")]
    [Tooltip(
        "Assign the root GameObjects containing normal gameplay UI. " +
        "Do not assign the Completion Panel or any parent containing it."
    )]
    [SerializeField]
    private GameObject[] gameplayUIObjectsToHide;

    [Header("Completion Message")]
    [TextArea(4, 8)]
    [SerializeField]
    private string completionMessage =
        "Tutorial completed!\n\n" +
        "You chose the {0}.\n\n" +
        "Your journey as a Dragon Rider is about to begin.\n\n" +
        "To Be Continued....";

    [Header("Failure Settings")]
    [Tooltip(
        "Used only if no FletcherShopkeeper reference can be found. " +
        "Set this to the price of one arrow refill."
    )]
    [SerializeField]
    private int fallbackArrowRefillCost = 5;

    [Tooltip(
        "How long the impossible-to-continue condition must remain true " +
        "before the failure screen appears."
    )]
    [SerializeField]
    private float failureGracePeriod = 1f;

    [TextArea(4, 8)]
    [SerializeField]
    private string failureMessage =
        "Tutorial failed!\n\n" +
        "You ran out of arrows and do not have enough gold " +
        "to purchase more.\n\n" +
        "You were unable to complete the deer hunt.";

    [Header("Restart")]
    [SerializeField]
    private string restartMessage =
        "Press SPACE to restart";

    [Header("Appearance")]
    [SerializeField]
    private float fadeDuration = 1.25f;

    [Header("Game Behavior")]
    [SerializeField]
    private bool pauseGameOnEnding = true;

    private TutorialEndState endState =
        TutorialEndState.None;

    private bool restartReady;
    private float failureTimer;

    private void Awake()
    {
        Time.timeScale = 1f;

        FindReferences();

        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        if (completionCanvasGroup != null)
        {
            completionCanvasGroup.alpha = 0f;
        }
    }

    private void Update()
    {
        if (endState == TutorialEndState.None)
        {
            CheckForTutorialEnding();
            return;
        }

        if (!restartReady)
        {
            return;
        }

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            RestartScene();
        }
    }

    private void CheckForTutorialEnding()
    {
        FindReferences();

        if (gameManager == null ||
            playerInventory == null)
        {
            return;
        }

        bool eggChosen =
            playerInventory.hasDragonEgg;

        bool gameFinished =
            gameManager.IsGameComplete;

        if (eggChosen && gameFinished)
        {
            endState =
                TutorialEndState.Completed;

            StartCoroutine(
                ShowEndScreen(
                    GetCompletionMessage()
                )
            );

            return;
        }

        if (ShouldFailTutorial())
        {
            failureTimer +=
                Time.unscaledDeltaTime;

            if (failureTimer >=
                failureGracePeriod)
            {
                endState =
                    TutorialEndState.Failed;

                StartCoroutine(
                    ShowEndScreen(
                        failureMessage
                    )
                );
            }
        }
        else
        {
            failureTimer = 0f;
        }
    }

    private bool ShouldFailTutorial()
    {
        if (playerInventory == null ||
            gameManager == null)
        {
            return false;
        }

        if (!playerInventory.hasBow)
        {
            return false;
        }

        if (gameManager.deerCollected >=
            gameManager.deerRequired)
        {
            return false;
        }

        if (playerInventory.arrowCount > 0)
        {
            return false;
        }

        int arrowRefillCost =
            GetArrowRefillCost();

        bool canAffordMoreArrows =
            playerInventory.gold >=
            arrowRefillCost;

        if (canAffordMoreArrows)
        {
            return false;
        }

        // Give the player's final arrow time to land before
        // deciding that the tutorial can no longer be completed.
        if (HasArrowStillInFlight())
        {
            return false;
        }

        // Do not fail if enough deer are already defeated and
        // only need to be walked up to and collected.
        int deerStillNeeded =
            Mathf.Max(
                0,
                gameManager.deerRequired -
                gameManager.deerCollected
            );

        int defeatedDeerAvailable =
            CountDefeatedUncollectedDeer();

        if (defeatedDeerAvailable >=
            deerStillNeeded)
        {
            return false;
        }

        return true;
    }

    private int GetArrowRefillCost()
    {
        if (fletcherShopkeeper != null)
        {
            return Mathf.Max(
                0,
                fletcherShopkeeper.arrowRefillCost
            );
        }

        return Mathf.Max(
            0,
            fallbackArrowRefillCost
        );
    }

    private bool HasArrowStillInFlight()
    {
        ArrowProjectile[] arrows =
            FindObjectsByType<ArrowProjectile>(
                FindObjectsSortMode.None
            );

        foreach (ArrowProjectile arrow in arrows)
        {
            if (arrow == null)
            {
                continue;
            }

            Rigidbody arrowRigidbody =
                arrow.GetComponent<Rigidbody>();

            if (arrowRigidbody == null)
            {
                continue;
            }

            bool moving =
                arrowRigidbody.linearVelocity
                    .sqrMagnitude > 0.01f;

            if (!arrowRigidbody.isKinematic &&
                moving)
            {
                return true;
            }
        }

        return false;
    }

    private int CountDefeatedUncollectedDeer()
    {
        DeerHuntTarget[] deer =
            FindObjectsByType<DeerHuntTarget>(
                FindObjectsSortMode.None
            );

        int defeatedCount = 0;

        foreach (DeerHuntTarget deerTarget in deer)
        {
            if (deerTarget != null &&
                deerTarget.IsDefeated)
            {
                defeatedCount++;
            }
        }

        return defeatedCount;
    }

    private string GetCompletionMessage()
    {
        string eggName =
            playerInventory != null
                ? playerInventory.SelectedDragonEggName
                : "Dragon Egg";

        return string.Format(
            completionMessage,
            eggName
        );
    }

    private IEnumerator ShowEndScreen(
        string message)
    {
        HideGameplayUI();

        if (completionText != null)
        {
            completionText.text = message;
        }

        if (restartText != null)
        {
            restartText.text =
                restartMessage;
        }

        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        if (pauseGameOnEnding)
        {
            Time.timeScale = 0f;
        }

        if (completionCanvasGroup == null)
        {
            restartReady = true;
            yield break;
        }

        completionCanvasGroup.alpha = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime +=
                Time.unscaledDeltaTime;

            completionCanvasGroup.alpha =
                Mathf.Clamp01(
                    elapsedTime /
                    fadeDuration
                );

            yield return null;
        }

        completionCanvasGroup.alpha = 1f;
        restartReady = true;
    }

    private void HideGameplayUI()
    {
        if (GameUIManager.Instance != null)
        {
            if (GameUIManager.Instance.MenuOpen)
            {
                GameUIManager.Instance.ClosePlayerMenu();
            }

            GameUIManager.Instance.HidePrompt();
        }

        if (gameplayUIObjectsToHide == null)
        {
            return;
        }

        foreach (GameObject uiObject in
                 gameplayUIObjectsToHide)
        {
            if (uiObject == null)
            {
                continue;
            }

            bool isCompletionPanel =
                completionPanel != null &&
                uiObject == completionPanel;

            bool containsCompletionPanel =
                completionPanel != null &&
                completionPanel.transform.IsChildOf(
                    uiObject.transform
                );

            if (isCompletionPanel ||
                containsCompletionPanel)
            {
                Debug.LogWarning(
                    uiObject.name +
                    " was not hidden because it contains " +
                    "the completion panel. Assign a smaller " +
                    "gameplay UI root instead.",
                    uiObject
                );

                continue;
            }

            uiObject.SetActive(false);
        }
    }

    private void FindReferences()
    {
        if (gameManager == null)
        {
            gameManager =
                FindAnyObjectByType<GameObjectiveManager>();
        }

        if (playerInventory == null)
        {
            BasicThirdPersonPlayer player =
                FindAnyObjectByType<BasicThirdPersonPlayer>();

            if (player != null)
            {
                playerInventory =
                    player.GetComponent<PlayerInventory>();
            }
        }

        if (fletcherShopkeeper == null)
        {
            fletcherShopkeeper =
                FindAnyObjectByType<FletcherShopkeeper>();
        }
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }
}