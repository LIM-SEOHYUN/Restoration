using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 인스턴스

    [Header("Player")]
    public int playerHealth = 3000;       // 현재 플레이어 체력
    public int maxPlayerHealth = 3000;    // 최대 플레이어 체력
    public float rightSkillCooldown = 0f; // 우클릭 스킬 쿨타임
    public float ultimateCooldown = 0f;   // 궁극기 쿨타임

    [Header("Player etc.")]
    public Camera mainCamera;             // 메인 카메라 참조
    public AudioSource audioSource;       // 오디오 소스
    public GameObject cutsceneUI;         // 컷씬 UI

    [Header("Scene Change")]
    public FadeInOut fadeUI;              // 페이드 인/아웃 UI

    void Awake()
    {
        DontDestroyOnLoad(gameObject);    // 씬 전환 시에도 유지

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 중복 방지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 메인 카메라와 오디오 소스 초기화
        GameManager.instance.mainCamera = Camera.main;
        GameManager.instance.audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 현재는 별도 로직 없음
    }

    // 씬 전환 (페이드 아웃 후 로드)
    public void NextScenesString(string nextSceneName)
    {
        StartCoroutine(fadeUI.FadeOutAndLoad(nextSceneName));
    }

    // 게임 상태 초기화
    public void ResetAll()
    {
        playerHealth = maxPlayerHealth; // 플레이어 체력 초기화

        // 몬스터 초기화
        if (MonsterManager.instance != null)
            MonsterManager.instance.ResetMonsters();

        // 보스 초기화
        if (BossHealth.Instance != null)
            BossHealth.Instance.ResetHealth();

        // 플레이어 초기화
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.gameObject.SetActive(true);
            PlayerHealth.Instance.ResetHealth();

            // 플레이어 스킬 쿨타임 초기화
            PlayerAction action = PlayerHealth.Instance.GetComponent<PlayerAction>();
            if (action != null) action.ResetCooldowns();
        }
    }
}
