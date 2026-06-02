using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용

public class StatusUI : MonoBehaviour
{
    public Slider hpBar;                // 체력바 UI
    public TextMeshProUGUI hpText;      // 체력 텍스트 UI

    void Start()
    {
        // 현재는 Start에서 별도 초기화 없음
    }

    void Update()
    {
        // 현재는 Update에서 별도 로직 없음
    }

    // 체력 UI 갱신 함수
    public void UpdateHP(float currentHp, float maxHp)
    {
        hpBar.maxValue = maxHp;                     // 체력바 최대값 설정
        hpBar.value = currentHp;                    // 현재 체력 값 반영
        hpText.text = currentHp + " / " + maxHp;    // 텍스트로 현재/최대 체력 표시
    }
}
