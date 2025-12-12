using DG.Tweening;

public class Window : ToggleInteractives
{
    public float moveDis = 2.5f;

    protected override void On()
    {
        isActing = true;
        transform.DOMoveX(transform.position.x - moveDis, 0.6f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            isActing = false;
        });
    }

    protected override void Off()
    {
        isActing = true;
        transform.DOMoveX(transform.position.x + moveDis, 0.6f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            isActing = false;
        });
    }
}
