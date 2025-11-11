using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int health = 5;
    public float invincibilityTime = 2f;
    public float jumpDis = 0.4f;
    public float jumpDur = 0.5f;
    public Camera cam;
    public TextMeshProUGUI healthTmp;
    public Movables movables;
    public Animator explotionAnim;
    public Image whiteBackground;

    bool isInvincible = false;

    SpriteRenderer spriter;
    Animator anim;
    Rigidbody rigid;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInvincible && collision.collider.CompareTag("Bullet"))
        {
            isInvincible = true;
            spriter.DOFade(0.5f, 0f);
            anim.SetTrigger("Hit");
            cam.DOShakePosition(0.3f);
            //movables.slowdown();
            --health;
            if (health > 0)
                healthTmp.text = health.ToString();
            else if (health == 0)
                healthTmp.text = "0.1";
            else
            {
                string s = healthTmp.text;
                healthTmp.text = s.Insert(2, "0");
            }

            Invoke(nameof(turnOffInvincible), invincibilityTime);
        }
    }

    private void turnOffInvincible()
    {
        isInvincible = false;
        spriter.DOFade(1f, 0f);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rigid.DOJump(rigid.position + new Vector3(0f, jumpDis, 0f), 1, 1, jumpDur);
        }
    }

    public void Die()
    {
        transform.DOMoveX(transform.position.x + 1f, 2f).OnComplete(() =>
        {
            explotionAnim.SetTrigger("Explode");
            anim.SetTrigger("Die");
            transform.DOMoveY(transform.position.y - 0.5f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                whiteBackground.DOFade(1f, 0.4f).OnComplete(() =>
                {
                    MiniGameManager.instance.GameEnd();
                });
            });
        });
    }
}
