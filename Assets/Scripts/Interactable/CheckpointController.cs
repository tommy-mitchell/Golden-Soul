using UnityEngine;
using System.Collections;
public class CheckpointController : MonoBehaviour
{
    private bool       checkpointReached;

    private Animator   anim;
    private GameObject mostRecentCheckpoint;
    private GameObject light1;
    private GameObject light2;

    void Start()
    {
        checkpointReached = false;

        anim   = GetComponent<Animator>();
        light1 = transform.Find("Spot Light"        ).gameObject;
        light2 = transform.Find("Spot Light Reverse").gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !checkpointReached)
        {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();

            mostRecentCheckpoint = controller.GetMostRecentCheckpoint();

            if(mostRecentCheckpoint != null)
            {
                mostRecentCheckpoint.GetComponent<CheckpointController>().ToggleCheckpoint();
                controller.SetMostRecentCheckpoint(gameObject);
            }
            else
                controller.SetMostRecentCheckpoint(gameObject);


            ToggleCheckpoint();
        }
    }

    void ToggleCheckpoint()
    {
        checkpointReached = !checkpointReached;
        anim.SetBool("Activated", checkpointReached);

        LightSwitch(checkpointReached);
    }

    private void LightSwitch(bool isOn)
    {
        light1.SetActive(isOn);
        light2.SetActive(isOn);
    }
}