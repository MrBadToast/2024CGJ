using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IntroSequence 
{
    public virtual IEnumerator Cor_Sequence()
    {
        yield return null;
    }
}

public class Seq_Dialogue : IntroSequence
{
    [TextArea]public string[] contexts;
    public bool closeAfterFinish = false;

    public override IEnumerator Cor_Sequence()
    {
        yield return UI_DialogueBehavior.Instance.StartCoroutine(UI_DialogueBehavior.Instance.Cor_DialogueSequence(contexts));

        if(closeAfterFinish)
            yield return UI_DialogueBehavior.Instance.StartCoroutine(UI_DialogueBehavior.Instance.Cor_CloseDialogue());
    }
}

public class Seq_Wait : IntroSequence
{
    public float time = 1.0f;

    public override IEnumerator Cor_Sequence()
    {
        yield return new WaitForSeconds(time);
    }
}

public class Seq_Event : IntroSequence
{
    public UnityEvent events;

    public override IEnumerator Cor_Sequence()
    {
        events.Invoke();

        yield return null;
    }
}

public class GameplayManager : SerializedMonoBehaviour
{
    public float initialTimelimit = 30f;
    public float bonusSeconds = 10f;
    public float humaneDiminishSpeed = 0.1f;
    public float cureOnHit = 33f;
    public float minimumBarInterval = 0.5f;
    public int rareOpponentRate = 5;

    public IntroSequence[] introSequences;
    public OpponentData[] common_opponents;
    public OpponentData[] rare_opponents;
    public Color[] opponent_colors;
    public Color PickRandomColor { get { return opponent_colors[UnityEngine.Random.Range(0,opponent_colors.Length)]; } }

    public SpriteRenderer playerSprite;
    public Animator playerAnimator;
    public OpponentBehavior opponentBehavior;
    public Slider humaneGuageUI;
    public TextMeshProUGUI timerUI;
    public GameObject hiddenOppUI;
    public GameObject resultUI;

    static public Dictionary<OpponentData, bool> rare_opponent_collected;

    public bool mainGameInProgress = false;
    bool opponentActive = false;
    float currentTime = 0f;
    float currentHumaneMeter = 0f;
    public int cured = 0;

    private void Awake()
    {
        if (rare_opponent_collected == null)
            rare_opponent_collected = new Dictionary<OpponentData, bool>();
    }

    private void Start()
    {
        if (rare_opponent_collected.Count == 0)
        {
            foreach (var rare in rare_opponents)
            {
                rare_opponent_collected.Add(rare, false);
            }
        }

        StartGameSequences();
    }

    float bartimer = 0f;
    float nextBarInterval = 0f;

    private void Update()
    {
        if (mainGameInProgress)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                currentTime -= Time.deltaTime * 10f;
            }

            if (opponentActive)
            {
                if (bartimer > nextBarInterval)
                {
                    PowerBar.Instance.SpawnBar();
                    bartimer = 0f;
                    nextBarInterval = minimumBarInterval * UnityEngine.Random.Range(1, 3);

                }
                else
                {
                    bartimer += Time.deltaTime;
                }
            }
        }
    }

    public void OnHitCorrect()
    {
        if (opponentActive)
        {
            opponentBehavior.DamageOpponent();
            currentHumaneMeter += cureOnHit;
            humaneGuageUI.value = currentHumaneMeter;
        }
    }

    public void OnHitMissed()
    {
        if (cured > 16) currentHumaneMeter -= cureOnHit / 2f;
    }

    public void StartGameSequences()
    {
        StartCoroutine(Cor_IntroSequences());
    }

    private IEnumerator Cor_MainGame()
    {
        mainGameInProgress = true;
        currentTime = initialTimelimit;
        opponentActive = true;

        while (currentTime > 0f)
        {
            if (currentHumaneMeter > 1.0f)
            {
                opponentActive = false;
                yield return StartCoroutine(Cor_OnOpponentNeutralized());
                currentHumaneMeter = 0.0f;
                opponentActive = true;
            }
            else if (currentHumaneMeter > 0f)
                currentHumaneMeter -= Time.deltaTime * humaneDiminishSpeed;
            else
                currentHumaneMeter = 0f;

            currentTime -= Time.deltaTime;
            timerUI.text = ((int)currentTime).ToString("00") + ":" + ((int)((currentTime % 1.0f) * 100f)).ToString("00"); //((Math.Truncate(currentTime * 100) / 100f) % 1f).ToString(); 

            humaneGuageUI.value = currentHumaneMeter;

            yield return null;
        }

        mainGameInProgress = false;

        Time.timeScale = 1.0f;
        currentTime = 0f;
        resultUI.SetActive(true);
    }

    private IEnumerator Cor_OnOpponentNeutralized()
    {
        Debug.Log("OpponentNeutrailized");

        currentTime += bonusSeconds;
        cured++;
        if (cured > 4) Time.timeScale = 1.25f;
        else if (cured > 8) Time.timeScale = 1.5f;
        else if (cured > 16) Time.timeScale = 2f;
        else Time.timeScale = 1.0f;

        if (currentTime > 60.00f) currentTime = 60.00f;

        opponentBehavior.NeutralizeOpponent();
        yield return new WaitForSeconds(1f);
        OpponentData opp = GetRandomOpponent();
        opponentBehavior.SetNewOpponent(opp, opp.rareOpponent ? Color.white : PickRandomColor);
        if(opp.rareOpponent)
        {
            if (!rare_opponent_collected[opp]) hiddenOppUI.SetActive(true);
            rare_opponent_collected[opp] = true;
        }
        yield return new WaitForSeconds(1f);
        yield return null;
    }

    private IEnumerator Cor_IntroSequences()
    {
        for(int i = 0; i < introSequences.Length; i++)
        {
            yield return StartCoroutine(introSequences[i].Cor_Sequence());
        }

        StartCoroutine(Cor_MainGame());
    }

    private OpponentData GetRandomOpponent()
    {
        bool isRare = UnityEngine.Random.Range(0, rareOpponentRate) == 0;
        if (cured < 5) isRare = false;

        if(isRare)
        {
            return rare_opponents[UnityEngine.Random.Range(0, rare_opponents.Length)];
        }
        else
        {
            return common_opponents[UnityEngine.Random.Range(0, common_opponents.Length)];
        }
    }
}
