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
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();

        walkDirection = new Vector2(Random.Range(-1f, 1f), 0).normalized;
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        if (enemyHealth == null || enemyHealth.currentHealth <= 0)
        {
            Die();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            animator.SetBool("isAttacking", true);
            sr.flipX = player.position.x < transform.position.x;
        }
        else
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", true);

            rb.MovePosition(rb.position + walkDirection * moveSpeed * Time.deltaTime);

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

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        enemyHealth.TakeDamage(damage);
        animator.SetTrigger("isAttaking");
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        this.enabled = false;
        Destroy(gameObject, 1f);
    }

    public void DealDamage()
    {
        Debug.Log("DealDamage called");
        Collider2D hitPlayer = Physics2D.OverlapBox(attackPoint.position, attackSize, 0f, playerLayer);
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
