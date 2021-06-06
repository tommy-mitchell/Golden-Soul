using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonMethods;

public class BossControllerOld : EnemyController
{
    public float attackSpeed;

    private int rand;
    private float defaultSpeed;
    private bool hasAttacked;
    private bool isAwake;

    private Animator anim;
    private BossHealth health;
    private Rigidbody2D rb;

    protected override void Setup()
    {
        base.Setup();

        anim = GetComponent<Animator>();
        health = GetComponent<BossHealth>();
        rb = GetComponent<Rigidbody2D>();

        hasAttacked = false;
        isAwake = false;
    }

    protected override void EnemyUpdate()
    {
        base.EnemyUpdate();

        if (!isMoving)
        {
            isAwake = IsInRange(playerDistance, playerRange + 3);

            if (isAwake)
                RotateTowardsPosition(transform, player.position);
        }
        else if (isMoving)
        {
            if (transform.position == startPosition)
                isMoving = false;
        }

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        anim.SetBool("Running", isMoving);
    }

    protected override void Attack()
    {
        //if (health.GetHealth() == 3)
        //{
            StartCoroutine("BossPhase1");
        //}
        //else if (health.GetHealth() == 2)
        //{
        //    //StopCoroutine("BossPhase1");
        //    //StartCoroutine("BossPhase2");
        //}
        //else
        //{
        //    StopCoroutine("BossPhase2");
        //    //StartCoroutine("BossPhase3");
        //}
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            col.gameObject.GetComponent<PlayerHealth>().PlayerDamage(1, Vector2.zero);

            hasAttacked = !hasAttacked;
            Physics2D.IgnoreCollision(col.collider, GetComponent<BoxCollider2D>());
        }
        else if (col.gameObject.CompareTag("Enemy"))
            Physics2D.IgnoreCollision(col.collider, GetComponent<BoxCollider2D>());
    }

    IEnumerator BossPhase1()
    {
        int sign = (transform.rotation.y == 0) ? 1 : -1;

        bool attackDecided = false;
        //Vector3 attackStartPosition = transform.position;
        Vector3 attackStartPosition = new Vector3(player.position.x + (attackRange * -sign), transform.position.y, transform.position.z);

        defaultSpeed = moveSpeed;
        moveSpeed = attackSpeed;

        while (!hasAttacked) // move in
        {
            if (!IsInRange(playerDistance, attackRange))
            {
                EndAttack();
                yield break;
            }

            /*New stuff messing around with, jumping boss and sometimes jumps over player*/
            if(attackDecided == false)
            {
                rand = Random.Range(1, 2);
                attackDecided = true;
                Debug.Log(rand);
            }

            if (rand == 0) //Small jump towards attacking player
            {
                Jump(10, 0, rb);
                Move(transform, player.position, moveSpeed);
            }
            else if (rand == 1) //Jump and slam down
            {

                if (rb.velocity.y == 0)
                {
                    Jump(15, 0, rb);
                    rb.gravityScale = 0.7f;
                }
                
                yield return new WaitForSeconds(0.75f);

                Jump(-15, 0, rb);
                rb.gravityScale = 1;
                //Move(transform, player.position, moveSpeed);
            }
            else if (rand == 2) //Dash behind player
            {
                //Physics2D.IgnoreCollision(player.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                //Move(transform, new Vector3(player.position.x + (10 * sign), player.position.y, player.position.z), moveSpeed);
            }
            else //Normal attack
            {
                Move(transform, player.position, moveSpeed);
            }

            yield return null;
        }

        if (hasAttacked) // move out
        {
            while (transform.position != attackStartPosition)
            {
                if (!IsInRange(playerDistance, attackRange))
                {
                    EndAttack();
                    yield break;
                }

                transform.position = Vector3.MoveTowards(transform.position, attackStartPosition, moveSpeed * Time.deltaTime); //Move without rotating to look ready for another attack
                yield return null;
            }


        }

        EndAttack();
    }

    IEnumerator BossPhase2()
    {
        Vector3 attackStartPosition = transform.position;

        defaultSpeed = moveSpeed;
        moveSpeed = attackSpeed;

        while (!hasAttacked) // move in
        {
            if (!IsInRange(playerDistance, attackRange))
            {
                EndAttack();
                yield break;
            }

            yield return null;
        }

        if (hasAttacked) // move out
        {
            while (transform.position != attackStartPosition)
            {
                if (!IsInRange(playerDistance, attackRange))
                {
                    EndAttack();
                    yield break;
                }

                transform.position = Vector3.MoveTowards(transform.position, attackStartPosition, moveSpeed * Time.deltaTime); //Move without rotating to look ready for another attack
                yield return null;
            }
        }

        EndAttack();
    }

    IEnumerator BossPhase3()
    {
        Vector3 attackStartPosition = transform.position;

        defaultSpeed = moveSpeed;
        moveSpeed = attackSpeed;

        while (!hasAttacked) // move in
        {
            if (!IsInRange(playerDistance, attackRange))
            {
                EndAttack();
                yield break;
            }

            Move(transform, player.position, moveSpeed);
            yield return null;
        }

        if (hasAttacked) // move out
        {
            while (transform.position != attackStartPosition)
            {
                if (!IsInRange(playerDistance, attackRange))
                {
                    EndAttack();
                    yield break;
                }

                transform.position = Vector3.MoveTowards(transform.position, attackStartPosition, moveSpeed * Time.deltaTime); //Move without rotating to look ready for another attack
                yield return null;
            }
        }

        EndAttack();
    }

    void EndAttack()
    {
        moveSpeed = defaultSpeed;
        hasAttacked = false;
        isAttacking = false;
        Physics2D.IgnoreCollision(player.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }

    private bool NeedToRotate() //Trying to implement wait time before guard turns so player can hit weakspot in back
    {
        bool turn = false;

        if (player.position.x > transform.position.x && Mathf.Approximately(transform.rotation.y, -180))
            turn = true;
        else if (player.position.x < transform.position.x && Mathf.Approximately(transform.rotation.y, 0))
            turn = true;

        return turn;
    }
}