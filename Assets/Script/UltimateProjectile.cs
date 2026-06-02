using UnityEngine;

public class UltimateProjectile : MonoBehaviour
{
    public int damage = 50;                  // 투사체가 주는 데미지
    public float speed = 20f;                // 투사체 속도
    public float lifeTime = 2f;              // 투사체 유지 시간
    public GameObject poisonAreaPrefab;      // 투사체가 사라질 때 생성할 독 구역 프리팹

    private Rigidbody2D rb;                  // 물리 이동 제어용 Rigidbody2D

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);       // 씬 전환 시에도 유지
    }

    void Start()
    {
        // lifeTime 후 독 구역 생성 및 투사체 파괴
        Invoke(nameof(SpawnPoisonAndDestroy), lifeTime);
    }

    // 투사체 방향과 속도 설정
    public void SetDirection(Vector2 dir)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * speed;
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 적(Enemy)과 충돌 시
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // 적 체력 감소
            }
            Destroy(gameObject); // 투사체 파괴
        }

        // 보스(Boss)와 충돌 시
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

    // 독 구역 생성 후 투사체 파괴
    private void SpawnPoisonAndDestroy()
    {
        if (poisonAreaPrefab != null)
        {
            // 투사체 위치 기준으로 아래 방향으로 Raycast → 땅 위치 확인
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Ground"));
            Vector3 spawnPos = transform.position;

            if (hit.collider != null)
            {
                // 땅에 맞으면 그 위치에 독 구역 생성
                spawnPos = hit.point + new Vector2(0f, 0.5f);
            }

            Instantiate(poisonAreaPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject); // 투사체 파괴
    }
}
