using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BalanceGame : MonoBehaviour
{
    public float controlStrength = 3f; // 플레이어 입력 반응
    public float damping = 0.9f;       // 관성 소멸 (0~1)
    public float failAngle = 60f;
    public float controlStrengthStep = 0.1f;

    public Sprite[] playerSprites;

    public Image background;
   
    float velocity = 0f;

    [SerializeField] private Vector2 _windForce = Vector2.zero;
    public Vector2 WindForce
    {
        get => _windForce;
        set => _windForce = value;
    }
    public Transform player;

    [Header("Particle")]
    public ParticleSystem windParticleLeft;
    public ParticleSystem windParticleRight;
    public float baseSpeed = 3f;
    public float speedScale = 1f;
    public float baseEmission = 3f;
    public float emissionScale = 1f;

    public static BalanceGame instance;

    ParticleSystem.MainModule mainLeft;
    ParticleSystem.EmissionModule emissionLeft;
    ParticleSystem.MainModule mainRight;
    ParticleSystem.EmissionModule emissionRight;

    void Awake()
    {
        mainLeft = windParticleLeft.main;
        emissionLeft = windParticleLeft.emission;
        mainRight = windParticleRight.main;
        emissionRight = windParticleRight.emission;

        instance = this;
    }

    public void ApplyWindParticle()
    {
        float dir = Mathf.Sign(WindForce.x);                   // -1 or 1 or 0
        float mag = Mathf.Abs(WindForce.x);                    // 강도

        if(dir > 0)
        {
            emissionLeft.enabled = true;
            emissionRight.enabled = false;
        }
        else
        {
            emissionLeft.enabled = false;
            emissionRight.enabled = true;
        }

        // 속도 조정
        mainLeft.startSpeed = mainRight.startSpeed = baseSpeed + mag * speedScale;

        // 파티클 양(바람이 강할수록 많이 날림)
        emissionLeft.rateOverTime =  emissionRight.rateOverTime = baseEmission + mag * emissionScale;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        controlStrength += controlStrengthStep * Time.deltaTime;

        float input = Input.GetAxis("Horizontal"); // -1 ~ 1
        float control = input * controlStrength * Time.deltaTime;

        // 바람은 외력: WindForce.x (유닛/초)
        velocity += control + WindForce.x * Time.deltaTime;

        // damping
        velocity *= Mathf.Pow(damping, Time.deltaTime * 60f);

        // 시각적 기울기(선택)
        player.localRotation = Quaternion.Euler(0f, 0f, -velocity);

        ApplyWindParticle();

        float tilt = player.localEulerAngles.z;

        // 0~360 범위를 -180~180 범위로 변환
        if (tilt > 180f) tilt -= 360f;

        if(Mathf.Abs(tilt) < failAngle * 0.3f)
            player.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
        else if(Mathf.Abs(tilt) < failAngle * 0.6f)
            player.GetComponent<SpriteRenderer>().sprite = playerSprites[1];
        else
            player.GetComponent<SpriteRenderer>().sprite = playerSprites[2];

        // 실패 판정
        if (Mathf.Abs(tilt) > failAngle)
        {
            MiniGameManager.instance.stop();

            AudioManager.Instance.PlaySfx(AudioType.SFX_L_Die);

            background.DOFade(1f, 0f);

            Invoke(nameof(miniGameEnd), MiniGameManager.instance.GameEndTime);
        }
    }

    private void miniGameEnd()
    {
        MiniGameManager.instance.GameEnd();
    }
}
