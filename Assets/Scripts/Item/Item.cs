using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public static class Item
{
    // ---------------------------------------------------- Item Objects --------------------------------------------------------
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

    // -------------------------------------------------- Interactibles ---------------------------------------------------

    public enum Interactives
    {
        Example
    };

    public static Dictionary<Interactives, bool> blurred = Enum.GetValues(typeof(Interactives))
        .Cast<Interactives>()
        .ToDictionary(s => s, s => true);

    public static Dictionary<Interactives, bool> dropped = Enum.GetValues(typeof(Interactives))
        .Cast<Interactives>()
        .ToDictionary(s => s, s => false);

    public static void UnlockBlurr(Interactives i)
    {
        blurred[i] = false;
        //PlayerPrefs.SetString(i.ToString() + "Blurred", "0");
    }

    public static bool IsBlurred(Interactives i)
    {
        /*
        if(PlayerPrefs.HasKey(i.ToString() + "Blurred") && PlayerPrefs.GetString(i.ToString() + "Blurred") == "0" )
            blurred[i] = false;
        */
        return blurred[i];
    }

    public static void Drop(Interactives i)
    {
        dropped[i] = true;
        //PlayerPrefs.SetString(i.ToString() + "Dropped", "1");
    }

    public static bool IsDropped(Interactives i)
    {
        /*
        if(PlayerPrefs.HasKey(i.ToString() + "Dropped") && PlayerPrefs.GetString(i.ToString() + "Dropped") == "1" )
            dropped[i] = false;
        */
        return dropped[i];
    }
}
