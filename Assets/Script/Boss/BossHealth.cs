using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    // 보스 체력 관련 변수
    public int hp = 10000;          // 현재 체력
    public int maxHp = 10000;       // 최대 체력
    public Slider hpBar;            // 체력바 UI
    public Animator animator;       // 보스 애니메이터
    public TextMeshProUGUI hpText;  // 체력 텍스트 표시

    // 싱글톤 패턴 (보스 체력 관리 전역 접근)
    public static BossHealth Instance;

    void Awake()
    {
        // 싱글톤 인스턴스 초기화
        Instance = this;
    }

    void Start()
    {
        // 시작 시 체력바와 텍스트 초기화
        hpBar.value = 1f; // 체력바를 가득 채움
        hpText.text = $"{hp}/{maxHp}"; // 체력 텍스트 표시
    }

    // 데미지를 입었을 때 호출
    public void TakeDamage(int dmg)
    {
        hp -= dmg;                  // 체력 감소
        if (hp < 0) hp = 0;         // 체력이 음수가 되지 않도록 보정

        // UI 업데이트
        hpBar.value = (float)hp / maxHp;
        hpText.text = $"{hp}/{maxHp}";

        if (hp <= 0) // 체력이 0 이하 → 사망 처리
        {
            animator.SetTrigger("isDeath"); // 죽음 애니메이션 실행
            GameOverUI.Instance.ShowGameOver(); // 게임 오버 UI 표시
        }
        else // 체력이 남아있으면 피격 애니메이션 실행
        {
            animator.SetTrigger("isHurt");
        }
    }

    // 보스 체력 초기화 (재시작 시 사용)
    public void ResetHealth()
    {
        hp = maxHp;                 // 체력 최대치로 회복
        hpBar.value = 1f;           // 체력바 가득 채움
        hpText.text = $"{hp}/{maxHp}"; // 텍스트 업데이트
    }
}
