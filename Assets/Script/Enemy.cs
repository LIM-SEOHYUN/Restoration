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
    public Vector2 attackOffset = new Vector2(1f, 0f);
    public LayerMask playerLayer; //플레이어 레이어 선택

    private float lastDamageTime;
    public float damageInterval = 1f;

    private float stateTimer;
    private float stateDuration = 3f;
    private bool isIdle = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip deathSound;

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

        MonsterManager.instance.RegisterMonster();


        walkDirection = new Vector2(Random.value > 0.5f ? 1f : -1f, 0f);
    }

    void FixedUpdate()
    {
        if (isDead) return;
        if (player == null) return;

        if (enemyHealth == null || enemyHealth.currentHealth <= 0)
        {
            Die();
            return;
        }

        if (isDead) return;

        stateTimer += Time.deltaTime;
        if (stateTimer >= stateDuration)
        {
            // 50% 확률로 Idle 전환
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
        if (deathSound != null) audioSource.PlayOneShot(deathSound);
        this.enabled = false;
        MonsterManager.instance.MonsterDied();
        Destroy(gameObject, 1f);
    }

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

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Vector2 boxCenter = attackPoint.position
                          + (Vector3)attackPoint.right * attackOffset.x
                          + (Vector3)attackPoint.up * attackOffset.y;
        Gizmos.DrawWireCube(boxCenter, attackSize);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            float newX = Random.value > 0.5f ? 1f : -1f;
            walkDirection = new Vector2(newX, 0f).normalized;
        }
    }


}
