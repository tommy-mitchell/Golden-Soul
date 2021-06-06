using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPointPlatform : MovingPlatform
{
    private Vector3 endPosition2;
    private Vector3 originalStartPosition;
    private Vector3 originalEndPosition;
    private bool    hasReachedPoint1;
    private bool    hasReachedEnd;

    public bool stopAtEnd;

    private void Awake()
    {
        base.Awake();

        endPosition2 = transform.Find("toPoint").Find("toPoint2").transform.position;

        originalStartPosition = startPosition;
        originalEndPosition   = endPosition;
        hasReachedPoint1      = false;
        hasReachedEnd         = false;
    }

    private void FixedUpdate()
    {
        if(!hasReachedEnd)
        {
            if(NeedToReturn())
            {
                startPosition = originalStartPosition;
                endPosition   = originalEndPosition;
            }

            if(transform.position == originalEndPosition)
            {
                if(!hasReachedPoint1)
                {
                    hasReachedPoint1 = true;
                    endPosition      = endPosition2;
                    returnPosition   = originalEndPosition;
                }
                else
                    returnPosition = startPosition;
            }
            else if(transform.position == endPosition2)
            {
                if(stopAtEnd)
                {
                    hasReachedEnd = true;
                    return;
                }

                endPosition    = originalEndPosition;
                returnPosition = originalEndPosition;
            }
            else if(transform.position == startPosition)
                hasReachedPoint1 = false;

            base.FixedUpdate();
        }
    }

    public bool HasStopped() => hasReachedEnd;
}
