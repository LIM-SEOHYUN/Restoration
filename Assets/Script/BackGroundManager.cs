using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private float length, startpos;       // 배경 이미지 길이와 시작 위치
    public float parallaxFactor;          // 패럴랙스 효과 비율 (카메라 이동 대비 배경 이동 정도)
    public GameObject cam;                // 카메라 참조
    public float PixelsPerUnit;           // 픽셀 퍼 유닛 (픽셀 단위 보정용)

    void Awake()
    {
        // 씬 전환 시에도 배경 오브젝트 유지
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 카메라가 지정되지 않았다면 "Virtual Camera"라는 이름의 오브젝트를 찾아서 연결
        if (cam == null)
        {
            GameObject vcam = GameObject.Find("Virtual Camera");
            if (vcam != null)
                cam = vcam;
            else
                Debug.LogError("Virtual Camera 'CM vcam1'을 찾을 수 없습니다!");
        }

        // 배경 시작 위치와 길이 계산
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        if (cam == null) return;

        // 카메라 이동에 따른 배경 위치 계산
        float temp = cam.transform.position.x * (1 - parallaxFactor);
        float distance = cam.transform.position.x * parallaxFactor;
        Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        // 배경 이동 적용
        transform.position = newPosition;

        // 배경이 화면을 벗어나면 위치를 보정하여 무한 스크롤처럼 이어지게 함
        if (temp > startpos + (length / 2))
        {
            startpos += length;
        }
        else if (temp < startpos - (length / 2))
        {
            startpos -= length;
        }
    }

    // 픽셀 단위로 위치를 보정하여 픽셀 퍼펙트 렌더링을 유지
    private Vector3 PixelPerfectClamp(Vector3 locationVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(
            Mathf.CeilToInt(locationVector.x * pixelsPerUnit),
            Mathf.CeilToInt(locationVector.y * pixelsPerUnit),
            Mathf.CeilToInt(locationVector.z * pixelsPerUnit)
        );
        return vectorInPixels / pixelsPerUnit;
    }
}
