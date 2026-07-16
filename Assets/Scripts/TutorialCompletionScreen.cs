using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialCompletionScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObjectiveManager gameManager;

    [SerializeField]
    private GameObject completionPanel;

    [SerializeField]
    private CanvasGroup completionCanvasGroup;

    [SerializeField]
    private TextMeshProUGUI completionText;

    [SerializeField]
    private TextMeshProUGUI restartText;

    [Header("Completion Message")]
    [TextArea(4, 8)]
    [SerializeField]
    private string completionMessage =
        "Tutorial completed!\n\n" +
        "You are now ready to get your dragon.\n\n" +
        "To Be Continued....";

    [SerializeField]
    private string restartMessage =
        "Press SPACE to restart";

    [Header("Appearance")]
    [SerializeField]
    private float fadeDuration = 1.25f;

    [Header("Game Behavior")]
    [SerializeField]
    private bool pauseGameOnCompletion = true;

    private bool tutorialCompleted;
    private bool restartReady;

    private void Awake()
    {
        // Prevent the scene from remaining paused after restarting.
        Time.timeScale = 1f;

        FindGameManager();

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
        if (!tutorialCompleted)
        {
            CheckForTutorialCompletion();
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

    private void CheckForTutorialCompletion()
    {
        if (gameManager == null)
        {
            FindGameManager();

            if (gameManager == null)
            {
                return;
            }
        }

        bool archeryFinished =
            gameManager.archeryTrainingComplete;

        bool deerHuntFinished =
            gameManager.deerCollected >=
            gameManager.deerRequired;

        if (archeryFinished && deerHuntFinished)
        {
            StartCoroutine(ShowCompletionScreen());
        }
    }

    private IEnumerator ShowCompletionScreen()
    {
        tutorialCompleted = true;

        if (completionText != null)
        {
            completionText.text = completionMessage;
        }

        if (restartText != null)
        {
            restartText.text = restartMessage;
        }

        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (pauseGameOnCompletion)
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
            elapsedTime += Time.unscaledDeltaTime;

            completionCanvasGroup.alpha =
                Mathf.Clamp01(
                    elapsedTime / fadeDuration
                );

            yield return null;
        }

        completionCanvasGroup.alpha = 1f;
        restartReady = true;
    }

    private void FindGameManager()
    {
        if (gameManager == null)
        {
            gameManager =
                FindAnyObjectByType<GameObjectiveManager>();
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