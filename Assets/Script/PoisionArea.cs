using UnityEngine;
using System.Collections;

public class PoisonArea : MonoBehaviour
{
    public int poisonDamage = 5;
    public float duration = 5f;//유지시간
    public int tickCount = 3;
    public float tickInterval = 0.5f;


    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(ApplyPoison(other.GetComponent<EnemyHealth>()));
        }
    }

    IEnumerator ApplyPoison(EnemyHealth enemy)
    {
        for (int i = 0; i < tickCount; i++)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(poisonDamage);
            }
            yield return new WaitForSeconds(tickInterval);
        }
    }
}
