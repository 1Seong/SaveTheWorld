using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static event Action OnClearEvent;

    public int GameRepeatNum = 4;
    public float InstructionPopupTime = 2f;
    public float GameStartTime = 0f;
    public float GameEndTime = 2f;
    public bool IsPlaying = false;
    public string[] EndingTexts;

    public static MiniGameManager instance;

    private int currentGameCount = 0;

    [SerializeField] private GameObject instruction;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI tmp;

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
        Invoke(nameof(hideInstruction), 5f);
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

    public void GameEnd()
    {
        IsPlaying = false;
        background.DOFade(1f, 0.5f);
        //typewrite(tmp, text); // 타이핑 연출 다하고, 글자 fade out되고, 다시 메인 게임으로 돌아와야됨
    }

    public void CountUp()
    {
        ++currentGameCount;
        if(currentGameCount == GameRepeatNum)
        {
            Invoke(nameof(GameEnd), GameEndTime);
        }
    }
}
