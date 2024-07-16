using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_DialogueBehavior : StaticSerializedMonoBehaviour<UI_DialogueBehavior>
{
    //============================================
    //
    // [싱글턴 오브젝트]
    // 대사창 UI 표시를 관리하는 클래스입니다.
    // 코루틴 부분은 Sequence 에서만 사용하세요!
    // 
    //============================================

    [SerializeField] private float textInterval = 0.05f;            // 텍스트 출력 시간 간격

    [Title("References")]
    [SerializeField] private GameObject visualGroup;
    [SerializeField] private TextMeshProUGUI context;
    [SerializeField] private GameObject inputWaitObject;
    [SerializeField] private DOTweenAnimation visualGroupAnim;

    private bool dialogueOpened = false;
    public bool DialogueOpened { get { return dialogueOpened; } }   // 현재 대사창 열렸는지 여부

    bool dialogueProceed = false;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        inputWaitObject.SetActive(false);
        visualGroup.SetActive(false);
        visualGroup.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Update()
    {
        if(dialogueOpened)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                if (!dialogueProceed)
                    dialogueProceed = true;
            }    
        }
    }

    /// <summary>
    /// Sequence 전용 : 대사창 코루틴
    /// </summary>
    /// <param name="dialogues"> 대사 데이터 </param>
    /// <returns></returns>
    public IEnumerator Cor_DialogueSequence(string[] dialogues)
    {
        visualGroup.SetActive(true);
        ClearDialogue();
        if (!dialogueOpened)
        {
            yield return StartCoroutine(Cor_OpenDialogue());
        }
        yield return StartCoroutine(Cor_TypeDialogue(dialogues));
    }

    private IEnumerator Cor_TypeDialogue(string[] dialogues)
    {
        for (int i = 0; i < dialogues.Length; i++)
        {
            inputWaitObject.SetActive(false);
            dialogueProceed = false;

            string ctx = dialogues[i];

            for (int j = 0; j <= ctx.Length; j++)
            {
                if (dialogueProceed == true)
                { context.text = ctx; break; }
                context.text = ctx.Substring(0,j);
                yield return new WaitForSeconds(textInterval);
            }

            dialogueProceed = false;

            inputWaitObject.SetActive(true);

            yield return new WaitUntil(() => dialogueProceed);
            inputWaitObject.SetActive(false);
        }
    }

    private IEnumerator Cor_OpenDialogue()
    {
        if (dialogueOpened) yield break;

        visualGroupAnim.DORestartById("DialogueFadein");
        Tween openTw = visualGroupAnim.GetTweens()[0];

        yield return openTw.WaitForCompletion();
        dialogueOpened = true;
    }

    /// <summary>
    /// Sequence 전용 : 대사창 닫기 코루틴
    /// </summary>
    /// <returns></returns>
    public IEnumerator Cor_CloseDialogue()
    {
        if (!dialogueOpened) yield break;

        dialogueOpened = false;
        visualGroupAnim.DORestartById("DialogueFadeout");
        Tween closeTw = visualGroupAnim.GetTweens()[1];
        inputWaitObject.SetActive(false);

        yield return closeTw.WaitForCompletion();

        visualGroup.SetActive(false);

    }

    private void ClearDialogue()
    {
        context.text = string.Empty;
    }
}
