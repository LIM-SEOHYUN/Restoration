using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneUI : MonoBehaviour
{
    private static CutsceneUI instance; // 싱글톤 인스턴스

    public RectTransform cutsceneImage; // 컷씬 이미지 위치/크기 제어
    public CanvasGroup canvasGroup;     // 알파값 제어 (페이드 인/아웃)

    // 컷씬 시작/종료 위치
    public Vector2 hiddenPos = new Vector2(-500, -300);
    public Vector2 visiblePos = new Vector2(-660, -150); // 화면 안 도착 위치

    void Awake()
    {
        // 싱글톤 패턴 적용: 중복 생성 방지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // 씬 전환 시에도 오브젝트 유지
        DontDestroyOnLoad(gameObject);
    }

    // 컷씬 표시 함수 (궁극기 시전 시 호출)
    public void ShowCutscene()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true); // 비활성화 상태면 활성화
        StartCoroutine(SlideAndFade()); // 슬라이드 + 페이드 연출 시작
    }

    // 슬라이드 + 페이드 연출 코루틴
    IEnumerator SlideAndFade()
    {
        float t = 0f;
        canvasGroup.alpha = 0f; // 처음엔 투명
        cutsceneImage.anchoredPosition = hiddenPos; // 시작 위치는 화면 밖

        // 등장 애니메이션 (0.5초 동안)
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            cutsceneImage.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t); // 알파값 증가 → 페이드 인
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); // 잠깐 머무름

        // 퇴장 애니메이션 (0.5초 동안)
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            cutsceneImage.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t); // 알파값 감소 → 페이드 아웃
            yield return null;
        }

        // 컷씬 종료 후 비활성화
        gameObject.SetActive(false);
    }
}
