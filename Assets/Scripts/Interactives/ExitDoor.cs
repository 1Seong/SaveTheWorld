using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour
{
    public Door door;

    [SerializeField] bool isUnlocked = false;

    bool isActing = false;

    private void Start()
    {
        GameManager.GameAllClearedEvent += ActivateDoor;
    }

    private void OnDisable()
    {
        GameManager.GameAllClearedEvent -= ActivateDoor;
    }

    public void OnClick()
    {
        if (isActing) return;

        if (isUnlocked)
            door.OnClick();
        else
        {
            isActing = true;
            transform.DOShakePosition(0.3f, 0.12f, 20).OnComplete(() =>
            {
                isActing = false;
            });
        }
    }

    private void ActivateDoor()
    {
        isUnlocked = true;
    }
}
