using DG.Tweening;
using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject planeParent;
    [SerializeField] private RoomPlane[] planes;
    public int CurrentPlaneId = 0;

    private Action convertAction;
    private Action convertLeftAction;
    private Action convertRightAction;
    private Action convertCeilingAction;
    private Action convertSideReturnAction;

    private int tempPlaneId;

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
        }

        planes = planeParent.GetComponentsInChildren<RoomPlane>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //planes[CurrentPlaneId].ActivateInteractives();

        convertLeftAction += SceneTransition.Instance.RoomLeftTransition;
        convertRightAction += SceneTransition.Instance.RoomRightTransition;
        convertCeilingAction += SceneTransition.Instance.RoomCeilingTransition;
        convertSideReturnAction += SceneTransition.Instance.RoomSideReturnTransition;
    }

    
    public void ConvertViewLeft()
    {
        if (GameManager.Instance.IsTurning) return;
        GameManager.Instance.IsTurning = true;

        //planes[CurrentPlaneId].DeactivateInteractives();

        ++CurrentPlaneId;
        if(CurrentPlaneId >= 4) CurrentPlaneId = 0;

        convertAction?.Invoke();
        convertLeftAction?.Invoke();

        ConvertView();
    }

    public void ConvertViewRight()
    {
        if (GameManager.Instance.IsTurning) return;
        GameManager.Instance.IsTurning = true;

        //planes[CurrentPlaneId].DeactivateInteractives();

        --CurrentPlaneId;
        if (CurrentPlaneId <= -1) CurrentPlaneId = 3;

        convertAction?.Invoke();
        convertRightAction?.Invoke();

        ConvertView();
    }

    public void ConvertViewCeiling()
    {
        if (GameManager.Instance.IsTurning) return;
        GameManager.Instance.IsTurning = true;

        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_RoomTrans);
        //planes[CurrentPlaneId].DeactivateInteractives();

        tempPlaneId = CurrentPlaneId;
        CurrentPlaneId = 4;

        convertAction?.Invoke();
        convertCeilingAction?.Invoke();

        var targetRot = -90f;
        
        Camera.main.transform.DORotate(new Vector3(targetRot, -90 * tempPlaneId, 0f), 0.9f).SetUpdate(true).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(targetRot, -90 * tempPlaneId, 0f));
            Camera.main.GetComponent<ViewCursorFollow>().ChangeOriginRot(Quaternion.Euler(targetRot, -90 * tempPlaneId, 0f));
            //planes[CurrentPlaneId].ActivateInteractives();
            GameManager.Instance.IsTurning = false;
        });
    }

    public void ReturnToSide()
    {
        if (GameManager.Instance.IsTurning) return;
        GameManager.Instance.IsTurning = true;

        //planes[CurrentPlaneId].DeactivateInteractives();

        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_RoomTrans);

        CurrentPlaneId = tempPlaneId;

        convertAction?.Invoke();
        convertSideReturnAction?.Invoke();

        Camera.main.transform.DORotate(new Vector3(0f, -90 * tempPlaneId, 0f), 0.9f).SetUpdate(true).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0f, -90 * tempPlaneId, 0f));
            Camera.main.GetComponent<ViewCursorFollow>().ChangeOriginRot(Quaternion.Euler(0f, -90 * tempPlaneId, 0f));
            //planes[CurrentPlaneId].ActivateInteractives();
            GameManager.Instance.IsTurning = false;
        });
    }

    private void ConvertView()
    {
        AudioManager.Instance.PlaySfx(AudioType.SFX_Etc_RoomTrans);

        var targetRot = -90f * CurrentPlaneId;

        Camera.main.transform.DORotate(new Vector3(0f, targetRot, 0f), 0.9f).SetUpdate(true).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0f, targetRot, 0f));
            Camera.main.GetComponent<ViewCursorFollow>().ChangeOriginRot(Quaternion.Euler(0f, targetRot, 0f));
            //planes[CurrentPlaneId].ActivateInteractives();
            GameManager.Instance.IsTurning = false;
        });
    }
}
