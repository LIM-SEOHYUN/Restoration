using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;
    public GameObject gameOverPanel;

    void Awake()
    {

        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // 상태 초기화
        GameManager.instance.ResetAll();

        UnityEngine.SceneManagement.SceneManager.LoadScene("Cave");
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.ResetHealth();

        gameOverPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


}
