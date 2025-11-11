using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DinoGame : MonoBehaviour
{
    public UniversalRendererData urpData;
    public Material crtMat;

    void Start()
    {
        FullScreenPassRendererFeature rf;

        if (urpData.TryGetRendererFeature(out rf))
        {
            rf.passMaterial = crtMat;
            rf.SetActive(true);
        }

        
    }

    private void OnDisable()
    {
        FullScreenPassRendererFeature rf;

        if (urpData.TryGetRendererFeature(out rf))
        {
            rf.passMaterial = null;
            rf.SetActive(false);
        }

    }
}
