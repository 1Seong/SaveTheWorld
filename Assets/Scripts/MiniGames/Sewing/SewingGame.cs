using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SewingGame : MonoBehaviour
{
    public static SewingGame instance;
    public Image background;
    public MeshRenderer clothMeshRenderer;
    public Material[] clothMaterials;
    public TrailRenderer trailRenderer;
    public Material trailMaterial;
    public Transform movingPoint;
    public PathGenerator pathGenerator;
    public float[] pathWidthes;
    public float[] pathNoises;

    public int cnt = 0;

    private void Awake()
    {
        instance = this;
    }

    public void GameEnd()
    {
        AudioManager.Instance.LoopSfxOff();

        MiniGameManager.instance.IsPlaying = false;

        ++cnt;

        if(cnt < MiniGameManager.instance.GameRepeatNum)
        {
            Invoke(nameof(Restart), 1.5f);
        }
        else
        {
            Invoke(nameof(miniGameEnd), MiniGameManager.instance.GameEndTime);
        }
        
    }

    private void miniGameEnd()
    {
        MiniGameManager.instance.GameEnd(0);
    }

    private void Restart()
    {
        background.DOFade(1f, 1f).OnComplete(() =>
        {
            clothMeshRenderer.material = clothMaterials[cnt];
            trailRenderer.startColor = Color.red;
            trailRenderer.endColor = Color.red;
            trailMaterial.color = Color.red;
            movingPoint.localPosition = new Vector3(0f, 0f, 4.71f);
            trailRenderer.Clear();
            pathGenerator.width = pathWidthes[cnt];
            pathGenerator.noiseScale = pathNoises[cnt];
            pathGenerator.GeneratePath();

            MiniGameManager.instance.IsPlaying = true;
            Invoke(nameof(backgroundDisappear), 0.5f);
        });
    }

    private void backgroundDisappear()
    {
        background.DOFade(0f, 1f);
    }
}
