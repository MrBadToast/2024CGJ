using UnityEngine;

public class HiddenResult : MonoBehaviour
{
    public GameObject[] windows;
    public OpponentData[] hiddens;

    private void OnEnable()
    {
        for(int i = 0; i < windows.Length; i++)
        {
            if(GameplayManager.rare_opponent_collected[hiddens[i]])
            {
                windows[i].SetActive(true);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) gameObject.SetActive(false);
    }
}
