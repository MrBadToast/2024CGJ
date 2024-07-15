using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : PersistentSerializedMonoBehaviour<SceneLoader>
{
    [SerializeField] private GameObject visualGroup;
    [SerializeField] private DOTweenAnimation fadeTween;

    bool loadingInProgress = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public static void LoadNewScene(string sceneName)
    {
        if (Instance.loadingInProgress) return;

        Instance.StartCoroutine(Instance.Cor_LoadNewScene(sceneName));
    }

    private IEnumerator Cor_LoadNewScene(string sceneName)
    {
        loadingInProgress = true;

        visualGroup.SetActive(true);

        Tween tw;

        fadeTween.DORestartById("LoaderFadein");
        tw = fadeTween.GetTweens()[0];
        yield return tw.WaitForCompletion();

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        
        while (!load.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        fadeTween.DORestartById("LoaderFadeout");
        tw = fadeTween.GetTweens()[1];
        yield return tw.WaitForCompletion();

        visualGroup.SetActive(false);

        loadingInProgress = false;
    }
}
