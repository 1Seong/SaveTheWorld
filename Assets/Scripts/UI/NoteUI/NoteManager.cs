using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour, ISaveable
{
    public string SaveKey => "NoteManager";

    public static event Action<Item.Interactives> BlurrUnlockEvent;

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
        // 중복 방지
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

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

        foreach (var image in TargetImages)
        {
            image.material.DOFloat(0f, "_DissolveStrength", 0f);
        }
    }

    private void OnDestroy()
    {
        if(SaveManager.Instance != null)
            SaveManager.Instance.Unregister(this);

        _instance = null;
    }

    [System.Serializable]
    private class NoteManagerData
    {
        public SerializableIntBoolDict insertedData;
    }

    public string Save()
    {
        var d = new NoteManagerData() { insertedData = new SerializableIntBoolDict(LetterInserted) };

        return JsonUtility.ToJson(d);
    }

    public void Load(string json)
    {
        var d = JsonUtility.FromJson<NoteManagerData>(json);

        LetterInserted = d.insertedData.ToDictionary();

        foreach (var i in letterTargets)
        {
            i.ApplyLetterData(LetterInserted);
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

        AudioManager.Instance.PlaySfx(AudioType.SFX_Note_Show);

        noteUIParent.DOAnchorPosY(0f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
        noteUIGrid.DOLocalMoveY(0f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
    }

    public void Hide()
    {
        isActive = false;

        AudioManager.Instance.PlaySfx(AudioType.SFX_Note_Show);

        noteUIParent.DOAnchorPosY(-403f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
        noteUIGrid.DOLocalMoveY(-6.9f, 0.7f).SetUpdate(true).SetEase(Ease.OutExpo);
    }

    public void OnInsertLetter(int type, int id, bool playSFX = true)
    {
        ++CompletedLetterCount[type];
        LetterInserted[id] = true;

        if (playSFX)
            AudioManager.Instance.PlaySfx(AudioType.SFX_Note_Letter);

        if (LetterCountGoal[type] <= CompletedLetterCount[type])
        {
            if (playSFX)
                AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_ShowBlurr);

            BlurrUnlockEvent?.Invoke((Item.Interactives)type);
            TargetImages[type].material.DOFloat(1f, "_DissolveStrength", 1.5f);
        }
    }


    // -------------------- utility --------------------------

    [System.Serializable]
    private class SerializableIntBoolDict
    {
        public List<int> keys = new List<int>();
        public List<bool> values = new List<bool>();

        public SerializableIntBoolDict() { }

        public SerializableIntBoolDict(Dictionary<int, bool> dict)
        {
            foreach (var kv in dict)
            {
                keys.Add(kv.Key);
                values.Add(kv.Value);
            }
        }

        public Dictionary<int, bool> ToDictionary()
        {
            var dict = new Dictionary<int, bool>();
            for (int i = 0; i < keys.Count; i++)
                dict[keys[i]] = values[i];
            return dict;
        }
    }
}
