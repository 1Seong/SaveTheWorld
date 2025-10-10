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
                // ������ ã��
                _instance = FindFirstObjectByType<StageManager>();

                // ������ ���� ����
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
        // �ߺ� ����
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

    
    public void ConvertViewLeft()
    {
        if (GameManager.Instance.IsTurning) return;
        GameManager.Instance.IsTurning = true;

        planes[currentPlaneId].DeactivateInteractives();

        ++currentPlaneId;
        if(currentPlaneId == 4) currentPlaneId = 0;

        convertAction?.Invoke();
        convertLeftAction?.Invoke();

        ConvertView();
    }

    public void ConvertViewRight()
    {
        if (GameManager.Instance.IsTurning) return;
        GameManager.Instance.IsTurning = true;

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
