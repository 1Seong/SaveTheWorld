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


    // -------------------------------------------------- Interactibles ---------------------------------------------------

    public enum Interactives
    {
        Syringe, Crutches, Jar, // Page1
        Pencil, Sewing, Laundry, // Page2
        TV,
        Others
    };

    public static Dictionary<Interactives, bool> dropped = Enum.GetValues(typeof(Interactives))
        .Cast<Interactives>()
        .ToDictionary(s => s, s => false);

    public static void Drop(Interactives i)
    {
        dropped[i] = true;
    }

    public static bool IsDropped(Interactives i)
    {
        return dropped[i];
    }
}
