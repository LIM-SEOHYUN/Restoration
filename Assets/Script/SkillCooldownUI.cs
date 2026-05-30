using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("Image")]
    public Image mask;
    public TextMeshProUGUI cooldownText;
    public Image icon;
    private Outline outline;

    private float cooldownTime;
    private float cooldownTimer;
    private bool isCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() //시작상태
    {
        outline = icon.GetComponent<Outline>();

        mask.enabled = false;
        cooldownText.enabled = false;
        if (outline != null) outline.enabled = false;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer <= 0)
            {
                cooldownTimer = 0;
                isCooldown = false;
                mask.enabled = false;
                cooldownText.enabled=false;
            }
            mask.fillAmount = cooldownTimer / cooldownTime;
            cooldownText.text = isCooldown ? Mathf.Ceil(cooldownTimer).ToString() : "";
        }
    }

    public bool IsOnCooldown()
    {
        return isCooldown;
    }

    public void SetGlow(bool active)
    {
        if (outline != null)
        {
            outline.enabled = active;
        }
        if (active) //쿨타임 도는 동안에는 마스크와 쿨타임 텍스트가 꺼짐
        {
            mask.enabled = false;
            cooldownText.enabled = false;
        }
    }
    public void StartCooldown(float time)
    {
        cooldownTime = time;
        cooldownTimer = time;
        isCooldown = true;
        mask.enabled = true;
        cooldownText.enabled = true;
        if (outline != null) outline.enabled = false;
    }

    void OnDestroy()
    {

    }

    public void ResetUI()
    {
        isCooldown = false;
        cooldownTimer = 0f;
        mask.enabled = false;
        cooldownText.enabled = false;
        if (outline != null) outline.enabled = false;
    }

}
