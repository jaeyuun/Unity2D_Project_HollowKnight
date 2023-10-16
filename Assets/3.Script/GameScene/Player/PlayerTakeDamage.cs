using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] private GameObject stunEffect;
    private PlayerInfo playerInfo;
    private Animator animator;
    private bool isHurt = false;
    private float timer = 0;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
        playerInfo = transform.GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (isHurt)
        {
            timer += Time.deltaTime;
            if (timer > 0.3f)
            {
                animator.SetBool("Stun", false);
                stunEffect.SetActive(false);
            }
            if (timer > 1.5f)
            {
                gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                isHurt = false;
                timer = 0;
            }
        }
    }

    private void PlayerHurt()
    {
        playerInfo.PlayerHpInfo();
        animator.SetBool("Stun", true);
        stunEffect.SetActive(true);
        gameObject.transform.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { // enemy effect
        if (collision.gameObject.CompareTag("Enemy") && !isHurt)
        {
            if (collision.gameObject.name.Equals("SpinyHusk_Fire(Clone)"))
            {
                if (!collision.gameObject.transform.GetComponent<FireController>().isLand)
                {
                    isHurt = true;
                    PlayerHurt();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { // enemy
        if (collision.gameObject.CompareTag("Enemy") && !isHurt)
        {
            isHurt = true;
            PlayerHurt();
        }
    }
}