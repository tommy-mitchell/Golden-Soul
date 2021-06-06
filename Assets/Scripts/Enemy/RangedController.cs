using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonMethods;

public class RangedController : EnemyController
{
    //public AudioSource arrowSound;
    public GameObject _arrowPrefab;

    private float defaultSpeed;
    private bool hasAttacked;

    private Animator anim;

    private GameObject arrow;

    private float scaredDistance;

    protected override void Setup()
    {
        base.Setup();

        anim = GetComponent<Animator>();

        hasAttacked = false;

        OnHitSound = audioManager.EnemyAudio.OnArcherDamage;
    }

    protected override void EnemyUpdate()
    {
        base.EnemyUpdate();

        if(!isMoving)
        {
            if(IsInRange(playerDistance, attackRange + 5))
            {
                RotateTowardsPosition(transform, player.position);
                anim.SetBool("Aiming", true);
            }
            else
                anim.SetBool("Aiming", false);
        }
    }

    protected override void Attack()
    {
        StartCoroutine(ArcherAttack());
    }

    IEnumerator ArcherAttack()
    {
        Vector3 attackStartPosition = transform.position;

        defaultSpeed = moveSpeed;

        while(!hasAttacked)
        {
            if(!IsInRange(playerDistance, attackRange))
            {
                EndAttack();
                yield break;
            }
            else if(IsInRange(playerDistance, attackRange))
            {
                anim.SetTrigger("Fired");
                FireProjectile();
                AudioSource.PlayClipAtPoint(audioManager.EnemyAudio.OnArcherArrowFire, transform.position);
                hasAttacked = !hasAttacked;
            }

            yield return null;
        }

        if(hasAttacked)
        {
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
        moveSpeed = defaultSpeed;
        hasAttacked = false;
        isAttacking = false;
    }

    void FireProjectile()
    {
        Vector3 archerPos = transform.position;

        arrow = Instantiate(_arrowPrefab, archerPos, _arrowPrefab.transform.rotation);
        arrow.GetComponent<ArrowController>().Fire(GetProjectileDirection(), GetProjectileRotation());
    }

    private Vector2 GetProjectileDirection()
    {
        return (player.transform.position - transform.position).normalized; //Omni-directional firing
    }

    private float GetProjectileRotation() // same as code for sword -> make a projectile controller (?) that projectiles inherit from
    {
        Vector3 targetDirection;
        Vector3 playerPos = player.transform.position;

        targetDirection.x = playerPos.x - transform.position.x;
        targetDirection.y = playerPos.y - transform.position.y;
        float fireAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        return fireAngle; // maybe this script shouldn't handle the angle of the projectile anyway -> just a "FireArrow()"
    }
}
