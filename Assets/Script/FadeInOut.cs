using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private AnimationCurve _fadeCurve;

    public IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(0, 1);
        SceneManager.LoadScene(sceneName);
        yield return null;
        StartCoroutine(Fade(1, 0));
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0f;
        float percent = 0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, _fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;
        }
    }
}
