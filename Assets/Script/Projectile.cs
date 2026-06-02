using UnityEngine;

public class Projectile : MonoBehaviour // 치명타 설정
{
    public int baseDamage = 10;                  // 기본 데미지
    public float criticalChance = 0.2f;          // 치명타 확률 (20%)
    public float minCriticalMultiplier = 1.1f;   // 치명타 배율 최소값
    public float maxCriticalMultiplier = 2f;     // 치명타 배율 최대값

    public GameObject damagePopupPrefab;         // 데미지 팝업 프리팹

    void Awake()
    {
        DontDestroyOnLoad(gameObject);           // 씬 전환 시에도 유지
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌 대상: " + collision.name);

        // EnemyHealth 컴포넌트가 있는 오브젝트만 처리
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth == null) return;

        // 치명타 여부 결정
        bool isCritical = Random.value < criticalChance;
        int finalDamage = baseDamage;

        if (isCritical)
        {
            // 치명타 배율 랜덤 적용
            float multiplier = Random.Range(minCriticalMultiplier, maxCriticalMultiplier);
            finalDamage = Mathf.RoundToInt(baseDamage * multiplier);
        }

        // 적에게 데미지 적용
        enemyHealth.TakeDamage(finalDamage);

        // 데미지 팝업 생성
        if (damagePopupPrefab == null)
        {
            Debug.LogError("damagePopupPrefab 연결 안 됨!");
            return;
        }

        // 팝업 위치 (적 위치 + 오프셋)
        Vector3 spawnPos = enemyHealth.transform.position + new Vector3(0, enemyHealth.popupOffsetY, 0);

        GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

        // DamagePopup 스크립트 가져와서 설정
        DamagePopup dp = popup.GetComponentInChildren<DamagePopup>();
        if (dp != null)
        {
            dp.Setup(finalDamage, isCritical);
        }
        else
        {
            Debug.LogError("DamagePopup 스크립트를 찾을 수 없음!");
        }

        // 투사체 파괴
        Destroy(gameObject);
    }
}
