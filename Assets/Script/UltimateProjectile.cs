using UnityEngine;

public class UltimateProjectile : MonoBehaviour
{
    public int damage = 50;
    public float speed = 20f;
    public float lifeTime = 2f;
    public GameObject poisonAreaPrefab;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Invoke(nameof(SpawnPoisonAndDestroy), lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            SpawnPoisonAndDestroy();
        }
    }

    private void SpawnPoisonAndDestroy()
    {
        if (poisonAreaPrefab != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Ground"));
            Vector3 spawnPos = transform.position;

            if (hit.collider != null)
            {
                spawnPos = hit.point + new Vector2(0f, 0.5f);
            }

            Instantiate(poisonAreaPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
