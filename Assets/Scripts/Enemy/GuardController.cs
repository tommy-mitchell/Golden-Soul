using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonMethods;
using static CommonLibrary.CommonDefinitions;

public class GuardController : EnemyController
{
    public float attackSpeed;

    private float defaultPlayerRange;
    private float defaultSpeed;
    private bool  hasAttacked;
    private bool  isAwake;
    private bool  retreat;

    private Animator      anim;
    private BoxCollider2D weaponCollider;

    protected override void Setup()
    {
        base.Setup();

        anim           = GetComponent<Animator>();
        weaponCollider = GetComponentInChildren<BoxCollider2D>();

        hasAttacked = false;
        isAwake     = false;
        defaultPlayerRange = playerRange;

        OnHitSound = audioManager.EnemyAudio.OnGuardDamage;
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
            if(transform.position.x == startPosition.x)
                isMoving = false;
        }

        if(!IsInRange(playerDistance, defaultPlayerRange))
            playerRange = defaultPlayerRange;

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        anim.SetBool("Running", isMoving);
    }

    protected override void Attack()
    {
        StartCoroutine(GuardAttack());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
            Physics2D.IgnoreCollision(col.collider, GetComponent<BoxCollider2D>());
        else if(col.gameObject.layer == LayerMask.NameToLayer("Grates"))
        {
            EndAttack();
            playerRange = 0;
        }
    }

    IEnumerator GuardAttack()
    {
        Vector3 attackStartPosition = transform.position;

        defaultSpeed = moveSpeed;
        moveSpeed    = attackSpeed;

        anim.SetBool("Running", true);

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

        anim.SetBool("Running", false);
        anim.SetTrigger("Attacking");
        weaponCollider.offset.Set(weaponCollider.offset.x + OFFSET_PER_PIXEL * 3, weaponCollider.offset.y);
        yield return new WaitForSeconds(.5f);
        weaponCollider.offset.Set(weaponCollider.offset.x - OFFSET_PER_PIXEL * 3, weaponCollider.offset.y);

        anim.SetBool("Running", true);

        if(hasAttacked) // move out
        {
            while(transform.position != attackStartPosition)
            {
                if(!IsInRange(playerDistance, attackRange))
                {
                    EndAttack();
                    yield break;
                }

                transform.position = Vector3.MoveTowards(transform.position, attackStartPosition, moveSpeed * Time.deltaTime); //Move without rotating to look ready for another attack
                yield return null;
            }
        }

        if(IsInRange(playerDistance, attackRange))
            isMoving = false;

        EndAttack();
    }

    void EndAttack()
    {
        moveSpeed   = defaultSpeed;
        hasAttacked = false;
        isAttacking = false;
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

    public void OnAttackHit() // change to event
    {
        hasAttacked = true;
    }
}