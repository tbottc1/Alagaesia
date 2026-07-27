using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public sealed class MainMenuController : MonoBehaviour
{
    [Header("Scene")]
    [Tooltip(
        "Enter the exact name of the gameplay scene, " +
        "without the .unity extension."
    )]
    [SerializeField]
    private string gameSceneName =
        "Tutorial_for_Alagaesia";

    private bool isLoadingGame;

    public void PlayGame()
    {
        if (isLoadingGame)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(gameSceneName))
        {
            Debug.LogError(
                "MainMenuController is missing the gameplay scene name.",
                this
            );

            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(
                gameSceneName))
        {
            Debug.LogError(
                "Unity cannot load the scene '" +
                gameSceneName +
                "'. Make sure it is included in the active " +
                "Build Profile Scene List and that the name matches exactly.",
                this
            );

            return;
        }

        isLoadingGame = true;

        // Protect against entering gameplay while an older scene
        // or menu left the game paused.
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            gameSceneName,
            LoadSceneMode.Single
        );
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // Lets the Exit button visibly work while testing in Unity.
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}