using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    private static PlayerPersistence instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//씬 넘어가도 플레이어 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
