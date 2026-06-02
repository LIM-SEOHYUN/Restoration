using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;             // 최대 체력
    public int currentHealth;               // 현재 체력
    public float popupOffsetY = 1.5f;       // 데미지 팝업 위치 오프셋

    [Header("Audio")]
    public AudioSource audioSource;         // 오디오 소스
    public AudioClip deathSound;            // 사망 효과음

    private Animator animator;              // 애니메이터

    void Start()
    {
        currentHealth = maxHealth;          // 시작 시 체력 초기화
        animator = GetComponent<Animator>(); // 애니메이터 참조
    }

    void Update()
    {
        // 현재는 별도 로직 없음
    }

    // 데미지 처리
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;            // 체력 감소
        animator.SetTrigger("isHit");       // 피격 애니메이션 실행

        if (currentHealth <= 0)
        {
            Die();                          // 체력이 0 이하 → 사망 처리
        }
    }

    // 사망 처리
    void Die()
    {
        animator.SetBool("isDead", true);   // 사망 애니메이션 실행
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject, 1f);            // 1초 후 오브젝트 제거
    }

    // 현재 체력 반환 (읽기 전용 프로퍼티)
    public int CurrentHealth => currentHealth;
}
