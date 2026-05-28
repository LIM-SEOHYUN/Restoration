using UnityEngine;

public class Projectile : MonoBehaviour //치명타 설정
{
    public int baseDamage = 10;
    public float criticalChance = 0.2f;//치명타 확률
    public float minCriticalMultiplier = 1.1f;//치명타 배율 1.1~2
    public float maxCriticalMultiplier = 2f;



    public GameObject damagePopupPrefab;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌 대상: " + collision.name);


        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth == null) return;

        bool isCritical = Random.value < criticalChance;
        int finalDamage = baseDamage;

        if (isCritical)
        {
            float multiplier = Random.Range(minCriticalMultiplier, maxCriticalMultiplier);
            finalDamage = Mathf.RoundToInt(baseDamage * multiplier);
        }

        enemyHealth.TakeDamage(finalDamage);

        if (damagePopupPrefab == null)
        {
            Debug.LogError("damagePopupPrefab 연결 안 됨!");
            return;
        }

        Vector3 spawnPos = enemyHealth.transform.position + new Vector3(0, enemyHealth.popupOffsetY, 0);

        GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

        DamagePopup dp = popup.GetComponentInChildren<DamagePopup>();
        if (dp != null)
        {
            dp.Setup(finalDamage, isCritical);
        }
        else
        {
            Debug.LogError("DamagePopup 스크립트를 찾을 수 없음!");
        }

        dp.Setup(finalDamage, isCritical);

        Destroy(gameObject);
    }

}
