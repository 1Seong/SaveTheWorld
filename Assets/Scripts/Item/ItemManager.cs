using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> inventoryItems;

    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // ������ ã��
                _instance = FindFirstObjectByType<ItemManager>();

                // ������ ���� ����
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
    }
}
