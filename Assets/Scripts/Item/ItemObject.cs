using System;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData data;

    public static event Action<ItemData> CollectEvent;

    public void Collect()
    {
        CollectEvent?.Invoke(data);
    }
}
