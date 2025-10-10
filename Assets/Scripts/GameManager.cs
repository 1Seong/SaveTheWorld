using System;
using UnityEngine;

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

    [SerializeField] private bool _isHolding = false;
    public bool IsHolding
    {
        get => _isHolding;
        set { _isHolding = value; }
    }

    [SerializeField] private int currentPhase = 0;
    public event Action PhaseChangedEvent;
    public void phaseChange()
    {
        ++currentPhase;
        PhaseChangedEvent?.Invoke();
    }

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
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }



}
