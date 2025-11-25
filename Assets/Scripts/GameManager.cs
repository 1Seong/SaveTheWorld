using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    /*
    public int CurrentPhase = 0;
    public event Action PhaseChangedEvent;
    public void phaseChange()
    {
        ++CurrentPhase;
        PhaseChangedEvent?.Invoke();
    }
    */

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

        isGameCleared = new Dictionary<string, bool>() 
        {
            {"Test", false},
            {"Syringe", false},
            {"Crutches", false},
            {"Jar", false},
            {"Pencil", false},
            {"Sewing", false },
            {"Laundry", false }
        };
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

    // use playerprefs
    public void SetMiniGameClear(string name)
    {
        isGameCleared[name] = true;

        bool notExist = false;
        foreach (var i in isGameCleared)
        {
            if(!i.Value)
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

    public void MiniGameClear()
    {
        SetMiniGameClear(SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name);
        SceneTransition.Instance.UnloadScene();
    }
}
