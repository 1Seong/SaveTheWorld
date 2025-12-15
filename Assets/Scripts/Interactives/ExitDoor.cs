using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] bool isUnlocked = false;

    bool isActing = false;

    private void Awake()
    {
        GameManager.GameAllClearedEvent += ActivateDoor;
    }

    private void OnDestroy()
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
            SceneTransition.Instance.WhiteBackground.gameObject.SetActive(true);

            SceneTransition.Instance.WhiteBackground.DOFade(1f, 4f).OnComplete(() =>
            {
                AudioManager.Instance.PlayBgm(AudioType.BGM_Ending);

                SaveManager.Instance.SaveAll();
                SceneManager.LoadScene("Ending");
                SceneTransition.Instance.WhiteBackground.DOFade(0f, 4f);
            });
        }
        else
        {
            isActing = true;

            AudioManager.Instance.PlaySfx(AudioType.SFX_Room_Door);

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
