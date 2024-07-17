using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public int rareOpponentRate = 5;

    public IntroSequence[] introSequences;
    public OpponentData[] common_opponents;
    public OpponentData[] rare_opponents;
    public Color[] opponent_colors;

    Dictionary<OpponentData, bool> rare_opponent_collected;

    [SerializeField,ReadOnly()] bool skipTutorial = false;
    bool tutorialInProgress = false;

    private void Awake()
    {
        rare_opponent_collected = new Dictionary<OpponentData, bool>();
    }

    private void Start()
    {
        foreach (var rare in rare_opponents)
        {
            rare_opponent_collected.Add(rare, false);
        }

        StartGameSequences();
    }

    private void Update()
    {
        if (tutorialInProgress)
        {
            if (skipTutorial)
            {
                StopAllCoroutines();
                tutorialInProgress = false;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    skipTutorial = true;
                }
            }
        }
    }

    public void StartGameSequences()
    {
        StartCoroutine(Cor_IntroSequences());
    }

    private IEnumerator Cor_MainGame()
    {
        yield return null;
    }

    private IEnumerator Cor_IntroSequences()
    {
        tutorialInProgress = true;
        for(int i = 0; i < introSequences.Length; i++)
        {
            yield return StartCoroutine(introSequences[i].Cor_Sequence());
        }
        tutorialInProgress = false;
    }
}
