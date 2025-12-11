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

        targetImage.transform.DOScale(250f, 1.5f).OnComplete(() =>
        {
            if(isAdditive)
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            else
                SceneManager.LoadScene(sceneName);
            targetImage.transform.localScale = Vector3.one * 250f;
            targetImage.transform.DOScale(0f, 1.5f);
        });
    }

    public void SceneUnloadTransition()
    {
        targetImage.transform.localScale = Vector3.zero;
        (targetImage.transform as RectTransform).anchoredPosition = Vector3.zero;

        targetImage.transform.DOScale(250f, 1.5f).OnComplete(() =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1).name);
            targetImage.transform.localScale = Vector3.one * 250f;
            targetImage.transform.DOScale(0f, 1.5f);
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
