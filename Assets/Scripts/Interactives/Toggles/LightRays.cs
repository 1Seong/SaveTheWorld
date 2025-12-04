using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class LightRays : MonoBehaviour
{
    public Material targetMaterial;

    private void Awake()
    {
        targetMaterial.DOFade(0f, 0f);
    }

    public void LightAppear()
    {
        targetMaterial.DOFade(0.3f, 0.8f);
    }

    public void LightDisappear()
    {
        targetMaterial.DOFade(0f, 0.8f);
    }
       
}
