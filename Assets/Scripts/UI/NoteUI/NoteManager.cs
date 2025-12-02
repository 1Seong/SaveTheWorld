using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour, ISaveable
{
    public string SaveKey => "NoteManager";

    public event Action<Item.Interactives> BlurrUnlockEvent;

    public bool isActive = false;

    [SerializeField] private RectTransform noteUIParent;
    [SerializeField] private Transform noteUIGrid;

    public int[] LetterCountGoal;
    public int[] CompletedLetterCount;
    public Image[] TargetImages;

    private Dictionary<int, bool> LetterInserted;
    private LetterTarget[] letterTargets;

    private static NoteManager _instance;
    public static NoteManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        SaveManager.Instance.Register(this);

        letterTargets = FindObjectsByType<LetterTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        LetterInserted = new Dictionary<int, bool>()
        {
            {0, false},
            {1, false},
            {2, false},
            {3, false},
            {4, false},
            {5, false},
            {6, false},
            {7, false},
            {8, false},
            {9, false},
            {10, false},
            {11, false},
            {12, false},
            {13, false},
            {14, false}
        };

        // 중복 방지
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // TODO : Load data (insert again - and unlock interactives)
    }

    private void OnDestroy()
    {
        if(SaveManager.Instance != null)
            SaveManager.Instance.Unregister(this);
    }

    [System.Serializable]
    private class NoteManagerData
    {
        public int[] countData;
        public Dictionary<int, bool> insertedData;
    }

    public string Save()
    {
        var d = new NoteManagerData() { countData = CompletedLetterCount, insertedData = LetterInserted };

        return JsonUtility.ToJson(d);
    }

    public void Load(string json)
    {
        try
        {
            var d = JsonUtility.FromJson<NoteManagerData>(json);

            CompletedLetterCount = d.countData;
            LetterInserted = d.insertedData;

            foreach (var i in letterTargets)
            {
                i.ApplyLetterData(LetterInserted);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"NoteManager.Load failed: {e.Message}");
        }
    }

    public void TurnOff()
    {
        noteUIGrid.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void TurnOn()
    {
        noteUIGrid.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Show()
    {
        isActive = true;
        noteUIParent.DOAnchorPosY(0f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
        noteUIGrid.DOLocalMoveY(0f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
    }

    public void Hide()
    {
        isActive = false;
        noteUIParent.DOAnchorPosY(-403f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
        noteUIGrid.DOLocalMoveY(-6.9f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
    }

    public void OnInsertLetter(int type, int id)
    {
        // TODO : save which one is inserted

        ++CompletedLetterCount[type];
        LetterInserted[id] = true;

        if (LetterCountGoal[type] == CompletedLetterCount[type])
        {
            BlurrUnlockEvent?.Invoke((Item.Interactives)type);
            TargetImages[type].material.DOFloat(1f, "_DissolveStrength", 1f);
        }
    }
}
