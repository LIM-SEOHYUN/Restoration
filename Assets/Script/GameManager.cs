using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public int playerHealth = 1000;
    public float rightSkillCooldown = 0f;
    public float ultimateCooldown = 0f;

    [Header("Player etc.")]
    public Camera mainCamera;
    public AudioSource audioSource;
    public GameObject cutsceneUI;

    void Awake()
    {
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
}
