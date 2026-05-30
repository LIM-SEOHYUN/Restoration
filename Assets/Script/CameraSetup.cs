using UnityEngine;
using Cinemachine;

public class CameraSetup : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (vcam.Follow == null || vcam.LookAt == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                vcam.Follow = player.transform;
                vcam.LookAt = player.transform;
            }
        }
    }
}
