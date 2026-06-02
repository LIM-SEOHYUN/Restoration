using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private Image image;          // 페이드 효과를 적용할 UI 이미지
    [SerializeField] private float fadeTime = 1f;  // 페이드 지속 시간
    [SerializeField] private AnimationCurve _fadeCurve; // 페이드 속도 곡선 (자연스러운 전환)

    // 씬 전환 시 페이드 아웃 → 씬 로드 → 페이드 인
    public IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(0, 1);                   // 화면을 어둡게 (투명 → 불투명)
        SceneManager.LoadScene(sceneName);         // 씬 로드
        yield return null;
        StartCoroutine(Fade(1, 0));                // 화면을 밝게 (불투명 → 투명)
    }

    // 페이드 처리 (start → end)
    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            // 이미지 알파값 변경 (곡선 기반)
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, _fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }
    }
}
