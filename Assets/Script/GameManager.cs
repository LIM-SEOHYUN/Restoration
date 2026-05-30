using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public int playerHealth = 3000;
    public int maxPlayerHealth = 3000;
    public float rightSkillCooldown = 0f;
    public float ultimateCooldown = 0f;

    [Header("Player etc.")]
    public Camera mainCamera;
    public AudioSource audioSource;
    public GameObject cutsceneUI;

    [Header("Scene Change")]
    public FadeInOut fadeUI;

    void Awake()
    {
            DontDestroyOnLoad(gameObject);


        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);//중복 방지
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.mainCamera = Camera.main;
        GameManager.instance.audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextScenesString(string nextSceneName)
    {
        StartCoroutine(fadeUI.FadeOutAndLoad(nextSceneName));
    }

    public void ResetAll()
    {
        playerHealth = maxPlayerHealth;

        if (MonsterManager.instance != null)
            MonsterManager.instance.ResetMonsters();

        if (BossHealth.Instance != null)
            BossHealth.Instance.ResetHealth();

        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.gameObject.SetActive(true);
            PlayerHealth.Instance.ResetHealth();

            PlayerAction action = PlayerHealth.Instance.GetComponent<PlayerAction>();
            if (action != null) action.ResetCooldowns();
        }
    }



}
