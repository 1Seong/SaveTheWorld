using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject planeParent;
    [SerializeField] private RoomPlane[] planes;
    [SerializeField] private int currentPlaneId = 0;
    
    [SerializeField] private bool isTurning = false;

    private Action convertAction;
    private Action convertLeftAction;
    private Action convertRightAction;

    private void Awake()
    {
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

        if(!isTurning)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isTurning = true;
                ConvertViewLeft();
            }
            else if(Input.GetKeyDown(KeyCode.E)) 
            {
                isTurning = true;
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

        ConvertView(-90f);
    }

    private void ConvertViewRight()
    {
        planes[currentPlaneId].DeactivateInteractives();

        --currentPlaneId;
        if (currentPlaneId == -1) currentPlaneId = 3;

        convertAction?.Invoke();
        convertRightAction?.Invoke();

        ConvertView(90f);
    }

    private void ConvertView(float targetRot)
    {
        var currentRotY = Camera.main.transform.rotation.eulerAngles.y;

        Camera.main.transform.DORotate(new Vector3(0f, currentRotY + targetRot, 0f), 0.9f).SetUpdate(true).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0f, currentRotY + targetRot, 0f));
            planes[currentPlaneId].ActivateInteractives();
            isTurning = false;
        });
    }
}
