using Unity.VisualScripting;
using UnityEngine;

public class MainScreenUI : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    public void StartGame()
    {
        SceneLoader.LoadNewScene(nextSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
