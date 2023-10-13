using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBound : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] private GameObject player; // 바운딩 시킬 오브젝트
    [SerializeField] private float boundForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            ExcecuteBounding(collision);
        }
        if (!isPlayer)
        {
            if (collision.gameObject.CompareTag("Land"))
            {
                ExcecuteBounding(collision);
            }
        }
    }

    private void ExcecuteBounding(Collision2D collision)
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        ContactPoint2D cp = collision.GetContact(0); // 충돌 시 충돌되는 접점의 정보 반환, trigger에서는 사용할 수 없음
        Vector2 dir = playerPosition - cp.point; // 접촉 지점부터 플레이어의 방향
        player.transform.GetComponent<Rigidbody2D>().AddForce((dir).normalized * boundForce);
    }
}
