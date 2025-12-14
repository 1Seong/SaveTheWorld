using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public string SaveKey => "GameManager";

    [SerializeField] private bool _isPlaying;
    public bool IsPlaying
    {
        get => _isPlaying;
        set { _isPlaying = value; }
    }

    [SerializeField] private bool _isTurning = false;
    public bool IsTurning
    {
        get => _isTurning;
        set { _isTurning = value; }
    }

    private Dictionary<string, bool> isGameCleared;
    public static event Action GameAllClearedEvent;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 찾기
                _instance = FindFirstObjectByType<GameManager>();

                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject("GameManager");
                    _instance = singletonObj.AddComponent<GameManager>();
                    DontDestroyOnLoad(singletonObj);
                }
            }

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
            DontDestroyOnLoad(gameObject);
        }

        SaveManager.Instance.Register(this);

        DOTween.SetTweensCapacity(200, 50);

        ResetGameCleared();
    }

    public void ResetGameCleared()
    {
        isGameCleared = new Dictionary<string, bool>()
        {
            {"Syringe", false},
            {"Crutches", false},
            {"Jar", false},
            {"Pencil", false},
            {"Sewing", false },
            {"Laundry", false },
            {"TV", false }
        };
    }

    private void OnDestroy()
    {
        if(SaveManager.Instance != null)
            SaveManager.Instance.Unregister(this);
    }

    [System.Serializable]
    private class GameManagerData
    {
        public SerializableStringBoolDict gmDatas;
    }

    public string Save()
    {
        var d = new GameManagerData() { gmDatas = new SerializableStringBoolDict(isGameCleared) };

        return JsonUtility.ToJson(d);
    }

    public void Load(string json)
    {
        try
        {
            var d = JsonUtility.FromJson<GameManagerData>(json);

            isGameCleared = d.gmDatas.ToDictionary();
            checkMiniGameAllClear();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"GameManager.Load failed: {e.Message}");
        }
    }


    public void StopTime()
    {
        IsPlaying = false;
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        IsPlaying = true;
        Time.timeScale = 1f;
    }

    // -------------------------------------------- Mini Game Controls ------------------------------------------------

    public void SetMiniGameClear(string name)
    {
        isGameCleared[name] = true;

        checkMiniGameAllClear();
    }

    private void checkMiniGameAllClear()
    {
        bool notExist = false;
        foreach (var i in isGameCleared)
        {
            if (!i.Value)
            {
                notExist = true;
                break;
            }
        }
        if (!notExist)
            GameAllClearedEvent?.Invoke();
    }

    public bool IsMiniGameCleared(string name)
    {
        return isGameCleared[name];

    }


    // -------------------------------------- utility ----------------------------------------

    [System.Serializable]
    private class SerializableStringBoolDict
    {
        public List<string> keys = new List<string>();
        public List<bool> values = new List<bool>();

        public SerializableStringBoolDict() { }

        public SerializableStringBoolDict(Dictionary<string, bool> dict)
        {
            foreach (var kv in dict)
            {
                keys.Add(kv.Key);
                values.Add(kv.Value);
            }
        }

        public Dictionary<string, bool> ToDictionary()
        {
            var dict = new Dictionary<string, bool>();
            for (int i = 0; i < keys.Count; i++)
                dict[keys[i]] = values[i];
            return dict;
        }
    }
}
