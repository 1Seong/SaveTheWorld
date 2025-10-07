using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour, ITransition
{
    [SerializeField] Image targetImage;

    public void InTransition()
    {

    }

    public void OutTransition()
    {

    }

    public void FullTransition()
    {

    }

    public void SceneLoadTransition(string sceneName)
    {
        targetImage.transform.localScale = Vector3.zero;

        //example
        targetImage.transform.DOScale(30f, 0.6f).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
            targetImage.transform.localScale = Vector3.one * 30f;
            targetImage.transform.DOScale(0f, 0.6f);
        });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
