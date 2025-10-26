using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public event Action ItemHoldEvent;
    public event Action ItemReleaseEvent;

    public event Action ItemAddedToUIEvent;

    public event Action ReturnFromCloseUpEvent;

    [Header("# UI Control")]
    [SerializeField] GameObject LeftButton;
    [SerializeField] GameObject RightButton;
    [SerializeField] GameObject UpButton;
    [SerializeField] GameObject ReturnFromCloseUpButton;

    [Header("# Inventory Item")]
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private RectTransform targetGroup;

    [SerializeField] private InventoryItem _selectedItem;
    public InventoryItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
        }
    }

    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            

            return _instance;
        }
    }

    private bool _isHolding = false;
    public bool IsHolding
    {
        get => _isHolding;
        set
        {
            _isHolding = value;
            if (value) // hold item
            {
                ItemHoldEvent?.Invoke();
            }
            else // release item
            {
                SelectedItem.DeactivateItemFollow();
                SelectedItem = null;
                ItemReleaseEvent?.Invoke();
            }
        }
    }

    private void Awake()
    {
        //inventoryItems = new List<InventoryItem>();

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

    private void Start()
    {
        ItemObject.CollectEvent += AddItem;

        // LoadSavedItems();
    }

    public void SaveItems()
    {
        /*
        foreach(var item in inventoryItems)
        */
        // save to playerprefs
    }

    private void LoadSavedItems()
    {
        // for all datas in files, find from playerprefs?
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;
        
        if(IsHolding)
        {
            var enumName = Enum.GetName(typeof(Item.Items), SelectedItem.Data.id); // get enum name

            if (Input.GetMouseButtonDown(0))
            {
                if (enumName.Contains("Letter")) // use GraphicRaycaster for letter item target
                {
                    //Debug.Log("1");
                    var pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;

                    var results = new List<RaycastResult>();
                    NoteManager.Instance.GetComponent<GraphicRaycaster>().Raycast(pointerEventData, results);

                    foreach (var result in results)
                    {
                        //Debug.Log("2");
                        var target = result.gameObject.GetComponent<LetterTarget>();
                        if(target != null)
                        {
                            target.OnInteract(SelectedItem.Data);
                            break;
                        }
                    }
                }
                else // use Physics Raycast for other items
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("1");
                        var target = hit.collider.GetComponent<ItemTarget>();
                        if (target != null)
                        {
                            //Debug.Log("2");
                            target.OnInteract(SelectedItem.Data);
                        }
                    }
                }

                IsHolding = false;
            }
        }
    }

    public void AddItem(ItemData d, Vector3 startPos)
    {
        StartCoroutine(AddItemCoroutine(d, startPos));
        
    }

    private IEnumerator AddItemCoroutine(ItemData d, Vector3 startPos)
    {
        //Debug.Log(startPos.ToString());

        var itemUi = Instantiate(inventoryItemPrefab, targetGroup).GetComponent<InventoryItem>();
        inventoryItems.Add(itemUi);
        itemUi.Data = d;

        // Add item to List
        var item = itemUi.transform.GetChild(0).GetComponent<RectTransform>();
        var itemTemp = Instantiate(item, GetComponent<RectTransform>());
        itemTemp.gameObject.SetActive(true);

        // Item Moving Animation + Add item to UI
        // world pos to screen pos
        var screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, startPos);

        //Debug.Log(screenPos.ToString());

        // screen pos to canvas pos
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(), screenPos, null, out localPos);

        itemTemp.anchoredPosition = localPos;
        //Debug.Log(localPos.ToString());

        // world pos to screen pos to canvas pos (item)
        itemUi.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame(); // must wait for the update so that we can get the element position of vertical layout group

        var screenPosC = RectTransformUtility.WorldToScreenPoint(null, item.position);
        //Debug.Log(screenPosC.ToString());
        Vector2 localPosC;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(), screenPosC, null, out localPosC);
        //Debug.Log(localPosC.ToString());

        itemTemp.DOAnchorPos3D(localPosC, 0.5f).SetUpdate(true).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Destroy(itemTemp.gameObject);
            item.gameObject.SetActive(true);
            ItemAddedToUIEvent?.Invoke();
        });
    }

    public void RemoveItem() 
    {
        inventoryItems.Remove(SelectedItem);
        Destroy(SelectedItem.gameObject);
    }

    //--------------------------------------------------------- UI Control ----------------------------------------------------------------

    public void TurnOffGoButtons()
    {
        LeftButton.GetComponent<Button>().interactable = false;
        LeftButton.GetComponent<Image>().DOFade(0f, 0.3f);
        RightButton.GetComponent<Button>().interactable = false;
        RightButton.GetComponent<Image>().DOFade(0f, 0.3f);
        UpButton.GetComponent<Button>().interactable = false;
        UpButton.GetComponent<Image>().DOFade(0f, 0.3f);
    }

    public void TurnOnGoButtons()
    {
        LeftButton.GetComponent<Image>().DOFade(1f, 0.3f).OnComplete(() =>
        {
            LeftButton.GetComponent<Button>().interactable = true;
        });
        RightButton.GetComponent<Image>().DOFade(1f, 0.3f).OnComplete(() =>
        {
            RightButton.GetComponent<Button>().interactable = true;
        });
        UpButton.GetComponent<Image>().DOFade(1f, 0.3f).OnComplete(() =>
        {
            UpButton.GetComponent<Button>().interactable = true;
        });
    }

    public void TurnOnReturnFromCloseUpButton()
    {
        ReturnFromCloseUpButton.gameObject.SetActive(true);
    }

    public void ReturnFromCloseUpOnClick()
    {
        Camera.main.transform.DOMove(Vector3.zero, 0.6f).SetUpdate(true).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            ReturnFromCloseUpEvent?.Invoke();
            TurnOnGoButtons();
        });
    }
}
