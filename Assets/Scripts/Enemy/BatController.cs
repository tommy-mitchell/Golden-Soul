using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonMethods;

public class BatController : EnemyController
{
    public float       attackSpeed;
    //public AudioSource attackSound;

    private float defaultSpeed;
    private bool  hasAttacked;
    private bool  isAwake;

    private Animator anim;
    
    // animation tribool for attack

    private static int MOVING_IN     =  1;
    private static int NOT_ATTACKING =  0;
    private static int MOVING_OUT    = -1;

    protected override void Setup()
    {
        base.Setup();

        anim = GetComponent<Animator>();

        hasAttacked = false;
        isAwake     = false;

        OnHitSound = audioManager.EnemyAudio.OnBatDamage;
    }

    protected override void EnemyUpdate()
    {
        base.EnemyUpdate();

        if(!isMoving)
        {
            isAwake = IsInRange(playerDistance, playerRange + 3);

            if(isAwake)
                RotateTowardsPosition(transform, player.position);
        }
        else if(isMoving)
        {
            if(transform.position == startPosition)
                isMoving = false;
        }

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        anim.SetBool("Flying", isMoving);
        anim.SetBool("Awake",  isAwake);

        if(hit)
        {
            anim.SetTrigger("Hit");

            hit = false;
        }
    }

    protected override void Attack()
    {
        StartCoroutine("BatAttack");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "Player")
        {
            col.gameObject.GetComponent<PlayerHealth>().PlayerDamage(1, Vector2.zero);
            AudioSource.PlayClipAtPoint(audioManager.EnemyAudio.OnBatAttack, transform.position);

            hasAttacked = !hasAttacked;
        }
    }

    IEnumerator BatAttack()
    {
        Vector3 attackStartPosition = transform.position;

        anim.SetInteger("Attacking", MOVING_IN);
            defaultSpeed = moveSpeed;
            moveSpeed    = attackSpeed;

        while(!hasAttacked) // move in
        {
            if(!IsInRange(playerDistance, attackRange))
            {
                EndAttack();
                yield break;
            }

            Move(transform, player.position, moveSpeed);
            yield return null;
        }

        if(hasAttacked) // move out
        {
            anim.SetInteger("Attacking", MOVING_OUT);

            while(transform.position != attackStartPosition)
            {
                if(!IsInRange(playerDistance, attackRange))
                {
                    EndAttack();
                    yield break;
                }

                Move(transform, attackStartPosition, moveSpeed);
                yield return null;
            }
        }

        EndAttack();
    }

    void EndAttack()
    {
        anim.SetInteger("Attacking", NOT_ATTACKING);

        moveSpeed   = defaultSpeed;
        hasAttacked = false;
        isAttacking = false;
    }
}