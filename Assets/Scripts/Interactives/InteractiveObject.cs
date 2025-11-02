using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    public Item.Interactives typeId;

    [Header("# Blurr Setting")]
    [SerializeField] private bool hasBlurr = false;

    [Header("# Drop Item Setting")]
    [SerializeField] private bool hasDropItem = false;
    [SerializeField] private GameObject itemObject;
    [SerializeField] private float dropDisX = 0f;
    [SerializeField] private float dropDisY = -0.5f;

    [Header("# Toggle Item Setting")]
    [SerializeField] private bool hasToggleItem = false;

    [Header("# Close up Setting")]
    [SerializeField] private bool hasCloseUp = false;
    [SerializeField] private float closeUpDistance = 5f;
    [SerializeField] private Vector3 biasV = Vector3.zero;
    [SerializeField] private bool hasMiniGame = false;
    [SerializeField] private string sceneName;


    private void Start()
    {
        if (hasBlurr && Item.IsBlurred(typeId))
        {
            GetComponent<Button>().interactable = false;
            GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);
        }
        else
            GetComponent<Button>().interactable = true;

        /*
        if(hasDropItem && Item.IsDropped(typeId))
            DropInit();
        */
        NoteManager.Instance.BlurrUnlockEvent += unlockBlurr;
        ItemManager.Instance.ReturnFromCloseUpEvent += buttonInteractiveOn;
    }

    private void OnDestroy()
    {
        NoteManager.Instance.BlurrUnlockEvent -= unlockBlurr;
        ItemManager.Instance.ReturnFromCloseUpEvent -= buttonInteractiveOn;
    }

    private void unlockBlurr(Item.Interactives id)
    {
        if (id != typeId) return;

        Item.UnlockBlurr(typeId);
        GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
        GetComponent<Button>().interactable = true;
    }

    private void DropInit()
    {
        if (GetComponentInChildren<ItemObject>() == null) return; // item is already collected

        var itemObject = GetComponentInChildren<ItemObject>().gameObject;
        itemObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(dropDisX, -5f), 0.5f).SetUpdate(true).OnComplete(() =>
        {
            itemObject.SetActive(true);
        });
    }

    public void DropItemOnClick()
    {
        if (!hasDropItem) return;
        if(Item.IsDropped(typeId)) return;

        Item.Drop(typeId);
        if(itemObject == null)
            itemObject = GetComponentInChildren<ItemObject>(true).gameObject;
        itemObject.SetActive(true);

        Sequence seq = DOTween.Sequence(itemObject);

        seq.Append(itemObject.transform.DOMoveY(itemObject.transform.position.y + dropDisY, .8f).SetUpdate(true).SetEase(Ease.OutSine)) // 1
        .Join(itemObject.transform.DOMoveX(itemObject.transform.position.x + dropDisX, .8f).SetUpdate(true).SetEase(Ease.InOutSine))
        .Append(itemObject.transform.DOMoveY(itemObject.transform.position.y + 2 * dropDisY, .8f).SetUpdate(true).SetEase(Ease.OutSine)) // 2
        .Join(itemObject.transform.DOMoveX(itemObject.transform.position.x - dropDisX, .8f).SetUpdate(true).SetEase(Ease.InOutSine))
        .Append(itemObject.transform.DOMoveY(itemObject.transform.position.y + 3 * dropDisY, .8f).SetUpdate(true).SetEase(Ease.OutSine)) // 3
        .Join(itemObject.transform.DOMoveX(itemObject.transform.position.x + dropDisX, .8f).SetUpdate(true).SetEase(Ease.InOutSine))
        .Append(itemObject.transform.DOMoveY(itemObject.transform.position.y + 4 * dropDisY, .8f).SetUpdate(true).SetEase(Ease.OutSine)) // 4
        .Join(itemObject.transform.DOMoveX(itemObject.transform.position.x - dropDisX, .8f).SetUpdate(true).SetEase(Ease.InOutSine));

    }

    public void ToggleItemOnClick()
    {
        if(!hasToggleItem) return;
        if (GetComponentInChildren<ItemObject>(true) == null) return;

        var itemObject = GetComponentInChildren<ItemObject>(true).gameObject;
        itemObject.SetActive(!itemObject.activeSelf);
    }

    public void CloseUpOnClick()
    {
        if(!hasCloseUp) return;
        GetComponent<Button>().interactable = false;
        ItemManager.Instance.TurnOffGoButtons();

        if(!hasMiniGame)
            ItemManager.Instance.TurnOnReturnFromCloseUpButton();

        var targetPos = transform.position;
        var res = Camera.main.transform.position + (targetPos - Camera.main.transform.position + biasV).normalized * closeUpDistance;
        /*
        var res = StageManager.Instance.CurrentPlaneId switch
        {
            0 => new Vector3(targetPos.x, targetPos.y, closeUpDistance),
            1 => new Vector3(-closeUpDistance, targetPos.y, targetPos.z),
            2 => new Vector3(targetPos.x, targetPos.y, -closeUpDistance),
            3 => new Vector3(closeUpDistance, targetPos.y, targetPos.z),
            4 => new Vector3(targetPos.x, closeUpDistance, targetPos.z),
            _ => throw new System.NotImplementedException()
        };
        */

        if (hasMiniGame)
        {
            if (!GameManager.Instance.IsMiniGameCleared(sceneName))
            {
                Camera.main.transform.DOMove(res, 0.5f).SetUpdate(true).SetEase(Ease.OutCirc).OnComplete(() =>
                {
                    //ItemManager.Instance.ReturnFromCloseUpEvent += deactiveItems;
                    Invoke(nameof(giveRewards), 0.3f);
                    ItemManager.Instance.TurnOnReturnFromCloseUpButton();
                    SceneTransition.Instance.LoadSceneAdditive(sceneName);

                });
            }
            else
            {
                Camera.main.transform.DOMove(res, 0.5f).SetUpdate(true).SetEase(Ease.OutCirc).OnComplete(() =>
                {
                    ItemManager.Instance.TurnOnReturnFromCloseUpButton();

                });
                /*
                foreach (var i in GetComponentsInChildren<ItemObject>(true))
                {
                    i.gameObject.SetActive(!i.gameObject.activeSelf);
                }
                ItemManager.Instance.ReturnFromCloseUpEvent += deactiveItems;
                */
            }
        }
        else
            Camera.main.transform.DOMove(res, 0.5f).SetUpdate(true).SetEase(Ease.OutCirc);

    }

    private void giveRewards()
    {
        foreach (var i in GetComponentsInChildren<ItemObject>(true))
        {
            i.gameObject.SetActive(true);
        }
    }

    private void buttonInteractiveOn()
    {
        GetComponent<Button>().interactable = true;
    }

    private void deactiveItems()
    {
        foreach (var i in GetComponentsInChildren<ItemObject>(true))
        {
            i.gameObject.SetActive(false);
        }
        ItemManager.Instance.ReturnFromCloseUpEvent -= deactiveItems;
    }
}
