using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public GameplayManager gameplayManager;

    private void OnEnable()
    {
        resultText.text = "ġ�Ἲ��:" + gameplayManager.cured.ToString();
    }

    public void OnRestart()
    {
        SceneLoader.LoadNewScene(SceneManager.GetActiveScene().name);
    }
}
