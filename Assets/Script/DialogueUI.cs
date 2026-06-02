using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // 대사 텍스트 표시용
    public CanvasGroup canvasGroup;      // 알파값 제어 (페이드 인/아웃)

    // 메시지를 표시하는 함수
    public void ShowMessage(string message)
    {
        dialogueText.text = message;     // 텍스트에 메시지 설정
        StartCoroutine(FadeOut());       // 페이드 아웃 코루틴 실행
    }

    // 페이드 아웃 코루틴
    IEnumerator FadeOut()
    {
        canvasGroup.alpha = 1f;          // 처음엔 완전히 보이도록 설정
        yield return new WaitForSeconds(1f); // 1초 동안 유지

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;         // 시간 경과
            canvasGroup.alpha = 1f - t;  // 알파값 감소 → 점점 투명해짐
            yield return null;           // 다음 프레임까지 대기
        }
    }
}
