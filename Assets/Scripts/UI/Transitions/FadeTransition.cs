using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour, ITransition, IRoomTransition
{
    [SerializeField] Image targetImage;

    public void SceneLoadTransition(string sceneName, bool isAdditive)
    {
        targetImage.transform.localScale = Vector3.zero;
        (targetImage.transform as RectTransform).anchoredPosition = Vector3.zero;

        if(sceneName != "Ending2")
            AudioManager.Instance.StopBgm();
        AudioManager.Instance.LoopSfxOn(AudioType.SFX_Etc_SceneTrans);

        targetImage.transform.DOScale(250f, 1.5f).OnComplete(() =>
        {
            AudioManager.Instance.LoopSfxOff();

            if (isAdditive) // TV, MiniGame
            {
                if(sceneName == "TV")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_TV1);
                }
                else if(sceneName == "Syringe")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Syringe);
                }
                else if(sceneName == "Crutches")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Crutch);
                }
                else if(sceneName == "Jar")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Jar);
                }
                else if(sceneName == "Pencil")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Pencil);
                }
                else if(sceneName == "Sewing")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Sewing);
                }
                else if(sceneName == "Laundry")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Laundry);
                }

                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else // Ending, MainMenu, MainScene
            {
                if(sceneName == "Ending1")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Ending);
                }
                else if(sceneName == "MainMenu")
                {
                    AudioManager.Instance.PlayBgm(AudioType.BGM_Title);
                }

                SceneManager.LoadScene(sceneName);
            }
            targetImage.transform.localScale = Vector3.one * 250f;

            AudioManager.Instance.LoopSfxOn(AudioType.SFX_Etc_SceneTrans);
            targetImage.transform.DOScale(0f, 1.5f).OnComplete(() =>
            {
                AudioManager.Instance.LoopSfxOff();
            });

        });
    }

    public void SceneUnloadTransition() // Used in MiniGame, TV Scenes
    {
        targetImage.transform.localScale = Vector3.zero;
        (targetImage.transform as RectTransform).anchoredPosition = Vector3.zero;

        AudioManager.Instance.StopBgm();

        AudioManager.Instance.LoopSfxOn(AudioType.SFX_Etc_SceneTrans);
        targetImage.transform.DOScale(250f, 1.5f).OnComplete(() =>
        {
            AudioManager.Instance.LoopSfxOff();

            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name);
            targetImage.transform.localScale = Vector3.one * 250f;

            AudioManager.Instance.LoopSfxOn(AudioType.SFX_Etc_SceneTrans);
            targetImage.transform.DOScale(0f, 1.5f).OnComplete(() =>
            {
                AudioManager.Instance.LoopSfxOff();
            });
        });
    }

    public void RoomLeftTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(-2000f, 0, 0);
        
        targetImage.transform.DOLocalMoveX(2000f, 0.9f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(2000f, 0f, 0f);
        });
        
    }

    public void RoomRightTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(2000f, 0, 0);

        targetImage.transform.DOLocalMoveX(-2000f, 0.9f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(-2000f, 0f, 0f);
        });
    }


    public void RoomCeilingTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(0, 1850f, 0);

        targetImage.transform.DOLocalMoveY(-1850f, 0.9f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(0f, -1850f, 0f);
        });
    }

    public void RoomSideReturnTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(0, -1850f, 0);

        targetImage.transform.DOLocalMoveY(1850f, 0.9f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(0f, 1850f, 0f);
        });
    }
}
