using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Optional button selected when the pause menu opens.")]
    [SerializeField] private GameObject firstSelectedButton;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        SetPaused(false);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        // Another gameplay script may try to capture the cursor.
        // Keep overriding it while the pause screen is open.
        if (IsPaused)
        {
            KeepCursorFree();
        }
    }

    private void LateUpdate()
    {
        // LateUpdate runs after most camera/player scripts,
        // so this wins any cursor-lock conflict.
        if (IsPaused)
        {
            KeepCursorFree();
        }
    }

    public void TogglePause()
    {
        SetPaused(!IsPaused);
    }

    public void ResumeGame()
    {
        SetPaused(false);
    }

    public void ReturnToMainMenu()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(
            mainMenuSceneName,
            LoadSceneMode.Single
        );
    }

    public void ExitGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;

        if (pausePanel != null)
        {
            pausePanel.SetActive(paused);
        }

        Time.timeScale = paused ? 0f : 1f;

        if (paused)
        {
            KeepCursorFree();

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);

                if (firstSelectedButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(
                        firstSelectedButton
                    );
                }
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    private void KeepCursorFree()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }
}