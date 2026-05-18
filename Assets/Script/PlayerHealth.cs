using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hp = 100;
    public PlayerMoving playerMoving;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        playerMoving.currentState = PlayerState.Dead;
        animator.SetTrigger("isDead");

        if (playerMoving.currentState == PlayerState.Ultimate)
        {
            animator.SetBool("isUltimate", true); // 궁극기 사망 애니메이션
        }
        else
        {
            animator.SetBool("isUltimate", false); // 일반 사망 애니메이션
        }

        // 이동/스킬 입력 막기
        playerMoving.enabled = false;
        GetComponent<PlayerAction>().enabled = false;
    }
}
