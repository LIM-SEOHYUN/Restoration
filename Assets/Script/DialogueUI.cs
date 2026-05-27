using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public CanvasGroup canvasGroup;

    public void ShowMessage(string message)
    {
        dialogueText.text = message;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(1f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - t;
            yield return null;
        }
    }
}
