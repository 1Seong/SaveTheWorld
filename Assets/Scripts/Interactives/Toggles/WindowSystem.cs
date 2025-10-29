using UnityEngine;
using DG.Tweening;

public class WindowSystem : MonoBehaviour
{
    [SerializeField] private ToggleInteractives windowObj;
    [SerializeField] private ToggleInteractives curtainObj;
    [SerializeField] private LightRays fakeLight;

    public void OnClick() // Caution : put this on click function to the last
    {
        if(windowObj.IsActive &&  curtainObj.IsActive)
        {
            fakeLight.LightAppear();
        }
        else
        {
            fakeLight.LightDisappear();
        }
    }
}
