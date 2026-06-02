using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatueTrigger : MonoBehaviour
{
    public GameObject boss;       // 트리거 후 활성화할 보스 오브젝트
    public GameObject bossHpUI;   // 트리거 후 활성화할 보스 HP UI

    SpriteRenderer sr;            // 석상 스프라이트 렌더러

    private bool triggered = false; // 트리거가 이미 발동했는지 여부 체크

    private void Awake()
    {
        // 스프라이트 렌더러 컴포넌트 가져오기
        sr = GetComponent<SpriteRenderer>();
    }

    // 플레이어가 트리거 영역에 들어왔을 때 실행
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true; // 중복 실행 방지
            StartCoroutine(FadeOutStatue()); // 석상 페이드 아웃 시작
        }
    }

    // 석상이 서서히 사라지는 코루틴
    IEnumerator FadeOutStatue()
    {
        float t = 0f;
        Color c = sr.color;

        // 2초 동안 알파값을 1 → 0으로 변경
        while (t < 1f)
        {
            t += Time.deltaTime / 2f;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
            yield return null;
        }

        // 석상 비활성화
        gameObject.SetActive(false);

        // 보스와 HP UI 활성화
        if (boss != null) boss.SetActive(true);
        if (bossHpUI != null) bossHpUI.SetActive(true);
    }
}
