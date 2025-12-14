using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TVManager : MonoBehaviour
{
    public Item.Interactives typeId;

    public static event Action<Item.Interactives> OnClearEvent;

    public float GameStartWaitTime = 3f;

    public TextAsset dialogueCSV1;
    public TextAsset dialogueCSV2;
    public TextAsset dialogueCSV3;
    public TextMeshProUGUI dialogueText;
    public GameObject DialogueUI;

    public float textAppendTime = 0.08f;
    public float nextTextWaitTime = 2f;
    public float fadeOutTime = 1.8f;

    public PortraitController portraitController;
    public GameObject seq1Background;
    public GameObject seq2Background;
    public GameObject seq3Background;
    public Image blackBackground;

    public GameObject leftCharImage;
    public GameObject rightCharImage;

    private List<DialogueData> dialogues1;
    private List<DialogueData> dialogues2;
    private List<DialogueData> dialogues3;
    private int index = 0;

    [Serializable]
    private class SeqData
    {
        public List<DialogueData> dialogues;
        public GameObject background;
    }

    private SeqData[] seqs;

    bool skipPressed = true;



    void Start()
    {
        dialogues1 = CSVDialogueParser.Parse(dialogueCSV1);
        dialogues2 = CSVDialogueParser.Parse(dialogueCSV2);
        dialogues3 = CSVDialogueParser.Parse(dialogueCSV3);

        seqs = new SeqData[3];
        seqs[0] = new SeqData() { dialogues = dialogues1, background = seq1Background };
        seqs[1] = new SeqData() { dialogues = dialogues2, background = seq2Background };
        seqs[2] = new SeqData() { dialogues = dialogues3, background = seq3Background };

        seq1Background.SetActive(true);

        //DialogueUI.SetActive(true);

        Invoke(nameof(ShowFirstDialogue), fadeOutTime + GameStartWaitTime);
        
    }

    private void ShowFirstDialogue()
    {
        DialogueUI.SetActive(true);
        ShowDialogue(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            skipPressed = true;
    }

    void ShowDialogue(int seqIndex)
    {
        var targetDialogue = seqs[seqIndex].dialogues;

        if (index >= targetDialogue.Count) return;

        var d = targetDialogue[index];

        portraitController.UpdatePortrait(
            d.isSpeakerLeft,
            d.leftCharacter,
            d.rightCharacter
        );

        StartCoroutine(dialogueCoroutine(d.dialogue, seqIndex));
    }

    public void NextDialogue(int seq)
    {
        ++index;
        if (index < seqs[seq].dialogues.Count)
            ShowDialogue(seq);

        else if(seq < 2)
        {
            index = 0;

            DialogueUI.SetActive(false);
            leftCharImage.SetActive(false);
            rightCharImage.SetActive(false);

            AudioManager.Instance.StopBgm();

            blackBackground.DOFade(1f, fadeOutTime).OnComplete(() =>
            {
                if (seq == 0)
                    AudioManager.Instance.PlayBgm(AudioType.BGM_TV2);
                else if (seq == 1)
                    AudioManager.Instance.PlayBgm(AudioType.BGM_TV3);

                seqs[seq].background.SetActive(false);
                seqs[seq + 1].background.SetActive(true);

                blackBackground.DOFade(0f, fadeOutTime).OnComplete(() =>
                {
                    DialogueUI.SetActive(true);
                    ShowDialogue(seq + 1);
                });
            });
        }
        else
        {
            returnToMain();
        }
    }

    IEnumerator dialogueCoroutine(string line, int seq)
    {
        dialogueText.text = "";
        skipPressed = false;

        bool skippedTyping = false;

        for (int i = 0; i < line.Length; i++)
        {
            if (skipPressed)
            {
                skippedTyping = true;
                break;
            }

            dialogueText.text += line[i];
            yield return new WaitForSeconds(textAppendTime);
        }

        if (skippedTyping)
            dialogueText.text = line;

        float t = 0f;
        skipPressed = false;

        while (t < nextTextWaitTime)
        {
            if (skipPressed)
                break;

            t += Time.deltaTime;
            yield return null;
        }

        NextDialogue(seq);
    }

    private void returnToMain()
    {
        OnClearEvent?.Invoke(typeId);

        SceneTransition.Instance.UnloadScene();
    }
}
