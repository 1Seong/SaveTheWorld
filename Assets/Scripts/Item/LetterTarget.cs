using UnityEditor;
using UnityEngine;

public class LetterTarget : ItemTarget
{
    [SerializeField] private Item.Interactives interactiveType;

    protected override void itemMatched()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        NoteManager.Instance.OnInsertLetter(interactiveType);
    }
}
