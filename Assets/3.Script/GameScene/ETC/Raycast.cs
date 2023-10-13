using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayerName
{
    Player = 0,
    Land,
    Wall,
    Enemy
}

public class Raycast : MonoBehaviour
{
    [SerializeField] private bool isPlayer; // 플레이어인지 아닌지

    [SerializeField] private LayerMask[] collisionLayer; // 부딪힐 레이어
    [SerializeField] private float distanceX = 0; // 감지 거리 X
    [SerializeField] private float distanceY = 0; // 감지 거리 Y
    public RaycastHit2D hitX, hitY;

    [Header("Enemy")]
    [SerializeField] private float playerX = 0; // 플레이어 감지 거리 Y
    [SerializeField] private float playerY = 0; // 플레이어 감지 거리 Y
    public RaycastHit2D hitPlayerX, hitPlayerY; // 플레이어를 감지할 때 사용

    [Header("Effect")]
    [SerializeField] private bool isEffect; // 이펙트인지 아닌지

    public RaycastHit2D hitEnemyX, hitEnemyY; // 적을 감지할 때 사용

    private Vector2 vectorX;
    private Vector2 vectorY = Vector2.down;

    private void Update()
    {
        RaycastDraw();
    }

    private void RaycastDraw()
    {
        float x = transform.localScale.x;

        if (isPlayer)
        {
            x *= -1;
        }

        switch (x)
        {
            case -1:
                vectorX = Vector2.right;
                break;
            case 0:
                vectorX = Vector2.zero;
                break;
            case 1:
                vectorX = Vector2.left;
                break;
        }

        if (isPlayer)
        {
            Debug.DrawRay(transform.position, vectorX * distanceX, Color.yellow);
            Debug.DrawRay(transform.position, vectorY * distanceY, Color.yellow);
        } else
        {
            if (isEffect)
            {
                Debug.DrawRay(transform.position, vectorX * distanceX, Color.white);
                Debug.DrawRay(transform.position, Vector2.up * playerY, Color.white);

                hitEnemyX = Physics2D.Raycast(transform.position, vectorX, playerX, collisionLayer[(int)LayerName.Enemy]);
                hitEnemyY = Physics2D.Raycast(transform.position, Vector2.up, playerY, collisionLayer[(int)LayerName.Enemy]);
            } else {
                Debug.DrawRay(transform.position, vectorX * distanceX, Color.red);
                Debug.DrawRay(transform.position, Vector2.up * playerY, Color.red);

                hitPlayerX = Physics2D.Raycast(transform.position, vectorX, playerX, collisionLayer[(int)LayerName.Player]);
                hitPlayerY = Physics2D.Raycast(transform.position, Vector2.up, playerY, collisionLayer[(int)LayerName.Player]);
            }
        }

        hitX = Physics2D.Raycast(transform.position, vectorX, distanceX, collisionLayer[(int)LayerName.Land]);
        hitY = Physics2D.Raycast(transform.position, vectorY, distanceY, collisionLayer[(int)LayerName.Land]);
    }
}
