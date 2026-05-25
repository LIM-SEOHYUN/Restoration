using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 100;
    public int maxHp = 100;
    public PlayerMoving playerMoving;
    private Animator animator;
    private bool isDead = false;
    public StatusUI statusUI;

    [Header("Color")]
    private SpriteRenderer sr;
    private Color originalColor;

    [Header("invincible")]
    private bool isInvincible = false; //무적 여부
    public float invincibleDuration = 1f;//무적 유지시간

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

        statusUI.UpdateHP(hp, maxHp);//체력 UI 갱신

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
        playerMoving.currentState = PlayerState.Dead;
        animator.SetTrigger("isDead");

        if (playerMoving.currentState == PlayerState.Ultimate)
        {
            animator.SetBool("isUltimate", true); //궁극기 사망 애니메이션
        }
        else
        {
            animator.SetBool("isUltimate", false); //일반 사망 애니메이션
        }

        // 이동/스킬 입력 막기
        playerMoving.enabled = false;
        GetComponent<PlayerAction>().enabled = false;
    }
    IEnumerator DamageFlash()//피해를 입었을 때 깜빡거림
    {
        for (int i = 0; i < 2; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = originalColor;
            yield return new WaitForSeconds(0.1f);

        }

        IEnumerator DamageRoutine()
        {
            isInvincible = true;
            for (int i = 0; i < 2; i++)
            {
                sr.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                sr.color = originalColor;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(invincibleDuration);//무적 유지 시간
            isInvincible = false;
        }

    }
}
