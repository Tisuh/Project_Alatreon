using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void LoadLevel(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
