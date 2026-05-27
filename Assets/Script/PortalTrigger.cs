using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public DialogueUI dialogueUI;//대사창 UI 연결
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected at portal!");

            if (MonsterManager.instance.AllMonstersDead())
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                dialogueUI.ShowMessage("아직 활성화되지 않은 것 같다.. 남아있는 호위병들을 다 잡고 다시 와보자.");
            }
        }
    }
}
