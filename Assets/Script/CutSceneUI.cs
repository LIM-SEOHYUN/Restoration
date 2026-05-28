using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneUI : MonoBehaviour
{

    private static CutsceneUI instance;

    public RectTransform cutsceneImage; //컷씬 위치 크기 제어
    public CanvasGroup canvasGroup; //알파값 제어

    public Vector2 hiddenPos = new Vector2(-500, -300); 
    public Vector2 visiblePos = new Vector2(-660, -150);//화면 안 도착 위치

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowCutscene()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true); //궁극기 시전 시 컷씬 등장
        StartCoroutine(SlideAndFade()); //슬라이드 + 페이드
    }

    IEnumerator SlideAndFade()
    {
        float t = 0f;
        canvasGroup.alpha = 0f;
        cutsceneImage.anchoredPosition = hiddenPos;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            cutsceneImage.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);//알파값을 조절하여 평소에는 드러나지 않게
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.5f;
            cutsceneImage.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
