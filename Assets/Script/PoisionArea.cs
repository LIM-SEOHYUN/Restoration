using UnityEngine;
using System.Collections;

public class PoisonArea : MonoBehaviour
{
    public int poisonDamage = 5;       // 독 데미지 (틱마다 적용)
    public float duration = 5f;        // 독 구역 유지 시간
    public int tickCount = 3;          // 총 데미지 적용 횟수
    public float tickInterval = 1f;    // 데미지 적용 간격 (초)

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
    }

    void Start()
    {
        // duration 후 자동 파괴
        Destroy(gameObject, duration);
    }

    // 적이 독 구역에 들어왔을 때 실행
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                // 코루틴으로 지속 데미지 적용
                StartCoroutine(ApplyPoison(enemy));
            }
        }
    }

    // 지속 데미지 코루틴
    IEnumerator ApplyPoison(EnemyHealth enemy)
    {
        for (int i = 0; i < tickCount; i++)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(poisonDamage); // 데미지 적용
            }
            yield return new WaitForSeconds(tickInterval); // 간격 대기
        }
    }
}
