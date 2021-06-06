using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonDefinitions;

public class SwordController : MonoBehaviour
{
    public bool CanHangOnSword { get; private set; }

    private AudioSource swordStickAudio;
    private GameObject player;
    private PlayerSettings settings;
    private AudioManager   audioManager;

    //public Sprite isFlying;
    //public Sprite inWall;
    //public Sprite onGround;
    //public float  travelDistance;

    private Vector3 startPosition;
    private Vector2 direction;
    private bool hit;

    bool isFalling = false; //Detects falling from upward throw

    //private SpriteRenderer currentSprite;
    private Rigidbody2D rb;
    private new BoxCollider2D collider;

    private static Vector3 STARTING_OFFSET_UP   = new Vector3(4 * OFFSET_PER_PIXEL, 3 * OFFSET_PER_PIXEL, 0);
    private static Vector3 STARTING_OFFSET_DOWN = new Vector3(7 * OFFSET_PER_PIXEL, -8 * OFFSET_PER_PIXEL, 0);
    private static Vector3 STARTING_OFFSET_SIDE = new Vector3(11 * OFFSET_PER_PIXEL, 0, 0);

    void Start()
    {
        swordStickAudio = GetComponent<AudioSource>();
        //currentSprite = GetComponent<SpriteRenderer>();
        collider      = GetComponent<BoxCollider2D>();
        rb            = GetComponent<Rigidbody2D>();
        settings = GameObject.Find("Player State").GetComponent<PlayerStateManager>().Settings;
        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();

        hit = false;

        player = GameObject.FindGameObjectWithTag("Player"); //Caches results of player object
        Physics2D.IgnoreCollision(collider, player.GetComponent<BoxCollider2D>()); //Ensures player does not bump into sword during flight
    }

    public void Throw(Vector2 throwDirection)
    {
        startPosition = transform.position;
        direction     = throwDirection;

        if(!WillHitWithinStartingDistance())
        {
            startPosition     += OffsetStartPosition();
            transform.position = startPosition;
        }

        RotateOnThrow();

        if(direction.y < 0) // allow sword to be picked up
            isFalling = true;

        //rb.AddForce(direction, ForceMode2D.Impulse);
    }

    private bool WillHitWithinStartingDistance()
    {
        float distanceToCheck = (direction == Vector2.up) ? STARTING_OFFSET_UP.y : (direction == Vector2.down) ? -STARTING_OFFSET_DOWN.y : STARTING_OFFSET_SIDE.x;

        ContactFilter2D hitFilter = new ContactFilter2D();
        hitFilter.SetLayerMask(LayerMask.GetMask("Ground", "Enemy"));

        return Physics2D.Raycast(startPosition, direction, hitFilter, new List<RaycastHit2D>(), distanceToCheck) > 0;
    }

    private Vector3 OffsetStartPosition()
    {
        Vector3 offset;

        bool isRight = transform.rotation.y == 0;
        int  sign    = (isRight) ? 1 : -1;

        if(direction == Vector2.up || direction == Vector2.down)
        {
            Vector3 directionOffset = ( direction == Vector2.up ) ? STARTING_OFFSET_UP : STARTING_OFFSET_DOWN;
            directionOffset.x *= sign; // go to left of player if facing left

            offset = directionOffset;
        }
        else
            offset = sign * STARTING_OFFSET_SIDE;

        return offset;
    }
    //private Quaternion initialRotation;
    private void RotateOnThrow()
    {
        /*if(direction == Vector2.up || direction == Vector2.down)
        {
            int sign = (direction == Vector2.up) ? 1 : -1;

            transform.Rotate(new Vector3(0, 0, sign * 90));
        }*/
        //transform.Rotate(new Vector3(0, direction.x, direction.y));

        int yRotation = direction.x < 0 ? 180 : 0;

        if(direction.x == -.0001f)
            direction = direction.y > 0 ? Vector2.up : Vector2.down;

        //Debug.Log("direction: " + direction);
        transform.rotation = Quaternion.Euler(0, yRotation, direction.y * 90);
       // transform.rotation = initialRotation;
       // Debug.Log("sword rotation: " + transform.rotation);
    }

