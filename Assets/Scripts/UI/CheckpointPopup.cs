using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPopup : MonoBehaviour
{
    public GameObject text;
    public bool       isActivated;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!isActivated && col.CompareTag("Player"))
        {
            isActivated = true;

            StartCoroutine(ShowText());
        }
    }

    private IEnumerator ShowText()
    {
        text.SetActive(true);

        yield return new WaitForSeconds(3f);

        text.SetActive(false);
    }
}
