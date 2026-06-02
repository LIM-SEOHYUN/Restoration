using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    public int hp = 3000;                  // 현재 체력
    public int maxHp = 3000;               // 최대 체력

    public GameObject diedPanel;           // 사망 시 표시할 패널
    public PlayerMoving playerMoving;      // 플레이어 이동 스크립트 참조

    private Animator animator;
    private bool isDead = false;           // 사망 여부
    public StatusUI statusUI;              // 체력 UI 참조

    public float popupOffsetY = 1.5f;      // 데미지 팝업 오프셋

    [Header("Color")]
    private SpriteRenderer sr;             // 플레이어 색상 변경용
    private Color originalColor;           // 원래 색상 저장

    [Header("invincible")]
    private bool isInvincible = false;     // 무적 상태 여부
    public float invincibleDuration = 1f;  // 무적 지속 시간

    public GameObject damagePopupPrefab;   // 데미지 팝업 프리팹

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;           // 사망 효과음
    public AudioClip attackSound;          // 피격 효과음

    public static PlayerHealth Instance;   // 싱글톤 인스턴스

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);     // 씬 전환 시에도 유지
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        statusUI.UpdateHP(hp, maxHp);

        // GameManager에서 체력 불러오기
        hp = GameManager.instance.playerHealth;
        maxHp = 3000;
    }

    // 데미지 처리
    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return;

        hp -= damage;
        if (attackSound != null) audioSource.PlayOneShot(attackSound);
        if (hp < 0) hp = 0;

        GameManager.instance.playerHealth = hp;
        statusUI.UpdateHP(hp, maxHp);

        // 데미지 팝업 표시
        if (damagePopupPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, popupOffsetY, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            DamagePopup dp = popup.GetComponentInChildren<DamagePopup>();
            Destroy(popup, 1f);
            if (dp != null)
            {
                dp.Setup(damage, false);
            }
        }

        // 사망 여부 확인
        if (hp <= 0)
        {
            Die();
            if (GameOverUI.Instance != null)
                GameOverUI.Instance.ShowGameOver();
        }
        else
        {
            StartCoroutine(DamageFlash()); // 피격 시 깜빡임 효과
        }
    }

    // 사망 처리
    void Die()
    {
        isDead = true;
        if (deathSound != null) audioSource.PlayOneShot(deathSound);

        // 궁극기 상태 여부에 따라 애니메이션 설정
        if (playerMoving.currentState == PlayerState.Ultimate)
            animator.SetBool("isUltimate", true);
        else
            animator.SetBool("isUltimate", false);

        playerMoving.currentState = PlayerState.Dead;
        animator.SetTrigger("isDead");

        // 이동 및 행동 불가
        playerMoving.enabled = false;
        GetComponent<PlayerAction>().enabled = false;

        if (diedPanel != null)
            diedPanel.SetActive(true);

        gameObject.SetActive(false);
    }

    // 피격 시 깜빡임 + 무적 시간 적용
    IEnumerator DamageFlash()
    {
        isInvincible = true;
        for (int i = 0; i < 2; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.playerHealth = hp;
        }
    }

    // 체력 초기화 (부활 시 사용)
    public void ResetHealth()
    {
        hp = maxHp;
        statusUI.UpdateHP(hp, maxHp);
        isDead = false;

        gameObject.SetActive(true);

        playerMoving.enabled = true;
        GetComponent<PlayerAction>().enabled = true;

        playerMoving.currentState = PlayerState.Normal;

        animator.ResetTrigger("isDead");
        animator.SetBool("isUltimate", false);
        animator.Play("Player_idle");
    }
}
