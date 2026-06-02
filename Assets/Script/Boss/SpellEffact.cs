using UnityEngine;

public class SpellEffect : MonoBehaviour
{
    public int damage = 200;          // 스킬이 플레이어에게 입히는 데미지
    public float radius = 5f;         // 데미지 적용 범위 (원형 범위)

    private bool hasDealtDamage = false; // 이미 데미지를 준 상태인지 체크
    private BoxCollider2D col;           // 충돌 감지용 박스 콜라이더

    [Header("Sound")]
    public AudioSource audioSource;   // 오디오 소스
    public AudioClip spawnClip;       // 스킬 생성 시 재생할 소리
    public AudioClip impactClip;      // 충돌 시 재생할 소리

    void Awake()
    {
        // BoxCollider2D 초기화 및 트리거 모드 설정
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.enabled = false; // 기본적으로 비활성화
    }

    // 스킬 생성 시 소리 재생
    public void PlaySpawnSound() => PlaySound(spawnClip);

    // 충돌 시 소리 재생
    public void PlayImpactSound() => PlaySound(impactClip);

    // 범위 내 플레이어에게 데미지 적용
    public void ApplyDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            if (!hasDealtDamage && hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null) ph.TakeDamage(damage); // 플레이어 체력 감소
                hasDealtDamage = true; // 한 번만 데미지 적용
                PlayImpactSound();     // 충돌 소리 재생
            }
        }
    }

    // 콜라이더 활성화 (데미지 판정 가능)
    public void EnableCollider()
    {
        hasDealtDamage = false;
        col.enabled = true;
    }

    // 콜라이더 비활성화 (데미지 판정 불가능)
    public void DisableCollider()
    {
        col.enabled = false;
    }

    // 트리거 충돌 시 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasDealtDamage && other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage); // 플레이어 체력 감소
            hasDealtDamage = true;
            col.enabled = false; // 충돌 후 콜라이더 비활성화
            PlayImpactSound();   // 충돌 소리 재생
        }
        else
        {
            Debug.LogWarning("Player collided but PlayerHealth component not found!");
        }
    }

    // 사운드 재생 공통 함수
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
