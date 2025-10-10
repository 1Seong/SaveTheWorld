using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public event Action ItemAddedToUIEvent;

    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private List<InventoryItem> inventoryItems;
    [SerializeField] private RectTransform targetGroup;

    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬에서 찾기
                _instance = FindFirstObjectByType<ItemManager>();

                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject singletonObj = new GameObject("ItemManager");
                    _instance = singletonObj.AddComponent<ItemManager>();
                    DontDestroyOnLoad(singletonObj);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        inventoryItems = new List<InventoryItem>();

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
    }

    public void AddItem(ItemData d, Vector3 startPos)
    {
        StartCoroutine(AddItemCoroutine(d, startPos));
        
    }

    private IEnumerator AddItemCoroutine(ItemData d, Vector3 startPos)
    {
        Debug.Log(startPos.ToString());

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

    public void RemoveItem(int index) 
    {
        
    }
}
