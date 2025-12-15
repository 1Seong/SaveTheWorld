using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData data;

    public static event Action<ItemData, Vector3> CollectEvent;

    public void Collect()
    {
        if (data.id != Item.Items.FilledBottle && ItemManager.Instance.IsHolding) return;

        AudioManager.Instance.PlaySfx(AudioType.SFX_Room_Item);

        ItemManager.Instance.SetCollected(data.id);
        CollectEvent?.Invoke(data, transform.position);
        Destroy(gameObject);
    }

    public void ApplyData(Dictionary<Item.Items, bool> dic)
    {
        if (dic[data.id])
            Destroy(gameObject);
    }
}
