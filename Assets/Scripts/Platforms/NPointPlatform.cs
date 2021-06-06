using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPointPlatform : MonoBehaviour
{
    public float     moveSpeed;
    public float     reductionFactor;
    public bool      moveOnTouch;
    public float     onTouchReturnCooldown;
    public bool      stopAtEnd;
    public Transform initialTilemap;

    private Vector3          startPosition;
    private Vector3[]        toPoints;
    private Transform        currentTilemap;
    private PlayerController controller;

    private int   currentToPointIndex;
    private float currentReductionFactor;
    private bool  isTouchingPlayer;
    private float currentMoveSpeed;
    private float lowSpeedDist;
    private float jumpTime;
    private bool  hasReachedEnd;

    // OnSave -> serialize hasReachedEnd and unique ID

    private void Start()
    {
        startPosition = transform.position;

        GetListOfToPoints();

        currentToPointIndex = 1;

        isTouchingPlayer       = false;
        currentMoveSpeed       = moveSpeed;
        currentReductionFactor = reductionFactor;
        lowSpeedDist           = Mathf.Abs(Vector3.Distance(startPosition, toPoints[currentToPointIndex])) * 0.15f;
        jumpTime               = -1;
        hasReachedEnd          = false;

        currentTilemap = initialTilemap;
        controller     = GameObject.Find("Player").GetComponent<PlayerController>();

        GameObject.Find("Player").GetComponent<PlayerHealth>().OnDeath += OnPlayerDeath;
    }

    private void GetListOfToPoints()
    {
        List<Vector3> temp = new List<Vector3>();
        temp.Add(startPosition);

        Transform current = transform.Find("toPoints");

        if(current.childCount != 0) // no points
        {
            current = current.GetChild(0);
            temp.Add(current.position);

            while(current.childCount > 0)
            {
                current = current.GetChild(0);
                temp.Add(current.position);
            }
        }

        toPoints = temp.ToArray();
    }
    
    private void FixedUpdate()
    {
        if(!stopAtEnd || ( stopAtEnd && !hasReachedEnd ))
        {
            if(!moveOnTouch || isTouchingPlayer || PlayerIsHanging())
            {
                currentReductionFactor = (currentReductionFactor > 0) ? reductionFactor : 1;

                Vector3 toPosition = toPoints[currentToPointIndex];
                
                if(transform.position != toPosition)
                    //LERPMove(toPoints[currentToPointIndex - 1], toPosition);
                    MoveTowards(toPosition);
                else
                    GetNextPosition(); // stops at end if necessary
            }
            else if(NeedToReturn()) // return after grace period
            {
                Vector3 returnPosition;

                if(toPoints.Length == 2)
                    returnPosition = startPosition;
                else
                    returnPosition = toPoints[currentToPointIndex - 1];

                if(transform.position != returnPosition)
                    //LERPMove(toPoints[currentToPointIndex], returnPosition);
                    MoveTowards(returnPosition);
            }
        }
    }

    private void MoveTowards(Vector3 position)
    {
        currentMoveSpeed   = GetMoveSpeed(position);
        transform.position = Vector3.MoveTowards(transform.position, position, currentMoveSpeed * Time.fixedDeltaTime); // distance moved = speed * delta T
        
    }

    private void LERPMove(Vector3 startPosition, Vector3 endPosition)
    {
        //currentMoveSpeed   = GetMoveSpeed(endPosition);
        transform.position = Vector3.Lerp(startPosition, endPosition, (moveSpeed * Time.fixedDeltaTime) / Mathf.Abs(Vector3.Distance(transform.position, endPosition)));
    }

    private void GetNextPosition()
    {
        currentToPointIndex++;

        if(currentToPointIndex == toPoints.Length)
        {
            if(stopAtEnd)
            {
                hasReachedEnd = true;
                return;
            }

            System.Array.Reverse(toPoints);
            currentToPointIndex = 1;
        }
    }

    private float GetMoveSpeed(Vector3 position)
    {
        float distance = Mathf.Abs(Vector3.Distance(transform.position, position));

        return ( distance < lowSpeedDist ) ? moveSpeed / reductionFactor : moveSpeed;
    }

    public void CollisionEnter(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isTouchingPlayer) // TODO: assert that player is on top of platform (raycast to player is normal??)
        {
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

    private bool NeedToReturn() => !(isTouchingPlayer || PlayerIsHanging()) && jumpTime < Time.time;

    private bool PlayerIsHanging()
    {
        bool swordIsAttached = currentTilemap.Find("Sword(Clone)");

        if(!swordIsAttached)
            return false;

        bool playerIsHanging = controller.IsHanging();

        if(playerIsHanging)
            jumpTime = Time.time + onTouchReturnCooldown;

        return playerIsHanging;
    }

    private void OnPlayerDeath()
    {
        if(!hasReachedEnd) // only reset if not at end
        {
            transform.position = startPosition;

            if(toPoints[0] != startPosition)
                System.Array.Reverse(toPoints);

            currentToPointIndex = 1;
        }
    }

    public bool IsTouchingPlayer() => isTouchingPlayer || PlayerIsHanging();

    public bool HasStopped() => hasReachedEnd;
}
