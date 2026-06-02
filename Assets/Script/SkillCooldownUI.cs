using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("Image")]
    public Image mask;                   // 쿨타임 마스크 (아이콘 덮는 이미지)
    public TextMeshProUGUI cooldownText; // 쿨타임 남은 시간 표시 텍스트
    public Image icon;                   // 스킬 아이콘 이미지
    private Outline outline;             // 아이콘 테두리 효과

    private float cooldownTime;          // 전체 쿨타임 시간
    private float cooldownTimer;         // 남은 쿨타임 시간
    private bool isCooldown;             // 현재 쿨타임 진행 여부

    private void Awake() // 초기 상태 설정
    {
        outline = icon.GetComponent<Outline>();

        mask.enabled = false;            // 기본적으로 마스크 숨김
        cooldownText.enabled = false;    // 기본적으로 쿨타임 텍스트 숨김
        if (outline != null) outline.enabled = false; // 테두리 효과 숨김
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime; // 매 프레임마다 쿨타임 감소
            if (cooldownTimer <= 0)
            {
                cooldownTimer = 0;
                isCooldown = false;
                mask.enabled = false;        // 쿨타임 종료 → 마스크 숨김
                cooldownText.enabled = false;// 쿨타임 종료 → 텍스트 숨김
            }

            // 마스크 채워지는 정도 (fillAmount)로 쿨타임 진행 표시
            mask.fillAmount = cooldownTimer / cooldownTime;

            // 남은 시간 텍스트 표시 (올림 처리)
            cooldownText.text = isCooldown ? Mathf.Ceil(cooldownTimer).ToString() : "";
        }
    }

    // 현재 쿨타임 여부 확인
    public bool IsOnCooldown()
    {
        return isCooldown;
    }

    // 아이콘 테두리 효과 켜기/끄기
    public void SetGlow(bool active)
    {
        if (outline != null)
        {
            outline.enabled = active;
        }
        if (active) // 테두리 켜질 때는 마스크와 텍스트 숨김
        {
            mask.enabled = false;
            cooldownText.enabled = false;
        }
    }

    // 쿨타임 시작
    public void StartCooldown(float time)
    {
        cooldownTime = time;
        cooldownTimer = time;
        isCooldown = true;

        mask.enabled = true;             // 마스크 표시
        cooldownText.enabled = true;     // 텍스트 표시
        if (outline != null) outline.enabled = false; // 테두리 효과는 꺼짐
    }

    // UI 초기화 (쿨타임 종료 상태로 되돌림)
    public void ResetUI()
    {
        isCooldown = false;
        cooldownTimer = 0f;
        mask.enabled = false;
        cooldownText.enabled = false;
        if (outline != null) outline.enabled = false;
    }
}
