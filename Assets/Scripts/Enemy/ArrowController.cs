using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonDefinitions;

public class ArrowController : MonoBehaviour
{
    public int travelDistance;

    private Vector3 startPosition;
    private Vector2 direction;

    private Rigidbody2D rb;

    private static Vector3 STARTING_OFFSET_UP = new Vector3(0, 16 * OFFSET_PER_PIXEL, 0);
    private static Vector3 STARTING_OFFSET_DOWN = new Vector3(7 * OFFSET_PER_PIXEL, -8 * OFFSET_PER_PIXEL, 0);
    private static Vector3 STARTING_OFFSET_SIDE = new Vector3(11 * OFFSET_PER_PIXEL, 0, 0);
    private static int ARROW_LAYER = 11;
    private static int ENEMY_LAYER = 12;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.centerOfMass = new Vector3(0, 0, 1);

        Physics2D.IgnoreLayerCollision(ARROW_LAYER, ENEMY_LAYER);
    }

    public void Fire(Vector2 fireDirection, float fireAngle)
    {
        startPosition = transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, fireAngle - 90));
        direction = fireDirection;

        if(!WillHitWithinStartingDistance())
            OffsetStartPosition();
    }

    private bool WillHitWithinStartingDistance()
    {
        float distanceToCheck = (direction == Vector2.up) ? STARTING_OFFSET_UP.y : (direction == Vector2.down) ? -STARTING_OFFSET_DOWN.y : STARTING_OFFSET_SIDE.x;

        ContactFilter2D hitFilter = new ContactFilter2D();
        hitFilter.SetLayerMask(LayerMask.GetMask("Ground", "Player"));

        return Physics2D.Raycast(startPosition, direction, hitFilter, new List<RaycastHit2D>(), distanceToCheck) > 0;
    }

    private void OffsetStartPosition()
    {
        if (direction == Vector2.up)
            startPosition += STARTING_OFFSET_UP;
        else
        {
            bool isRight = transform.rotation.y == 0;
            int sign = (isRight) ? 1 : -1;

            if (direction == Vector2.down)
            {
                Vector3 directionOffset = STARTING_OFFSET_DOWN;
                directionOffset.x *= sign; // go to left of player if facing left

                startPosition += directionOffset;
            }
            else if (direction == Vector2.right || direction == Vector2.left)
                startPosition += sign * STARTING_OFFSET_SIDE;
        }

        transform.position = startPosition;
    }

    void FixedUpdate()
    {

        if (direction != Vector2.zero)
        {
            rb.AddForce(direction, ForceMode2D.Impulse);

            System.Func<float, float, bool> isInAir = (pos, startPos) => pos > (startPos + travelDistance) || pos < (startPos - travelDistance);

            if ((direction.y > 0 && isInAir(transform.position.y, startPosition.y)) || isInAir(transform.position.x, startPosition.x))
                rb.gravityScale += ((direction.y > 0 && rb.velocity.y == 0) || (direction.y <= 0 && rb.velocity.x == 0)) ? 0 : .1f;
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Sword") || col.gameObject.name == "Tilemap")
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            col.gameObject.GetComponent<PlayerHealth>().PlayerDamage(1, direction);
        }
    }
}
