using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject planeParent;
    [SerializeField] private RoomPlane[] planes;
    [SerializeField] private int currentPlaneId = 0;

    private Action convertAction;
    private Action convertLeftAction;
    private Action convertRightAction;

    private static StageManager _instance;
    public static StageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 찾기
                _instance = FindFirstObjectByType<StageManager>();

                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject("StageManager");
                    _instance = singletonObj.AddComponent<StageManager>();
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

        planes = planeParent.GetComponentsInChildren<RoomPlane>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planes[currentPlaneId].ActivateInteractives();

        convertLeftAction += SceneTransition.Instance.RoomLeftTransition;
        convertRightAction += SceneTransition.Instance.RoomRightTransition;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        if(!GameManager.Instance.IsTurning)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.Instance.IsTurning = true;
                ConvertViewLeft();
            }
            else if(Input.GetKeyDown(KeyCode.E)) 
            {
                GameManager.Instance.IsTurning = true;
                ConvertViewRight();
            }
        }
    }

    private void ConvertViewLeft()
    {
        planes[currentPlaneId].DeactivateInteractives();

        ++currentPlaneId;
        if(currentPlaneId == 4) currentPlaneId = 0;

        convertAction?.Invoke();
        convertLeftAction?.Invoke();

        ConvertView();
    }

    private void ConvertViewRight()
    {
        planes[currentPlaneId].DeactivateInteractives();

        --currentPlaneId;
        if (currentPlaneId == -1) currentPlaneId = 3;

        convertAction?.Invoke();
        convertRightAction?.Invoke();

        ConvertView();
    }

    private void ConvertView()
    {
        var targetRot = -90f * currentPlaneId;

        Camera.main.transform.DORotate(new Vector3(0f, targetRot, 0f), 0.9f).SetUpdate(true).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0f, targetRot, 0f));
            Camera.main.GetComponent<ViewCursorFollow>().ChangeOriginRot(Quaternion.Euler(0f, targetRot, 0f));
            planes[currentPlaneId].ActivateInteractives();
            GameManager.Instance.IsTurning = false;
        });
    }
}
