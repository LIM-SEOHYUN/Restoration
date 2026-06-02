using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyHealth enemyHealth;          // 적 체력 스크립트 참조
    public Slider healthSlider;              // 체력바 UI
    public Transform enemy;                  // 적 Transform (위치 추적)
    public Vector3 offset = new Vector3(0, 1f, 0); // 체력바 위치 오프셋 (적 머리 위)

    private Camera mainCamera;               // 메인 카메라 참조

    void Start()
    {
        mainCamera = Camera.main;

        // 체력바 초기화
        healthSlider.maxValue = enemyHealth.maxHealth;
        healthSlider.value = enemyHealth.currentHealth;

        if (enemyHealth != null && healthSlider != null)
        {
            healthSlider.value = enemyHealth.currentHealth;

            // 적 체력이 0 이하라면 체력바 비활성화
            if (enemyHealth.currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // 체력바 값 갱신
        healthSlider.value = enemyHealth.currentHealth;

        // 적 위치를 화면 좌표로 변환하여 체력바 위치 갱신
        Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.position + offset);
        healthSlider.transform.position = screenPos;

        // 적이 죽으면 체력바 숨김
        if (enemyHealth.currentHealth <= 0)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }
}
