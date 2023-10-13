using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private Rigidbody2D rigidbody;

    private Vector3 moveDirection = Vector3.zero;
    public float moveSpeed = 0f;
    public float jumpForce = 0f;
    public float jumpTime = 0;

    [Header("Hornet")]
    [SerializeField] private float moveForce;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void JumpMovement()
    {
        rigidbody.velocity = Vector3.zero;
        transform.position += Vector3.up * jumpForce;
    }

    public void AccelMovement()
    {
        rigidbody.AddForce(moveDirection * moveForce, ForceMode2D.Impulse);
    }

    public void DefaultMovement()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
