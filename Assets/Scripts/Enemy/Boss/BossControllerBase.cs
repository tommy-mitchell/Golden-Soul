using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonMethods;
public abstract class BossControllerBase : MonoBehaviour
{
    public float attackCooldown;
    public GameObject phase2Block;

    protected Transform player;    
    protected Vector3 startPosition;
    
    protected float playerPosition;
    protected float attackTimeStamp;
    
    protected bool isAttacking;
    protected bool hasStarted = false;

    protected Animator anim;
    protected int phase;

    void Start()
    {
        //Setup();
    }

    void FixedUpdate()
    {
        if(hasStarted)
            BossUpdate();
    }

    protected virtual void Setup()
    {
        player = GameObject.FindWithTag("Player").transform;
        startPosition = transform.position;
        attackTimeStamp = 0;
        anim = GetComponent<Animator>();

        phase = 1;

        isAttacking = false;
        hasStarted = true;
    }

    protected virtual void BossUpdate()
    {
        RotateTowardsPosition(transform, player.position);

        if(CanAttack())
        {
            if(phase == 2)
                phase2Block.SetActive(false);

            Attack();
        }
        else if(phase != 1 && !isAttacking)
            phase2Block.SetActive(true);
        else // phase == 1 || isAttacking
            phase2Block.SetActive(false);

        UpdateAnimations();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            col.gameObject.GetComponent<PlayerHealth>().PlayerDamage(1, Vector2.zero);
        }
    }

    protected abstract void Attack();

    protected bool CanAttack()
    {
        return !isAttacking && attackTimeStamp <= Time.time;
    }

    public void BeginFight(){
        Setup();
    }

    protected void UpdateAnimations()
    {
        anim.SetInteger("Phase", phase);
        anim.SetBool("Attacking", isAttacking);
    }

    public void OnHit()
    {
        anim.SetTrigger("Hit");
    }
}
