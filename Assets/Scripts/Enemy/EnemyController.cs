using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static CommonLibrary.CommonMethods;

public abstract class EnemyController : MonoBehaviour
{
    public float moveSpeed;      // 3
    public float playerRange;    // 5
    public float attackRange;    // 2
    public float attackCooldown; // 5

    public AudioClip OnHitSound { get; protected set; }

    protected Transform player;
    protected float     playerDistance;
    protected Vector3   startPosition;
    protected float     attackTimeStamp;

    protected bool isMoving;
    protected bool isAttacking;
    protected bool hit;

    protected AudioManager audioManager;

    void Start() => Setup();

    void FixedUpdate() => EnemyUpdate();

    protected virtual void Setup()
    {
        player          = GameObject.FindWithTag("Player").transform;
        startPosition   = transform.position;
        attackTimeStamp = 0;

        isMoving    = false;
        isAttacking = false;
        hit         = false;

        audioManager = GameObject.Find("Audio").GetComponent<AudioManager>();
    }

    protected virtual void EnemyUpdate()
    {
        playerDistance = Vector2.Distance(transform.position, player.position);

        if(CanAttack())
        {
            Attack();
            attackTimeStamp = Time.time + attackCooldown;
        }
        else if(!IsInRange(playerDistance, attackRange))
        {
            if(IsInRange(playerDistance, playerRange))
            {
                Move(transform, player.position, moveSpeed);

                isMoving = true;
            }
            else if(transform.position != startPosition)
                Move(transform, startPosition, moveSpeed);
            else
                isMoving = false;
        }
    }

    protected abstract void Attack();

    // helper methods

    protected bool CanAttack() => IsInRange(playerDistance, attackRange) && !isAttacking && attackTimeStamp <= Time.time;

    public void OnHit() => hit = true;
}