using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOscillate : MonoBehaviour
{
    public float amplitude;
    public float frequency;
    public bool  vertical;

    void Update()
    {
        float sinOffset = Mathf.Sin(frequency * Time.time) * amplitude;

        if(vertical)
            transform.position = new Vector3(transform.position.x, sinOffset + transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(sinOffset + transform.position.x, transform.position.y, transform.position.z);
    }
}
