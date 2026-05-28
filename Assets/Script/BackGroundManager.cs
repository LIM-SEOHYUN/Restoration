using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private float length, startpos;
    public float parallaxFactor;
    public GameObject cam;
    public float PixelsPerUnit;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (cam == null)
        {
            GameObject vcam = GameObject.Find("Virtual Camera");
            if (vcam != null)
                cam = vcam;
            else
                Debug.LogError("Virtual Camera 'CM vcam1'을 찾을 수 없습니다!");
        }

        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        if (cam == null) return;

        float temp = cam.transform.position.x * (1 - parallaxFactor);
        float distance = cam.transform.position.x * parallaxFactor;
        Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        transform.position = newPosition;

        if (temp > startpos + (length / 2))
        {
            startpos += length;
        }
        else if (temp < startpos - (length / 2))
        {
            startpos -= length;
        }
    }

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
