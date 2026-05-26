using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    public int hp = 100;
    public int maxHp = 100;

    public PlayerMoving playerMoving;

    private Animator animator;
    private bool isDead = false;
    public StatusUI statusUI;

    public float popupOffsetY = 1.5f;

    [Header("Color")]
    private SpriteRenderer sr;
    private Color originalColor;

    [Header("invincible")]
    private bool isInvincible = false;
    public float invincibleDuration = 1f;

    public GameObject damagePopupPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        statusUI.UpdateHP(hp, maxHp);
    }



    public void TakeDamage(int damage)
    {
        if (isDead || isInvincible) return;

        hp -= damage;
        if (hp < 0) hp = 0;

        statusUI.UpdateHP(hp, maxHp);

        if (damagePopupPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, popupOffsetY, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);

            DamagePopup dp = popup.GetComponentInChildren<DamagePopup>();
            if (dp != null)
            {
                dp.Setup(damage, false);
            }
        }

        if (hp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageFlash());
        }
    }

    void Die()
    {
        isDead = true;

        if (playerMoving.currentState == PlayerState.Ultimate)
        {
            animator.SetBool("isUltimate", true);
        }
        else
        {
            animator.SetBool("isUltimate", false);
        }

        playerMoving.currentState = PlayerState.Dead;
        animator.SetTrigger("isDead");

        playerMoving.enabled = false;
        GetComponent<PlayerAction>().enabled = false;

        Destroy(gameObject, 2f);
    }

    IEnumerator DamageFlash()
    {
        isInvincible = true;
        for (int i = 0; i < 2; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }
}
