using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollowTarget : MonoBehaviour
{
    public GameObject target;       // 카메라가 따라갈 대상 (플레이어)
    public float PixelsPerUnit;     // 픽셀 퍼 유닛 (픽셀 단위 보정용)

    void Awake()
    {
        // 씬 전환 시에도 카메라 오브젝트 유지
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 현재는 Start에서 별도 초기화 없음
    }

    void Update()
    {
        // target이 없으면 PlayerHealth 싱글톤을 통해 플레이어 오브젝트를 찾아서 연결
        if (target == null && PlayerHealth.Instance != null)
        {
            target = PlayerHealth.Instance.gameObject;
        }

        // target이 존재하면 카메라 위치를 픽셀 퍼펙트 보정하여 따라감
        if (target != null)
        {
            transform.position = PixelPerfectClamp(target.transform.position, PixelsPerUnit);
        }
    }

    // 픽셀 퍼펙트 보정 함수
    private Vector3 PixelPerfectClamp(Vector3 moveVector, float pixelsPerUnit)
    {
        // 위치를 픽셀 단위로 변환 후 다시 유닛 단위로 환산
        Vector3 vectorInPixels = new Vector3(
            Mathf.CeilToInt(moveVector.x * pixelsPerUnit),
            Mathf.CeilToInt(moveVector.y * pixelsPerUnit),
            Mathf.CeilToInt(moveVector.z * pixelsPerUnit)
        );
        return vectorInPixels / pixelsPerUnit;
    }
}
