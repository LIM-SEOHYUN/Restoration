using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public int damage =50;
    public float speed = 10f;
    public float lifeTime = 1.5f; //이펙트 유지 시간

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Boss"))
        {
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }


    }
    public void SetDirection(Vector2 dir)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * speed;
        Destroy(gameObject, lifeTime);
    }
}
