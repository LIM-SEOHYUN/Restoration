using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance; // 싱글톤 인스턴스

    void Awake()
    {
        // 인스턴스가 아직 없으면 현재 오브젝트를 인스턴스로 지정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 방지를 위해 파괴
            Destroy(gameObject);
        }
    }
}
