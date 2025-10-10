using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData data;

    public static event Action<ItemData, Vector3> CollectEvent;

    public void Collect()
    {
        if (ItemManager.Instance.IsHolding) return;

        gameObject.SetActive(false);
        CollectEvent?.Invoke(data, transform.position);
    }
}
