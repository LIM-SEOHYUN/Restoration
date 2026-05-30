using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float wanderRadius = 80f;
    public float detectRange = 12f;
    public float attackRange = 10f;

    public float castCooldown = 20f;
    private float lastCastTime;

    public Animator animator;
    public Transform player;
    public GameObject spellPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 walkDirection;
    private Vector3 wanderTarget;

    public float spellSpawnRangeX = 50f;
    public float spellSpawnHeight = 10f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip statueDisappearClip;
    public AudioClip spellCastClip;
    public AudioClip attackClip;
    public AudioClip hurtClip;
    public AudioClip bossAttackClip;
    public AudioClip bossCastClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        ChooseNewWanderTarget();
        walkDirection = new Vector2(Random.value > 0.5f ? 1f : -1f, 0f);
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // ±Ã±Ø±â ÄðÅ¸ÀÓ Ã¼Å©
        if (Time.time - lastCastTime >= castCooldown)
        {
            animator.SetTrigger("isCast");
            lastCastTime = Time.time;
        }

        if (dist <= detectRange) // ÃßÀû
        {
            Vector2 targetPos = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.deltaTime);
            rb.MovePosition(targetPos);

            animator.SetBool("isWalking", true);
            sr.flipX = player.position.x < transform.position.x;

            if (dist <= attackRange) // °ø°Ý
            {
                animator.SetTrigger("isAttacking");
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            Wander();
        }
    }

    void Wander()
    {
        if (Vector2.Distance(transform.position, wanderTarget) < 1f)
        {
            ChooseNewWanderTarget();
        }

        Vector2 targetPos = Vector2.MoveTowards(rb.position, wanderTarget, moveSpeed * Time.deltaTime);
        rb.MovePosition(targetPos);

        animator.SetBool("isWalking", true);
        sr.flipX = wanderTarget.x < transform.position.x;
    }

    void ChooseNewWanderTarget()
    {
        wanderTarget = new Vector2(
            Random.Range(-wanderRadius, wanderRadius),
            transform.position.y
        );
    }
    public void EndAttack()
    {
        animator.ResetTrigger("isAttacking");
        animator.SetBool("isWalking", true);
    }

    public void EndCast()
    {
        animator.ResetTrigger("isCast");
        animator.SetBool("isWalking", true);
    }
    public void CastSpell()
    {
        PlaySound(spellCastClip);
        for (int i = 0; i < 15; i++)
        {
            Vector3 pos = new Vector3(
            Random.Range(-spellSpawnRangeX, spellSpawnRangeX),
            spellSpawnHeight,
            0
             );
            GameObject spell = Instantiate(spellPrefab, pos, Quaternion.identity);
            Destroy(spell, 3f);
        }
    }

    public void DealDamage()
    {
        PlaySound(attackClip);
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(100);
            animator.SetBool("isWalking", true);
        }
    }

    public void TakeDamage(int dmg) => PlaySound(hurtClip);
    public void PlayBossCastSound() => PlaySound(bossCastClip);
    public void PlayBossAttackSound() => PlaySound(bossAttackClip);
    public void PlayStatueDisappearSound() => PlaySound(statueDisappearClip);

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
