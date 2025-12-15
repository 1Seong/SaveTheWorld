using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PencilGame : MonoBehaviour
{
    public float[] dropSpeeds;
    public float[] fillAmounts;
    public float[] pencilScales;
    public float gameStartTime = 3.5f;
    public float pencilGetWaitingTime = 0f;
    public float idleSfxThreshold = 0.4f;

    public Transform pencil;
    public Slider slider;
    public Transform sharpner;
    public Transform sharpnerArm;
    public GameObject instruction;
    public GameObject sliderObject;
    public Transform cam;
    public Animator boyAnim;

    private float currentValue = 0f;
    private int cnt = 0;

    private bool loopSfxPlaying = false;
    float lastPressTime = -999f;

    private void Start()
    {
        Invoke(nameof(GameStart), gameStartTime);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        currentValue -= dropSpeeds[cnt] * Time.deltaTime;
        currentValue = Mathf.Clamp01(currentValue);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            lastPressTime = Time.time;

            if(!loopSfxPlaying)
            {
                loopSfxPlaying = true;
                AudioManager.Instance.LoopSfxOn(AudioType.SFX_P_Sharpner);
            }

            sharpner.DOShakePosition(0.2f, 0.01f, 15);
            sharpnerArm.DOLocalRotate(new Vector3(0, 180f, 0), 0.3f, RotateMode.LocalAxisAdd);
            currentValue += fillAmounts[cnt];
            currentValue = Mathf.Clamp01(currentValue);
        }

        if(loopSfxPlaying && (Time.time - lastPressTime > idleSfxThreshold))
        {
            loopSfxPlaying = false;
            AudioManager.Instance.LoopSfxOff();
        }

        slider.value = currentValue;

        if(currentValue >= 1f)
        {
            MiniGameManager.instance.stop();

            currentValue = 1f;
            slider.value = 1f;
            sharpnerArm.DOKill();

            AudioManager.Instance.LoopSfxOff();

            pencil.DOLocalMoveZ(pencil.localPosition.z + 0.1f, 1.2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                pencil.gameObject.SetActive(false);
                instruction.SetActive(false);
                sliderObject.SetActive(false);
                currentValue = 0f;
                slider.value = 0f;

                // get pencil animation, and then

                Invoke(nameof(gameEnd), pencilGetWaitingTime);
            });
        }
    }

    private void init()
    {
        boyAnim.transform.DOKill();
        boyAnim.transform.DOLocalMoveY(-2.9f, 0f);

        boyAnim.SetTrigger("Start");
        Invoke(nameof(GameStart), gameStartTime);
    }

    public void GameStart()
    {
        pencil.localScale = new Vector3(1f, 1f, pencilScales[cnt]);
        pencil.gameObject.SetActive(true);
        pencil.DOLocalMoveZ(pencil.localPosition.z - 0.1f, 1.2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            MiniGameManager.instance.play();

            instruction.SetActive(true);
            sliderObject.SetActive(true);
        });
    }

    private void gameEnd()
    {
        ++cnt;

        if(cnt < MiniGameManager.instance.GameRepeatNum)
        {
            init();
        }
        else
        {
            boyAnim.transform.DOKill();
            boyAnim.transform.DOLocalMoveY(-2.9f, 0f);

            boyAnim.SetTrigger("Start");
            Invoke(nameof(showPencilCase), gameStartTime + 1);
        }
    }

    private void showPencilCase()
    {
        cam.DORotate(new Vector3(90f, 0f, 0f), 1.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Invoke(nameof(miniGameEnd), MiniGameManager.instance.GameEndTime);
        });
    }

    private void miniGameEnd()
    {
        MiniGameManager.instance.GameEnd();
    }
}
