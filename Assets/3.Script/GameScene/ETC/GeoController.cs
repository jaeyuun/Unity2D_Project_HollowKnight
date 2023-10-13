using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoController : MonoBehaviour
{
    [SerializeField] private int price; // 1, 5, 25
    private Animator animator;
    private bool isGet = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.transform.SetParent(null);
    }

    private IEnumerator GeoDestroy_Co()
    {
        animator.SetTrigger("Get");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isGet)
            {
                PlayerInfo.PlayerMoneyInfo(price, false);
                StartCoroutine(GeoDestroy_Co());
                isGet = true;
            }
        }
    }
}