    void FixedUpdate()
    {
        if(direction != Vector2.zero && !hit)
        {
            /*if(isFalling == false) //Temporary fix to downward sword falls being impulsed down like a rocket
                rb.AddForce(direction, ForceMode2D.Impulse);

            System.Func<float, float, bool> isInAir = (pos, startPos) => pos > (startPos + travelDistance) || pos < (startPos - travelDistance);

            if((direction == Vector2.up && isInAir(transform.position.y * 3, startPosition.y)) || isInAir(transform.position.x, startPosition.x)) //Added *3 to transform.position.y so sword can't go as high vertically
                rb.gravityScale += ((direction == Vector2.up && rb.velocity.y == 0) || (direction != Vector2.up && rb.velocity.x == 0)) ? 0 : .1f;

            if(direction == Vector2.up && rb.velocity.y < 0)
            {
                transform.Rotate(new Vector3(0, 0, -180));
                direction = Vector2.down;
                isFalling = true;
                Physics2D.IgnoreCollision(collider, player.GetComponent<Collider2D>(), false); //Allow sword to hit player and return on fall
            }*/

            if(direction != Vector2.down || (direction == Vector2.down && rb.velocity.y > -10f) )
                rb.AddForce(direction, ForceMode2D.Impulse);

            rb.gravityScale += settings.SwordGravityScaleIncrement;

            //transform.right = rb.velocity;

           /* if(direction.y > 0 && rb.velocity.y < 0 && !isFalling)  // went up and is now falling
            {
                // temporary rotation at peak of arc by mirroring
                //transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, -transform.eulerAngles.z);

                isFalling = true;
                Physics2D.IgnoreCollision(collider, player.GetComponent<Collider2D>(), false); //Allow sword to hit player and return on fall
            }*/

            if(direction.y > 0 && rb.velocity.y < 0 && !isFalling)  // went up and is now falling
            {
                if(direction == Vector2.up)
                    transform.Rotate(new Vector3(0, 0, -180));

                isFalling = true;
                Physics2D.IgnoreCollision(collider, player.GetComponent<Collider2D>(), false); //Allow sword to hit player and return on fall
            }

            if(direction != Vector2.down && direction != Vector2.up)
            {
                //float y = direction.x > 0 ? rb.velocity.y : direction.x != 0 ? rb.velocity.x : 0;
                //float x = direction.x > 0 ? rb.velocity.x : direction.x != 0 ? rb.velocity.y : 0;

                int sign = direction.x >= 0 ? 1 : -1;

                float angle = Mathf.Atan2(rb.velocity.y, Mathf.Abs(rb.velocity.x)) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, direction.x < 0 ? 180 : 0, transform.eulerAngles.z);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy")) //Will register damage on Enemy objects
        {
            col.gameObject.SendMessage("Damage", 1);

            player.GetComponent<PlayerThrow>().HitEnemy();
        }
        else if(col.gameObject.CompareTag("Player") && isFalling)
        {
            player.GetComponent<PlayerThrow>().ResetSword();
        }
        else if(col.gameObject.CompareTag("Boss") || col.gameObject.CompareTag("BossHeart"))
        {
            Debug.Log("Hit boss or heart: " + col.gameObject.tag);

            if(col.gameObject.CompareTag("BossHeart"))
                col.gameObject.GetComponent<HeartHitBehavior>().HeartHit();

            player.GetComponent<PlayerThrow>().ResetSword();
        }

        if(!hit && col.gameObject.layer != LayerMask.NameToLayer("Default") && col.gameObject.layer != LayerMask.NameToLayer("Player")) // no sticking to default or player layer while flying
        {
            hit = true;

            SwordStick(col);
            //UpdateSprite(col);
        }

        //isFalling = false; //Reset falling value
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(this != null)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Respawn"))
            {
                UnityEngine.Tilemaps.Tilemap tilemap = collision.GetComponent<UnityEngine.Tilemaps.Tilemap>();

                float      offset       = rb.velocity.y > 0 ? 0 : -.5f;
                Vector3Int tilePosition = tilemap.WorldToCell(transform.position + new Vector3(0, offset, 0));

                AudioClip clip = audioManager.SwordAudio.RespawnHitSound;

                UnityEngine.Tilemaps.TileBase tile = tilemap.GetTile(tilePosition);
                if(tile != null)
                {
                    if(tile.name.Contains("Water"))
                        clip = audioManager.SwordAudio.WaterHitSound;
                    else if(tile.name.Contains("Spikes"))
                        clip = audioManager.SwordAudio.SpikesHitSound;
                }
                else
                    Debug.Log("tile at pos " + tilePosition + " is null");

                AudioSource.PlayClipAtPoint(clip, transform.position);
                player.GetComponent<PlayerThrow>().ResetSword();
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Respawn"))
        {
            //if(isFalling || direction == Vector2.down)
             //   transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            //else


            //player.GetComponent<PlayerThrow>().ResetSword();
        }
        else if(col.gameObject.layer == LayerMask.NameToLayer("Water"))
            player.GetComponent<PlayerThrow>().ResetSword();
    }*/

    void SwordStick(Collision2D col)
    {
        swordStickAudio.Play();

        if (direction == Vector2.up && !isFalling)
            CanHangOnSword = true;

        gameObject.transform.parent = col.transform;

        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        Physics2D.IgnoreCollision(collider, player.GetComponent<BoxCollider2D>(), false);
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }

    /*void UpdateSprite(Collision2D col)
    {
        bool isOnGround = Mathf.Approximately(col.contacts[0].normal.y, 1);

        if(direction == Vector2.down)
            isOnGround = false;

        currentSprite.sprite = (isOnGround) ? onGround : inWall;

        if(isOnGround)
        {
            collider.offset = new Vector2(OFFSET_PER_PIXEL * 1, OFFSET_PER_PIXEL * -5);
            collider.size   = new Vector2(OFFSET_PER_PIXEL * 12, OFFSET_PER_PIXEL * 6);

            transform.position = new Vector3(transform.position.x, transform.position.y + OFFSET_PER_PIXEL * 6, transform.position.z);
        }
        else // in wall
        {
            collider.offset = new Vector2(collider.offset.x + OFFSET_PER_PIXEL * 2, collider.offset.y);
            collider.size = new Vector2(OFFSET_PER_PIXEL * 11, collider.size.y);
        }
    }*/

    public bool HasHit() => hit;
}
