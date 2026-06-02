using UnityEngine;

public class MainCameraNotDestroy : MonoBehaviour
{
    void Awake()
    {
        // 씬이 바뀌어도 카메라 오브젝트가 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);
    }

    // Start는 첫 프레임 실행 전에 호출되지만 현재는 별도 로직 없음
    void Start()
    {

    }

    // Update는 매 프레임마다 호출되지만 현재는 별도 로직 없음
    void Update()
    {

    }
}
