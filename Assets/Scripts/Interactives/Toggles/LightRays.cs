using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class LightRays : MonoBehaviour
{
    public Material targetMaterial;
    public GameObject hint;

    private void Awake()
    {
        targetMaterial.DOFade(0f, 0f);
    }

    public void LightAppear()
    {
        targetMaterial.DOFade(0.2f, 0.8f);
        hint.SetActive(true);
    }

    public void LightDisappear()
    {
        targetMaterial.DOFade(0f, 0.8f);
        hint.SetActive(false);
    }
       
}
