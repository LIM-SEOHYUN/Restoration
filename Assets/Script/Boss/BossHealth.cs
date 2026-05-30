using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int hp = 10000;
    public int maxHp = 10000;
    public Slider hpBar;
    public Animator animator;
    public TextMeshProUGUI hpText;

    public static BossHealth Instance;


    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
        hpBar.value = 1f;
        hpText.text = $"{hp}/{maxHp}";//½Ì±ÛÅæ »ç¿ë
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp < 0) hp = 0;
        hpBar.value = (float)hp / maxHp;
        hpText.text = $"{hp}/{maxHp}";

        if (hp <= 0)
        {
            animator.SetTrigger("isDeath");
            GameOverUI.Instance.ShowGameOver();
        }
        else
        {
            animator.SetTrigger("isHurt");
        }
    }
    public void ResetHealth()
    {
        hp = maxHp;
        hpBar.value = 1f;
        hpText.text = $"{hp}/{maxHp}";
    }
}
