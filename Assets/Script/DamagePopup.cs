using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI text;       // 데미지 텍스트 표시용
    public float moveUpSpeed = 1f;     // 위로 올라가는 속도
    public float fadeSpeed = 2f;       // 페이드 아웃 속도

    private Color textColor;           // 텍스트 색상(알파값 포함)

    void Start()
    {
        textColor = text.color;        // 초기 텍스트 색상 저장
        Destroy(gameObject, 1f);       // 1초 뒤 자동 파괴
    }

    void Update()
    {
        // 위로 이동
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // 알파값 감소 → 점점 투명해짐
        textColor.a -= fadeSpeed * Time.deltaTime;
        text.color = textColor;

        // 완전히 투명해지면 즉시 파괴
        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 팝업 초기 설정 (데미지 값, 치명타 여부)
    public void Setup(int damage, bool isCritical)
    {
        text.text = damage.ToString(); // 데미지 숫자 표시

        if (isCritical)
        {
            text.color = Color.red;    // 치명타는 빨간색
            text.fontSize += 10;       // 글씨 크기 크게
        }
        else
        {
            text.color = Color.white;  // 일반 공격은 흰색
        }
    }
}
