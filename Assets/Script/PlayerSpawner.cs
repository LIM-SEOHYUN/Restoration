using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    void OnEnable()
    {
        // 씬이 로드될 때 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 씬 언로드 시 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // PlayerSpawn 태그가 붙은 오브젝트 찾기
        GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
        // Player 태그가 붙은 오브젝트 찾기
        GameObject player = GameObject.FindWithTag("Player");

        // 플레이어가 존재하면 스폰 위치로 이동
        if (spawn != null && player != null)
            player.transform.position = spawn.transform.position;

        // CameraSpawn 태그가 붙은 오브젝트 찾기
        GameObject camSpawn = GameObject.FindWithTag("CameraSpawn");
        // 메인 카메라가 존재하면 스폰 위치로 이동
        if (camSpawn != null && Camera.main != null)
            Camera.main.transform.position = camSpawn.transform.position;
    }
}
