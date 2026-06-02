using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    private static PlayerPersistence instance; // 싱글톤 인스턴스

    void Awake()
    {
        // 인스턴스가 없으면 현재 오브젝트를 인스턴스로 지정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않음
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 방지를 위해 파괴
            Destroy(gameObject);
        }
    }
}
