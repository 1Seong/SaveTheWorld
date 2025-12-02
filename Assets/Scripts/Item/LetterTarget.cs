using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LetterTarget : ItemTarget
{
    public int letterId;

    public static event Action tutorialEvent;

    [SerializeField] private Item.Interactives interactiveType;

    protected override void itemMatched()
    {
        tutorialEvent?.Invoke();

        InsertLetter();
    }

    private void InsertLetter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        NoteManager.Instance.OnInsertLetter((int)interactiveType, letterId);
    }

    public void ApplyLetterData(Dictionary<int, bool> dic)
    {
        if (dic[letterId])
            InsertLetter();
    }
}
