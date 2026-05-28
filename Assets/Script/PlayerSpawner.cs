using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject spawn = GameObject.FindWithTag("PlayerSpawn");
        GameObject player = GameObject.FindWithTag("Player");
        if (spawn != null && player != null)
            player.transform.position = spawn.transform.position;

        GameObject camSpawn = GameObject.FindWithTag("CameraSpawn");
        if (camSpawn != null && Camera.main != null)
            Camera.main.transform.position = camSpawn.transform.position;
    }

}
