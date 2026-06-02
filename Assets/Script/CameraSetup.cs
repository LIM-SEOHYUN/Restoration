using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    private CinemachineVirtualCamera vcam; // Cinemachine 가상 카메라 참조

    void Awake()
    {
        // 현재 오브젝트에서 CinemachineVirtualCamera 컴포넌트 가져오기
        vcam = GetComponent<CinemachineVirtualCamera>();

        // 씬 전환 시에도 카메라 오브젝트 유지
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // 카메라가 따라갈 대상(Follow, LookAt)이 비어있으면 플레이어를 찾아서 연결
        if (vcam.Follow == null || vcam.LookAt == null)
        {
            GameObject player = GameObject.FindWithTag("Player"); // 태그 "Player"로 플레이어 찾기
            if (player != null)
            {
                vcam.Follow = player.transform; // 카메라가 플레이어를 따라가도록 설정
                vcam.LookAt = player.transform; // 카메라가 플레이어를 바라보도록 설정
            }
        }
    }
}
