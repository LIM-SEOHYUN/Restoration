using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public DialogueUI dialogueUI;   // 대사창 UI 연결
    public string nextSceneName;    // 이동할 다음 씬 이름

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter: " + other.name);

        // 플레이어가 포탈에 닿았을 때만 실행
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected at portal!");

            // 모든 몬스터가 죽었는지 확인
            if (MonsterManager.instance.AllMonstersDead())
            {
                // 조건 충족 → 다음 씬으로 이동
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                // 조건 미충족 → 대사창에 메시지 출력
                dialogueUI.ShowMessage("아직 활성화되지 않은 것 같다.. 남아있는 호위병들을 다 잡고 다시 와보자.");
            }
        }
    }
}
