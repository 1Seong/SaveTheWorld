using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public Item.Interactives typeId;

    [Serializable]
    public struct Texts
    {
        public string[] texts;
    }

    public static event Action<Item.Interactives> OnClearEvent;

    public int GameRepeatNum = 4;
    public float InstructionPopupTime = 2f;
    public float InstructionShowTime = 5.0f;
    public float GameStartTime = 0f;
    public float GameEndTime = 2f;
    public float endFadeTime = 0.5f;
    public bool IsPlaying = false;
    public Texts[] EndingTexts;

    public static MiniGameManager instance;

    private int currentGameCount = 0;

    private bool isTypeWriting = false;
    private bool skipPressed = false;

    [SerializeField] private TextMeshProUGUI instruction;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI[] endingTmps;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(isTypeWriting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                skipPressed = true;
        }
    }

    private void OnEnable()
    {
        Invoke(nameof(showInstruction), InstructionPopupTime);
        Invoke(nameof(play), GameStartTime);
    }

    private void OnDisable()
    {
        if(background.material != null)
            background.material.SetFloat("_DissolveStrength", 1f);
        instance = null;
    }

    private void showInstruction()
    {
        StartCoroutine(FadeTMP(instruction, 1f, 0.5f));
        Invoke(nameof(hideInstruction), InstructionShowTime);
    }

    public void play()
    {
        IsPlaying = true;
    }

    public void stop()
    {
        IsPlaying = false;
    }

    private void hideInstruction()
    {
        StartCoroutine(FadeTMP(instruction, 0f, 0.5f));
    }

    public void GameEnd(int i = 0)
    {
        IsPlaying = false;

        background.DOFade(1f, endFadeTime).OnComplete(() =>
        {
            StartCoroutine(typeWrite(EndingTexts[i].texts));
        });
    }

    public void GameEnd(string[] texts)
    {
        IsPlaying = false;
        background.DOFade(1f, endFadeTime).OnComplete(() =>
        {
            StartCoroutine(typeWrite(texts));
        });
    }

    public void CountUp()
    {
        ++currentGameCount;
        if(currentGameCount == GameRepeatNum)
        {
            Invoke(nameof(GameEnd), GameEndTime);
        }
    }

    private IEnumerator typeWrite(string[] texts)
    {
        yield return new WaitForSeconds(1f);

        isTypeWriting = true;

        for(int i = 0; i < texts.Length; ++i)
        {
            skipPressed = false;
            string s = "";

            bool skipped = false;

            AudioManager.Instance.LoopSfxOn(AudioType.SFX_MG_Type);
            for (int j = 0; j < texts[i].Length; ++j)
            {
                if (skipPressed)
                {
                    skipped = true;
                    break;
                }

                s += texts[i][j];
                endingTmps[i].text = s;
                yield return new WaitForSeconds(0.09f);
            }
            if (skipped)
            {
                endingTmps[i].text = texts[i];
            }
            AudioManager.Instance.LoopSfxOff();

            skipPressed = false;
            float t = 0f;
            while(t < 1f)
            {
                if (skipPressed)
                    break;

                t += Time.deltaTime;
                yield return null;
            }
        }

        skipPressed = false;
        float t1 = 0f;
        while(t1 < 2.5f)
        {
            if (skipPressed)
                break;

            t1 += Time.deltaTime;
            yield return null;
        }

        isTypeWriting = false;

        for (int i = 0; i < texts.Length; ++i)
            StartCoroutine(FadeTMP(endingTmps[i], 0f, 0.8f));
        
        Invoke(nameof(returnToMain), 1f);
    }

    private IEnumerator FadeTMP(TextMeshProUGUI tmp, float targetAlpha, float duration)
    {
        float startAlpha = tmp.alpha;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            tmp.alpha = newAlpha;
            yield return null;
        }
        tmp.alpha = targetAlpha;
    }

    public void returnToMain()
    {
        OnClearEvent?.Invoke(typeId);

        SceneTransition.Instance.UnloadScene();
    }
}
