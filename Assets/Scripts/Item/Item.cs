using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public static class Item
{
    public enum Items
    {
        Example
    };

    public static Dictionary<Items, bool> collected = Enum.GetValues(typeof(Items))
    .Cast<Items>()
    .ToDictionary(s => s, s => false);

    public static void Collect(Items i)
    {
        collected[i] = true;
        //PlayerPrefs.SetString(i.ToString() + "Collected", "1");
    }

    public static bool IsCollected(Items i)
    {
        /*
        if(PlayerPrefs.HasKey(i.ToString() + "Collected") && PlayerPrefs.GetString(i.ToString() + "Collected") == "1" )
            collected[i] = true;
        */
        return collected[i];
    }
}
