using TMPro;
using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class StatueTrigger : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossHpUI;

    SpriteRenderer sr;

    private bool triggered = false;

    private void Awake()
    {
        
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(FadeOutStatue());
        }
    }

    IEnumerator FadeOutStatue()
    {
        float t = 0f;
        Color c = sr.color;

        while (t < 1f)
        {
            t += Time.deltaTime / 2f;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
            yield return null;
        }

        gameObject.SetActive(false);

        if (boss != null) boss.SetActive(true);
        if (bossHpUI != null) bossHpUI.SetActive(true);
    }
}
