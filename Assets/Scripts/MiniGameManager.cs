using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    [Serializable]
    public struct Texts
    {
        public string[] texts;
    }

    public static event Action OnClearEvent;

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

    [SerializeField] private GameObject instruction;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI[] endingTmps;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Invoke(nameof(showInstruction), InstructionPopupTime);
        Invoke(nameof(play), GameStartTime);
    }

    private void OnDisable()
    {
        instance = null;
    }

    private void showInstruction()
    {
        instruction.SetActive(true);
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
        instruction.SetActive(false);
    }

    public void GameEnd(int i = 0)
    {
        IsPlaying = false;

        background.DOFade(1f, endFadeTime);
        StartCoroutine(typeWrite(EndingTexts[i].texts));
    }

    public void GameEnd(string[] texts)
    {
        IsPlaying = false;
        background.DOFade(1f, 0.5f).OnComplete(() =>
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

        for(int i = 0; i < texts.Length; ++i)
        {
            string s = "";

            for (int j = 0; j < texts[i].Length; ++j)
            {
                s += texts[i][j];
                endingTmps[i].text = s;
                yield return new WaitForSeconds(0.17f);
            }
            yield return new WaitForSeconds(1f);
        }

        for(int i = 0; i < texts.Length; ++i)
            StartCoroutine(FadeTMP(endingTmps[i], 0f, 0.5f));
        
        Invoke(nameof(returnToMain), 0.6f);
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

    private void returnToMain()
    {
        SceneTransition.Instance.UnloadScene();
    }
}
