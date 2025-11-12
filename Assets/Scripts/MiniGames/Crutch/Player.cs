using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int health = 5;
    public float invincibilityTime = 2f;
    public float jumpPower = 1f;
    public float jumpDur = 0.5f;
    public Camera cam;
    public TextMeshProUGUI healthTmp;
    public Movables movables;
    public Animator explotionAnim;
    public Image whiteBackground;

    bool isInvincible = false;
    bool isJumping = false;

    SpriteRenderer[] spriters;
    Animator anim;
    Rigidbody2D rigid;

    private void Awake()
    {
        spriters = GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible && collision.CompareTag("Bullet"))
        {
            isInvincible = true;

            foreach (var i in spriters)
                i.DOFade(0.5f, 0f);

            anim.SetTrigger("Hit");
            cam.DOShakePosition(0.2f, 0.3f, 20);
            movables.SlowDown();
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
        foreach (var i in spriters)
            i.DOFade(1f, 0f);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying || !MiniGameManager.instance.IsPlaying) return;

        if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            rigid.DOJump(rigid.position, jumpPower, 1, jumpDur).OnComplete(() =>
            {
                isJumping = false;
            });
        }
    }

    public void Die()
    {
        transform.DOLocalMoveY(-0.52f, 0.3f).OnComplete(() =>
        {
            transform.DOMoveX(transform.position.x + 1f, 3.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                cam.DOShakePosition(0.2f, 0.5f, 20);
                explotionAnim.SetTrigger("Explode");
                anim.SetTrigger("Die");
                transform.DOMoveX(transform.position.x - 0.7f, 0.6f).SetEase(Ease.OutExpo).OnComplete(() =>
                {
                    whiteBackground.DOFade(1f, 0.4f).OnComplete(() =>
                    {
                        MiniGameManager.instance.GameEnd();
                    });
                });
            });
        });
    }
}
