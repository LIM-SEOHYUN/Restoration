using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public int contactDamage = 10;

    private Transform player;
    private Animator animator;
    private SpriteRenderer sr;
    private EnemyHealth enemyHealth;
    private Rigidbody2D rb;

    private bool isDead = false;
    private Vector2 walkDirection;

    public Transform attackPoint;
    public Vector2 attackSize = new Vector2(1f, 1f);
    public LayerMask playerLayer;

    private float lastDamageTime;
    public float damageInterval = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();

        // y축은 0으로 고정 → 위로 올라가는 문제 방지
        walkDirection = new Vector2(Random.Range(-1f, 1f), 0).normalized;
    }

    void Update()
    {
        if (isDead) return;

        if (enemyHealth.currentHealth <= 0)
        {
            Die();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            animator.SetBool("isAttacking", true);

            if (Time.time - lastDamageTime >= damageInterval)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(contactDamage);
                    lastDamageTime = Time.time;
                }
            }

            sr.flipX = player.position.x < transform.position.x;
        }
        else
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", true);

            rb.MovePosition(rb.position + walkDirection * moveSpeed * Time.deltaTime);

            if (walkDirection.x < 0) sr.flipX = true;
            else if (walkDirection.x > 0) sr.flipX = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        enemyHealth.TakeDamage(damage);
        animator.SetTrigger("isHit"); // 피격 모션 전용 Trigger
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        Destroy(gameObject, 1f);
    }

    // 애니메이션 이벤트에서 호출되는 공격 판정
    public void DealDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapBox(attackPoint.position, attackSize, 0f, playerLayer);
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
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }
}
