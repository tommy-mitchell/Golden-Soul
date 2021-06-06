using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
    public GameObject canvas;

    private Animator   batAnim;
    private Quaternion canvasRotation;

    private void Start()
    {
        batAnim        = GetComponent<Animator>();
        canvasRotation = canvas.transform.rotation;
    }

    private void Update()
    {
        canvas.SetActive(batAnim.GetBool("Flying"));
        canvas.transform.rotation = canvasRotation;
    }
}
