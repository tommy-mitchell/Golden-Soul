using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed;
    public float reductionFactor;
    public bool  moveOnTouch;
    public float onTouchReturnCooldown;

    Transform playerParent;

    protected Vector3 startPosition;
    protected Vector3 endPosition;
    protected Vector3 returnPosition;

    protected bool isTouchingPlayer;
    protected bool directionIsForward;

    protected float currentMoveSpeed;
    protected float currentReductionFactor;
    protected float lowSpeedDist;

    protected float jumpTime;

    private Transform        swordParent;
    private PlayerController controller;

    protected void Awake() // temp as protected
    {
        startPosition = transform.position;
        endPosition   = transform.Find("toPoint").transform.position;
        returnPosition = startPosition;

        isTouchingPlayer   = false;
        directionIsForward = true;

        currentMoveSpeed       = moveSpeed;
        currentReductionFactor = reductionFactor;
        lowSpeedDist           = Mathf.Abs(Vector3.Distance(startPosition, endPosition)) * 0.15f;

        jumpTime = -1;

        //swordParent =  transform.Find("Grid").Find("Tilemap").transform;
        controller  = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void CollisionEnter(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isTouchingPlayer) // TODO: assert that player is on top of platform (raycast to player is normal??)
        {
            //moving = true;
            collision.collider.transform.SetParent(transform);
            collision.gameObject.GetComponent<PlayerController>().SetOnMovingPlatform(true);
            isTouchingPlayer = true;
        }
    }

    public void CollisionExit(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && isTouchingPlayer)
        {
            collision.gameObject.GetComponent<PlayerController>().SetOnMovingPlatform(false);
            collision.collider.transform.SetParent(null);
            isTouchingPlayer = false;

            jumpTime = Time.time + onTouchReturnCooldown;
        }
    }

    private bool PlayerIsHanging()
    {
        bool swordIsAttached = transform.Find("Grid").Find("Tilemap").Find("Sword(Clone)");

        if(!swordIsAttached)
            return false;
        Debug.Log("sword attached");
        bool playerIsHanging = controller.IsHanging();
        Debug.Log("hanging: " + playerIsHanging);
        if(playerIsHanging)
            jumpTime = Time.time + onTouchReturnCooldown;

        return playerIsHanging;
    }

    protected void FixedUpdate()
    {
        if(!moveOnTouch || isTouchingPlayer || PlayerIsHanging())
        {
            currentReductionFactor = ( currentReductionFactor > 0 ) ? reductionFactor : 1;

            if(directionIsForward)
            {
                if(transform.position != endPosition)
                    MoveTowards(endPosition);
                else
                    directionIsForward = false;
            }
            else
            {
                if(transform.position != startPosition)
                    MoveTowards(startPosition);
                else
                    directionIsForward = true;
            }
        }
        else if(NeedToReturn()) // return after grace period
        {
            if(transform.position != returnPosition)
                MoveTowards(returnPosition);
        }
    }

    protected void MoveTowards(Vector3 position)
    {
        currentMoveSpeed   = GetMoveSpeed(position);
        transform.position = Vector3.MoveTowards(transform.position, position, currentMoveSpeed * Time.deltaTime);
    }

    protected float GetMoveSpeed(Vector3 position)
    {
        float distance = Mathf.Abs(Vector3.Distance(transform.position, position));

        return ( distance < lowSpeedDist ) ? moveSpeed / reductionFactor : moveSpeed;
    }

    protected bool NeedToReturn()
    {
        return !(isTouchingPlayer || PlayerIsHanging()) && jumpTime < Time.time;
    }

    public void OnPlayerDeath()
    {
        transform.position = startPosition;
    }

    public bool IsTouchingPlayer()
    {
        return isTouchingPlayer || PlayerIsHanging();
    }
}
