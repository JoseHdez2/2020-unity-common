using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartSceneManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && Debug.isDebugBuild)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
