using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour, ITransition, IRoomTransition
{
    [SerializeField] Image targetImage;

    public void SceneLoadTransition(string sceneName)
    {
        targetImage.transform.localScale = Vector3.zero;
        (targetImage.transform as RectTransform).anchoredPosition = Vector3.zero;

        targetImage.transform.DOScale(250f, 0.6f).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            targetImage.transform.localScale = Vector3.one * 250f;
            targetImage.transform.DOScale(0f, 0.6f);
        });
    }

    public void SceneUnloadTransition()
    {
        targetImage.transform.localScale = Vector3.zero;
        (targetImage.transform as RectTransform).anchoredPosition = Vector3.zero;

        targetImage.transform.DOScale(250f, 0.6f).OnComplete(() =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name);
            targetImage.transform.localScale = Vector3.one * 250f;
            targetImage.transform.DOScale(0f, 0.6f);
        });
    }

    public void RoomLeftTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(-2000f, 0, 0);
        
        targetImage.transform.DOLocalMoveX(2000f, 0.8f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(2000f, 0f, 0f);
        });
        
    }

    public void RoomRightTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(2000f, 0, 0);

        targetImage.transform.DOLocalMoveX(-2000f, 0.8f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(-2000f, 0f, 0f);
        });
    }


    public void RoomCeilingTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(0, 1850f, 0);

        targetImage.transform.DOLocalMoveY(-1850f, 0.8f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(0f, -1850f, 0f);
        });
    }

    public void RoomSideReturnTransition()
    {
        targetImage.transform.localScale = new Vector3(200f, 200f, 0);
        targetImage.transform.localPosition = new Vector3(0, -1850f, 0);

        targetImage.transform.DOLocalMoveY(1850f, 0.8f).SetUpdate(true).SetEase(Ease.OutSine).OnComplete(() =>
        {
            targetImage.transform.localPosition = new Vector3(0f, 1850f, 0f);
        });
    }
}
