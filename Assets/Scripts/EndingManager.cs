using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] Image whiteBackground;
    [SerializeField] Image blackBackground;
    [SerializeField] Transform player;
    [SerializeField] Animator playerAnim;
    [SerializeField] Transform door;
    [SerializeField] Transform god;
    [SerializeField] SpriteRenderer godSpriter;
    [SerializeField] Animator godAnim;
    [SerializeField] SpriteRenderer godBackground;
    [SerializeField] SpriteRenderer wifeSpriter;
    [SerializeField] GameObject textBox;
    [SerializeField] TextMeshProUGUI playerTMP;
    [SerializeField] TextMeshProUGUI godTMP;
    [SerializeField] GameObject playerTextTail;
    [SerializeField] GameObject godTextTail;
    [SerializeField] GameObject wifeTextTail;
    [SerializeField] float textAppendTime = 0.1f;
    [SerializeField] float nextTextWaitTime = 2.0f;

    [Header("Step1")]
    [SerializeField] float step1BackgroundFadeTime = 1.0f;
    [SerializeField] float step1WaitTime = 2.0f;
    [SerializeField] float step1MoveTime = 1.0f;
    [SerializeField] float step1GodBackgroundFadeTime = 1.0f;

    [Header("Step2")]
    [SerializeField] float step2waitTime = 1.0f;
    [SerializeField] string[] dialogue1;
    [SerializeField] string[] dialogue2;
    [SerializeField] string[] dialogue3;

    [Header("Step3")]
    [SerializeField] float step3WaitTime = 0.5f;
    [SerializeField] float step3PlayerFadeTime = 1.0f;
    [SerializeField] float step3GodFadeTime = 1.0f;
    [SerializeField] float step3WaitTime2 = 1.0f;
    [SerializeField] string[] dialogue4;
    [SerializeField] string[] dialogue5;

    [Header("Step4")]
    [SerializeField] float step4WifeFadeTime = 1.0f;
    [SerializeField] float step4BackgroundFadeTime = 1.0f;
    [SerializeField] string[] dialogue6;


    bool skipPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EndingSequenceCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            skipPressed = true;
    }

    IEnumerator EndingSequenceCoroutine()
    {
        // Step1 - Meet the God
        // white background fade out
        // wait for some seconds
        // player walk right(3.2), door and God move left(-1.8)
        // All stop moving, God background fade
        yield return new WaitForSeconds(step1WaitTime + 1f);

        playerAnim.SetBool("Walk", true);
        var seq1 = DOTween.Sequence();
        seq1.Append(player.DOLocalMoveX(-0.5f, step1MoveTime).SetEase(Ease.Linear));
        seq1.Join(door.DOLocalMoveX(-6f, step1MoveTime).SetEase(Ease.Linear));
        seq1.Join(god.DOLocalMoveX(0.5f, step1MoveTime).SetEase(Ease.Linear));
        yield return seq1.WaitForCompletion();

        playerAnim.SetBool("Walk", false);
        var t2 = godBackground.DOFade(0f, step1GodBackgroundFadeTime);
        yield return t2.WaitForCompletion();


        // Step2 - Conversation
        // proceed dialogue sequence1
        // player and God walk right, wait for some seconds
        // proceed dialogue sequence2
        // fade in black
        // proceed dialogue sequence3
        // fade out balck
        yield return StartCoroutine(dialogueCoroutine(dialogue1));

        playerAnim.SetBool("Walk", true);
        god.eulerAngles = new Vector3(0f, 0f, 0f);
        godAnim.SetBool("Walk", true);
        yield return new WaitForSeconds(step2waitTime);

        yield return StartCoroutine(dialogueCoroutine(dialogue2));

        var t3 = blackBackground.DOFade(1f, 0.3f);
        yield return t3.WaitForCompletion();

        yield return StartCoroutine(dialogueCoroutine(dialogue3, false));

        var t4 = blackBackground.DOFade(0f, 0.3f);
        yield return t4.WaitForCompletion();

        // Step3 - Present
        // player and God stop, wait for some seconds
        // God look left
        // proceed dialogue sequence4
        // player fade
        // proceed dialogue sequence5
        // God fade out
        // wait for some seconds
        playerAnim.SetBool("Walk", false);
        godAnim.SetBool("Walk", false);
        yield return new WaitForSeconds(step3WaitTime);

        god.eulerAngles = new Vector3(0f, 180f, 0f);
        yield return StartCoroutine(dialogueCoroutine(dialogue4));

        playerAnim.SetTrigger("Fade");
        yield return new WaitForSeconds(step3PlayerFadeTime);

        yield return StartCoroutine(dialogueCoroutine(dialogue5));

        var t5 = godSpriter.DOFade(0f, step3GodFadeTime);
        yield return t5.WaitForCompletion();

        yield return new WaitForSeconds(step3WaitTime2);


        // Step4 - Wife
        // wife fade in
        // proceed dialogue sequence6
        // white background fade in
        // Load "Ending2" Scene
        var t6 = wifeSpriter.DOFade(1f, step4WifeFadeTime);
        yield return t6.WaitForCompletion();

        yield return StartCoroutine(dialogueCoroutine(dialogue6));

        var t7 = whiteBackground.DOFade(1f, step4BackgroundFadeTime);
        yield return t7.WaitForCompletion();

        SceneManager.LoadScene("Ending2");
    }

    IEnumerator dialogueCoroutine(string[] texts, bool showTail = true)
    {
        textBox.SetActive(true);

        foreach(var text in texts)
        {
            playerTMP.text = "";
            godTMP.text = "";
            skipPressed = false;

            TextMeshProUGUI target = null;

            if (text[0] == 'p')
            {
                target = playerTMP;

                if(showTail)
                    playerTextTail.SetActive(true);
            }
            else if (text[0] == 'g')
            {
                target = godTMP;

                if(showTail)
                    godTextTail.SetActive(true);
            }
            else if (text[0] == 'w')
            {
                target = playerTMP;

                if(showTail)
                    wifeTextTail.SetActive(true);
            }

            bool skippedTyping = false;

            for (int i = 1; i < text.Length; i++)
            {
                if (skipPressed)
                {
                    skippedTyping = true;
                    break;
                }

                target.text += text[i];
                yield return new WaitForSeconds(textAppendTime);
            }

            if (skippedTyping)
                target.text = text.Substring(1);

            float t = 0f;
            skipPressed = false;

            while(t < nextTextWaitTime)
            {
                if (skipPressed)
                    break;

                t += Time.deltaTime;
                yield return null;
            }

            playerTextTail.gameObject.SetActive(false);
            godTextTail.SetActive(false);
            wifeTextTail.SetActive(false);
        }

        textBox.SetActive(false);
    }
}
