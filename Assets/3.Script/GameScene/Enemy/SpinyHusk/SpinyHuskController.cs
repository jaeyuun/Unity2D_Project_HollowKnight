using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpinyHusk
{
    Idle = 0,
    Walk,
    Turn,
    Attack,
    Dead,
    Burst,
}

public class SpinyHuskController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private Raycast raycast;
    private EnemyHp enemyHp;
    private EnemyGeoDead enemyDead;

    [SerializeField] private GameObject skillEffect;
    [SerializeField] Quaternion[] skillRotation;

    private int random;
    private bool isTurn;
    private bool isAttack;
    private bool isDead; // 죽었을 때 한번만 코루틴 실행
    private int attackCount = 1;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();
        raycast = GetComponent<Raycast>();
        enemyHp = GetComponent<EnemyHp>();
        enemyDead = GetComponent<EnemyGeoDead>();

        isDead = false;
    }

    private void Update()
    {
        if (enemyHp.enemyHp <= 0)
        { // 죽었을 때 한 번
            if (!isDead)
            {
                StartCoroutine(DeadAnimation_Co());
            } else
            {

            }            
        } 
        else if (enemyHp.enemyHp > 0)
        {
            if (!isAttack)
            {
                SpinyHuskMovement();
            }
            else
            {
                if (attackCount == 1)
                {
                    StartCoroutine(SpinyHuskAttack_Co());
                }
            }
        }
    }

    private void SpinyHuskMovement()
    {
        if (raycast.hitPlayerX || raycast.hitPlayerY)
        { // Player와 닿은 RaycastHit이 null이 아닐 때
            isAttack = true;
            return;
        } else if (!raycast.hitPlayerX && !raycast.hitPlayerY)
        { // Player와 닿은 RaycastHit이 전부 null이 되었을 때

        }
        if (raycast.hitX)
        {
            isTurn = true;
            animator.SetBool("Turn", true);
        }
        StartCoroutine(RandomAnimation_Co());

        movement2D.DefaultMovement();
    }

    private IEnumerator RandomAnimation_Co()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        { // 애니메이션 동작 시
            yield return null;
        }
        // 애니메이션 동작 후
        if (isTurn)
        {
            animator.SetBool("Turn", false);
            isTurn = false;

            if (transform.localScale.x == 1)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (transform.localScale.x == -1)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        random = Random.Range(0, 2);
        switch (random)
        {
            case 0: // Idle
                animator.SetBool("Walk", false);
                movement2D.MoveTo(new Vector3(0, 0, 0));
                break;
            case 1: // Walk
                animator.SetBool("Walk", true);
                movement2D.MoveTo(new Vector3(transform.localScale.x * -1, 0, 0));
                break;
        }
    }

    private IEnumerator SpinyHuskAttack_Co()
    {
        // 일정 거리에 들어왔을 때 공격, 딜레이 있음
        StartCoroutine(AttackCreate_Co());
        attackCount--;
        yield return new WaitForSeconds(3.0f);
        attackCount = 1;
    }

    private IEnumerator AttackCreate_Co()
    {
        animator.SetBool("Attack", true);
        movement2D.MoveTo(new Vector3(0, 0, 0));

        yield return new WaitForSeconds(1.0f);
        // 공격 발사체 생성
        for (int i = 0; i < skillRotation.Length; i++)
        {
            Instantiate(skillEffect, transform.position, skillRotation[i]);
        }

        animator.SetBool("Attack", false);
        yield return null;
        isAttack = false;
    }

    private IEnumerator DeadAnimation_Co()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        movement2D.MoveTo(new Vector3(0, 0, 0));

        yield return null;

        enemyDead.GeoCreate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slash") && isDead)
        {
            // 이펙트 추가할 것... todo
            Destroy(gameObject);
        }
    }
}
