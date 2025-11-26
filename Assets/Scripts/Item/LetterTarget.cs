using System;
using UnityEngine;

public class LetterTarget : ItemTarget
{
    public static event Action tutorialEvent;

    [SerializeField] private Item.Interactives interactiveType;

    protected override void itemMatched()
    {
        tutorialEvent?.Invoke();

        transform.GetChild(0).gameObject.SetActive(true);
        NoteManager.Instance.OnInsertLetter(interactiveType);
    }
}
