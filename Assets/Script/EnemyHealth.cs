using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header ("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public float popupOffsetY = 1.5f;



    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("isHit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        Debug.Log(gameObject.name + " died!");
        Destroy(gameObject, 1f);
    }
    public int CurrentHealth => currentHealth;


}
