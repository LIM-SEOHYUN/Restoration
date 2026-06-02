using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public int damage = 50;            // 공격 이펙트가 주는 데미지
    public float speed = 10f;          // 투사체 속도
    public float lifeTime = 1.5f;      // 이펙트 유지 시간 (초)

    private Rigidbody2D rb;            // 물리 이동 제어용 Rigidbody2D

    void Awake()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 일정 시간 뒤 자동 파괴 (lifeTime 초 후)
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 현재는 Update에서 별도 로직 없음
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적(Enemy)과 충돌했을 때
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // 적 체력 감소
            }
            Destroy(gameObject); // 투사체 파괴
        }

        // 보스(Boss)와 충돌했을 때
        if (other.CompareTag("Boss"))
        {
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage); // 보스 체력 감소
            }
            Destroy(gameObject); // 투사체 파괴
        }
    }

    // 투사체 방향과 속도 설정
    public void SetDirection(Vector2 dir)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * speed; // 지정된 방향으로 속도 적용
        Destroy(gameObject, lifeTime); // lifeTime 후 자동 파괴
    }
}
