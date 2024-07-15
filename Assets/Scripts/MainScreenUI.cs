using Unity.VisualScripting;
using UnityEngine;

public class MainScreenUI : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    public bool standbyForInput;

    private void Update()
    {
        if (standbyForInput)
        {
            if (Input.anyKeyDown == true)
            {
                standbyForInput = false;
                StartGame();
            }
        }
    }

    public void EnableInput()
    {
        standbyForInput = true;
    }

    public void StartGame()
    {
        SceneLoader.LoadNewScene(nextSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
