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
        Controller, FlyCatcher, Bottle, FilledBottle,
        LetterJu, LetterSa, LetterGi, // 주사기
        LetterMok, LetterBal, // 목발
        LetterHang, LetterAa, LetterRi, // 항아리
        LetterYeon, LetterPill, // 연필
        LetterJae, LetterBong, LetterTeul, // 재봉틀
        LetterPpal, LetterLae // 빨래
    };

    public static Dictionary<Items, bool> collected = Enum.GetValues(typeof(Items))
    .Cast<Items>()
    .ToDictionary(s => s, s => false);

    public static void Collect(Items i)
    {
        // TODO : save data

        collected[i] = true;
        //PlayerPrefs.SetString(i.ToString() + "Collected", "1");
    }

    public static bool IsCollected(Items i)
    {
        // TODO : check save data

        /*
        if(PlayerPrefs.HasKey(i.ToString() + "Collected") && PlayerPrefs.GetString(i.ToString() + "Collected") == "1" )
            collected[i] = true;
        */
        return collected[i];
    }

    // -------------------------------------------------- Item Target ------------------------------------------------------

    public static Dictionary<string, bool> targetAct = new Dictionary<string, bool>()
    {
        {"TV", false },
        {"Fly", false }
    };

    public static void TargetAct(string i)
    {
        if (i != "TV" && i != "Fly") return;

        targetAct[i] = true;
    }

    public static bool IsTargetActed(string i)
    {
        if (i != "TV" && i != "Fly") return false;

        return targetAct[i];
    }


    // -------------------------------------------------- Interactibles ---------------------------------------------------

    public enum Interactives
    {
        Syringe, Crutches, Jar, // Page1
        Pencil, Sewing, Laundry, // Page2
        TV,
        Others
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
    }

    public static bool IsBlurred(Interactives i)
    {
        return blurred[i];
    }

    public static void Drop(Interactives i)
    {
        dropped[i] = true;
    }

    public static bool IsDropped(Interactives i)
    {
        return dropped[i];
    }
}
