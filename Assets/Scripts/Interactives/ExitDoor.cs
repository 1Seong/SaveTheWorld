using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] bool isUnlocked = false;
    [SerializeField] Image whiteBackground;

    bool isActing = false;

    private void Start()
    {
        GameManager.GameAllClearedEvent += ActivateDoor;
    }

    private void OnDisable()
    {
        GameManager.GameAllClearedEvent -= ActivateDoor;
    }

    public void OnClick()
    {
        if (isActing) return;

        if (isUnlocked)
        {
            ItemManager.Instance.gameObject.SetActive(false);
            NoteManager.Instance.TurnOff();
            whiteBackground.gameObject.SetActive(true);
            whiteBackground.DOFade(1f, 1f).OnComplete(() =>
            {
                SaveManager.Instance.SaveAll();
                SceneManager.LoadScene("Ending");
            });
        }
        else
        {
            isActing = true;
            transform.DOShakePosition(0.3f, 0.12f, 20).OnComplete(() =>
            {
                isActing = false;
            });
        }
    }

    private void ActivateDoor()
    {
        isUnlocked = true;

        GetComponent<Image>().DOFade(0.003f, 0f);
        GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);
    }
}
