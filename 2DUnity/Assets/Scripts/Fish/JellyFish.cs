using UnityEngine;

public class JellyFish : Fish
{
    private bool isMovingUp = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float moveDir = isMovingUp ? 1f : -1f;
        rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, moveDir * fishConfig.moveSpeed); // ← X대신 Y축 이동

        float distanceMoved = transform.position.y - startPos.y;

        if (Mathf.Abs(distanceMoved) >= fishConfig.moveDistance)
        {
            isMovingUp = !isMovingUp;
            startPos = transform.position;
        }
    }
}
