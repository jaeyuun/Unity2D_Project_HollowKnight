using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;

    private bool isSlash;
    public bool isLand;
    private Vector3 fireDirection;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();

        isSlash = false;
        isLand = false;
        fireDirection = transform.up;
    }

    private void Update()
    {
        FireMovement();
    }

    private void FireMovement()
    {
        if (!isLand)
        {
            movement2D.MoveTo(fireDirection);
            movement2D.DefaultMovement();
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slash"))
        {
            isSlash = true;
            StartCoroutine(FireBreak_Co());
        }

        if (collision.gameObject.CompareTag("Land"))
        {
            isLand = true;
            animator.SetTrigger("Land");
            fireDirection = Vector3.zero;
            StartCoroutine(FireBreak_Co());
        }
    }

    private IEnumerator FireBreak_Co()
    {
        if (!isSlash)
        { // 공격을 했을 때 바로 없어져야하고, 땅에 떨어졌을 땐 천천히 없어지기
            yield return new WaitForSeconds(2f);
        }
        animator.SetTrigger("Break");
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
