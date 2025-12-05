using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        transform.GetChild(1).GetComponent<Image>().DOFade(1f, 0.3f);
        NoteManager.Instance.OnInsertLetter((int)interactiveType, letterId);
    }

    public void ApplyLetterData(Dictionary<int, bool> dic)
    {
        if (dic[letterId])
            InsertLetter();
    }
}
