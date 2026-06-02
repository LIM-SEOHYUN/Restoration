using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;   // 싱글톤 인스턴스
    public GameObject gameOverPanel;     // 게임 오버 패널 UI

    void Awake()
    {
        // 씬 전환 시에도 유지
        DontDestroyOnLoad(gameObject);

        // 싱글톤 패턴 적용
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

        // 시작 시 게임 오버 패널 비활성화
        gameOverPanel.SetActive(false);
    }

    // 게임 오버 패널 표시
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // 게임 재시작
    public void RestartGame()
    {
        // 게임 상태 초기화
        GameManager.instance.ResetAll();

        // Cave 씬 다시 로드
        SceneManager.LoadScene("Cave");

        // 플레이어 체력 초기화
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.ResetHealth();

        // 게임 오버 패널 숨김
        gameOverPanel.SetActive(false);
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        // 에디터 실행 중일 경우 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
