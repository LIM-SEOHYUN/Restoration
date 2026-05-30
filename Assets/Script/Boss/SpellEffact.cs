using UnityEngine;

public class SpellEffect : MonoBehaviour
{
    public int damage = 200;
    public float radius = 5f;

    private bool hasDealtDamage = false;
    private BoxCollider2D col;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip spawnClip;
    public AudioClip impactClip;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.enabled = false;
    }

    public void PlaySpawnSound() => PlaySound(spawnClip);
    public void PlayImpactSound() => PlaySound(impactClip);

    public void ApplyDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            if (!hasDealtDamage && hit.CompareTag("Player"))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null) ph.TakeDamage(damage);
                hasDealtDamage = true;
                PlayImpactSound();
            }
        }
    }

    public void EnableCollider()
    {
        hasDealtDamage = false;
        col.enabled = true;
    }

    public void DisableCollider()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasDealtDamage && other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(damage);
            hasDealtDamage = true;
            col.enabled = false;
            PlayImpactSound();
        }
        else
        {
            Debug.LogWarning("Player collided but PlayerHealth component not found!");
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip);
    }
}
