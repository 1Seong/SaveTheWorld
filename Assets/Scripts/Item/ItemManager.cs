using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Item;

public class ItemManager : MonoBehaviour, ISaveable
{
    public string SaveKey => "ItemManager";

    public event Action ItemHoldEvent;
    public event Action ItemReleaseEvent;

    public event Action ItemAddedToUIEvent;

    public event Action ReturnFromCloseUpEvent;

    public Dictionary<Items, bool> collected;

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

    [Header("Field Items")]
    [SerializeField] private ItemObject[] itemObjects;

    [Header("# Fly Catch")]
    public bool isFlyCatched = false;
    [SerializeField] private ItemTarget[] itemTargets;

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
        // 중복 방지
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        SaveManager.Instance.Register(this);

        itemObjects = FindObjectsByType<ItemObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        itemTargets = FindObjectsByType<ItemTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        collected = new Dictionary<Items, bool>
        {
            { Items.Controller, false },
            {Items.FlyCatcher,false },
            {Items.Bottle,false },
            {Items.FilledBottle,false },
            {Items.LetterJu,false },
            {Items.LetterSa,false },
            {Items.LetterGi,false },
            {Items.LetterMok,false },
            {Items.LetterBal,false },
            {Items.LetterHang,false },
            {Items.LetterAa,false },
            {Items.LetterRi,false },
            {Items.LetterYeon,false },
            {Items.LetterPill,false },
            {Items.LetterJae,false },
            {Items.LetterBong,false },
            {Items.LetterTeul,false },
            {Items.LetterPpal,false },
            {Items.LetterLae, false }
        };
    }

    public bool IsCollected(Items item)
    {
        return collected[item];
    }

    public void SetCollected(Items item)
    {
        collected[item] = true;
    }

    private void OnDestroy()
    {
        if(SaveManager.Instance != null)
            SaveManager.Instance.Unregister(this);

        _instance = null;
    }

    private void Start()
    {
        ItemObject.CollectEvent += AddItem;
    }

    [System.Serializable]
    private class ItemManagerData
    {
        public SerializableItemsBoolDict collectedDatas;
        public List<ItemData> inventoryDatas;
        public bool flyData;
    }

    public string Save()
    {
        List<ItemData> id = new List<ItemData>();

        foreach(var i in inventoryItems)
        {
            id.Add(i.Data);
        }

        var imd = new ItemManagerData { collectedDatas = new SerializableItemsBoolDict(collected), inventoryDatas = id, flyData = isFlyCatched };

        return JsonUtility.ToJson(imd);
    }

    public void Load(string json)
    {
        try
        {
            var d = JsonUtility.FromJson<ItemManagerData>(json);

            foreach(var i in d.inventoryDatas)
            {
                AddItem(i, Vector3.zero);
            }

            collected = d.collectedDatas.ToDictionary();

            foreach(var i in itemObjects)
            {
                i.ApplyData(collected);
            }

            isFlyCatched = d.flyData;
            if(isFlyCatched)
            {
                foreach (var i in itemTargets)
                    i.ApplyFlyData();
            }
        }
        catch(Exception e)
        {
            Debug.LogWarning($"InventoryManager.Load failed: {e.Message}");
        }
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

    public void TurnOnRightButton()
    {
        RightButton.GetComponent<Image>().DOFade(1f, 0.3f).OnComplete(() =>
        {
            RightButton.GetComponent<Button>().interactable = true;
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

    // ----------------------------------------------- utility --------------------------------------------

    [System.Serializable]
    private class SerializableItemsBoolDict
    {
        public List<Items> keys = new List<Items>();
        public List<bool> values = new List<bool>();

        public SerializableItemsBoolDict() { }

        public SerializableItemsBoolDict(Dictionary<Items, bool> dict)
        {
            foreach (var kv in dict)
            {
                keys.Add(kv.Key);
                values.Add(kv.Value);
            }
        }

        public Dictionary<Items, bool> ToDictionary()
        {
            var dict = new Dictionary<Items, bool>();
            for (int i = 0; i < keys.Count; i++)
                dict[keys[i]] = values[i];
            return dict;
        }
    }
}
