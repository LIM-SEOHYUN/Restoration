using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollowTarget : MonoBehaviour
{
    public GameObject target;
    public float PixelsPerUnit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PixelPerfectClamp(target.transform.position, PixelsPerUnit);
    }

    private Vector3 PixelPerfectClamp(Vector3 moveVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(moveVector.x * pixelsPerUnit), Mathf.CeilToInt(moveVector.y * pixelsPerUnit), Mathf.CeilToInt(moveVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
}
