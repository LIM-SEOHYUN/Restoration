using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{

    public EnemyHealth enemyHealth;
    public Slider healthSlider;
    public Transform enemy;
    public Vector3 offset = new Vector3 (0, 1f, 0);

    private Camera mainCamera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        mainCamera = Camera.main;
        healthSlider.maxValue = enemyHealth.maxHealth;
        healthSlider.value = enemyHealth.currentHealth;
        if (enemyHealth != null && healthSlider != null)
        {
            healthSlider.value = enemyHealth.currentHealth;

            if (enemyHealth.currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = enemyHealth.currentHealth;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.position + offset);
        healthSlider.transform.position = screenPos;

        if (enemyHealth.currentHealth <= 0)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }
}
