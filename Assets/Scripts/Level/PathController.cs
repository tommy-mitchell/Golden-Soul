using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public event Action OnPathFinish;

    public void OnFinish() => OnPathFinish?.Invoke();
}
