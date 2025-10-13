using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData data;

    public static event Action<ItemData, Vector3> CollectEvent;

    private void Start()
    {
        if (Item.IsCollected(data.id)) gameObject.SetActive(false);
    }

    public void Collect()
    {
        if (ItemManager.Instance.IsHolding) return;

        Item.Collect(data.id);
        gameObject.SetActive(false);
        CollectEvent?.Invoke(data, transform.position);
    }
}
