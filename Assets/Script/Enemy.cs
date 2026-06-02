using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;                  // 이동 속도
    public float detectionRange = 5f;             // 플레이어 탐지 범위
    public int contactDamage = 10;                // 접촉 시 데미지

    private Transform player;                     // 플레이어 위치 참조
    private Animator animator;                    // 애니메이터
    private SpriteRenderer sr;                    // 방향 전환용
    private EnemyHealth enemyHealth;              // 적 체력 관리
    private Rigidbody2D rb;                       // 물리 이동 제어

    private bool isDead = false;                  // 사망 여부
    private Vector2 walkDirection;                // 이동 방향

    public Transform attackPoint;                 // 공격 위치
    public Vector2 attackSize = new Vector2(1f, 1f); // 공격 범위 크기
    public Vector2 attackOffset = new Vector2(1f, 0f); // 공격 범위 오프셋
    public LayerMask playerLayer;                 // 플레이어 레이어

    private float lastDamageTime;                 // 마지막 공격 시간
    public float damageInterval = 1f;             // 공격 간격

    private float stateTimer;                     // 상태 전환 타이머
    private float stateDuration = 3f;             // 상태 유지 시간
    private bool isIdle = false;                  // Idle 상태 여부

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;                 // 공격 사운드
    public AudioClip deathSound;                  // 사망 사운드

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();

        // 몬스터 등록 (생존 수 증가)
        MonsterManager.instance.RegisterMonster();

        // 초기 이동 방향 랜덤 설정
        walkDirection = new Vector2(Random.value > 0.5f ? 1f : -1f, 0f);
    }

    void FixedUpdate()
    {
        if (isDead) return;
        if (player == null) return;

        // 체력이 0 이하라면 사망 처리
        if (enemyHealth == null || enemyHealth.currentHealth <= 0)
        {
            Die();
            return;
        }

        // 상태 전환 (Idle ↔ Walking)
        stateTimer += Time.deltaTime;
        if (stateTimer >= stateDuration)
        {
            isIdle = Random.value > 0.5f;
            if (!isIdle)
                walkDirection = new Vector2(Random.value > 0.5f ? 1f : -1f, 0f);
            stateTimer = 0f;
        }

        if (isIdle)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            rb.MovePosition(rb.position + walkDirection * moveSpeed * Time.deltaTime);
            sr.flipX = walkDirection.x < 0;
        }

        // 플레이어 탐지
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // 공격 상태
            animator.SetBool("isAttacking", true);
            sr.flipX = player.position.x < transform.position.x;
        }
        else
        {
            // 걷기 상태
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", true);

            rb.MovePosition(rb.position + walkDirection * moveSpeed * Time.deltaTime);

            // 방향에 따라 공격 위치 갱신
            if (walkDirection.x < 0)
            {
                sr.flipX = true;
                if (attackPoint != null)
                {
                    attackPoint.localPosition = new Vector3(
                        -Mathf.Abs(attackPoint.localPosition.x),
                        attackPoint.localPosition.y,
                        attackPoint.localPosition.z
                    );
                }
            }
            else if (walkDirection.x > 0)
            {
                sr.flipX = false;
                if (attackPoint != null)
                {
                    attackPoint.localPosition = new Vector3(
                        Mathf.Abs(attackPoint.localPosition.x),
                        attackPoint.localPosition.y,
                        attackPoint.localPosition.z
                    );
                }
            }
        }
    }

    // 외부에서 데미지 받기
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        enemyHealth.TakeDamage(damage);
        animator.SetTrigger("isAttaking"); // 오타: "isAttacking"이 맞을 듯
    }

    // 사망 처리
    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        if (deathSound != null) audioSource.PlayOneShot(deathSound);
        this.enabled = false;
        MonsterManager.instance.MonsterDied(); // 몬스터 수 감소
        Destroy(gameObject, 1f);
    }

    // 플레이어에게 데미지 주기
    public void DealDamage()
    {
        float offsetX = sr.flipX ? -attackOffset.x : attackOffset.x;
        Vector2 boxCenter = attackPoint.position + new Vector3(offsetX, attackOffset.y, 0);

        Collider2D hitPlayer = Physics2D.OverlapBox(boxCenter, attackSize, 0f, playerLayer);
        if (hitPlayer != null)
        {
            Debug.Log("Player detected: " + hitPlayer.name);
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Applying damage: " + contactDamage);
                playerHealth.TakeDamage(contactDamage);
            }
        }
        else
        {
            Debug.Log("No player in range");
        }

        // 일정 간격으로만 데미지 적용
        if (hitPlayer != null)
        {
            if (Time.time - lastDamageTime >= damageInterval)
            {
                PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(contactDamage);
                    lastDamageTime = Time.time;
                }
            }
            if (attackSound != null) audioSource.PlayOneShot(attackSound);
        }
    }

    // 공격 범위 Gizmo 표시
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Vector2 boxCenter = attackPoint.position
                          + (Vector3)attackPoint.right * attackOffset.x
                          + (Vector3)attackPoint.up * attackOffset.y;
        Gizmos.DrawWireCube(boxCenter, attackSize);
    }

    // 벽 충돌 시 방향 전환
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            float newX = Random.value > 0.5f ? 1f : -1f;
            walkDirection = new Vector2(newX, 0f).normalized;
        }
    }
}
