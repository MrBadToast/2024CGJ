using DG.Tweening;
using System.Threading;
using UnityEngine;

public class PowerBar : StaticSerializedMonoBehaviour<PowerBar>
{
    public GameplayManager gameplayManager;
    public GameObject powerBarPrefab;
    public Transform powerBarTFParent;
    public GameObject slashEffect;
    public Transform slashEffectParent;
    public GameObject hitText;
    public GameObject missedText;
    public Transform resultTextParent;
    public AudioClip[] sound_hit;
    public AudioClip[] sound_whoosh;
    public DOTweenAnimation flashAnimation;

    public HitTarget hitTarget;
    public float powerBarSpeed = 1.0f;
    public float powerBarSpawnpoint = 70f;

    public bool DebugSpawn = false;

    AudioSource audiosource;

    protected override void Awake()
    {
        base.Awake();
        audiosource  = GetComponent<AudioSource>();
    }

    int count = 0;

    private void Update()
    {
        if (DebugSpawn)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("BarSpawnedByDebug");
                SpawnBar();
            }
        }

        if (gameplayManager.mainGameInProgress)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (hitTarget.PowerbarDetected())
                {
                    gameplayManager.OnHitCorrect();
                    Instantiate(slashEffect, slashEffectParent);
                    Instantiate(hitText, resultTextParent);
                    count++;

                    flashAnimation.DORestart();

                    audiosource.PlayOneShot(sound_hit[Random.Range(0, sound_hit.Length)]);

                    if (count % 2 == 0)
                        gameplayManager.playerAnimator.Play("Attack1");
                    else
                        gameplayManager.playerAnimator.Play("Attack2");
                }
                else
                {
                    audiosource.PlayOneShot(sound_whoosh[Random.Range(0, sound_whoosh.Length)]);
                    Instantiate(missedText, resultTextParent);
                    gameplayManager.OnHitMissed();
                }
            }
        }
    }

    public void SpawnBar()
    {
        GameObject spawned = Instantiate(powerBarPrefab, powerBarTFParent);
        spawned.transform.localPosition = new Vector3(powerBarSpawnpoint, 0f, 0f);
    }


}
