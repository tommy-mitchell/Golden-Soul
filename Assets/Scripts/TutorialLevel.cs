using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    private int                counter;
    private List<GameObject>   triggers;
    private Transform          text;
    private PlayerStateManager stateManager;

    [SerializeField]
    private NewInput input;

    private bool hasThrown;
    [SerializeField]
    private GameObject _bat;
    [SerializeField]
    private GameObject _heart;
    [SerializeField]
    private GameObject _door;

    private void Start()
    {
        counter      = 0;
        text         = transform.Find("Tutorial Text");
        stateManager = GameObject.Find("Player State").GetComponent<PlayerStateManager>();

        SetupList();

        input.Player_onThrow += SetThrow;
        GameObject.Find("Player").GetComponent<PlayerCollectables>().OnKeyCollect += SetDoor;
    }

    private void SetupList()
    {
        triggers = new List<GameObject>();

        Transform triggersObject = transform.Find("Trigger Points");

        foreach(Transform trigger in triggersObject)
            triggers.Add(trigger.gameObject);

        Debug.Log("size of triggers: " + triggers.Count);
    }

    private void SetThrow() => hasThrown = true;

    private void SetDoor(string dummyInput) => _door.SetActive(true);

    private void OnDisable()
    {
        input.Player_onThrow -= SetThrow;
        GameObject.Find("Player").GetComponent<PlayerCollectables>().OnKeyCollect -= SetDoor;

        stateManager.ClearKeys();
        GameObject.Find("Player").GetComponent<PlayerCollectables>().UpdateText();
        GameObject.Find("Player").transform.position = new Vector2(-5f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("trigger: " + (counter + 1));

            if(counter + 1 < 11)
            {
                float y = counter + 1 > 4 ? 1 : counter + 1 > 6 ? 5 : 0;
                stateManager.RespawnLocation = new Vector3(triggers[counter].transform.position.x, y, 0);
            }

            triggers[counter].SetActive(false);
            counter++;

            if(counter == 6 || counter == 9 || counter == 12 || counter == 7 || counter == 10 || counter == 13)
            {
                text.Find((counter - 1).ToString()).gameObject.SetActive(false);
                text.Find(      counter.ToString()).gameObject.SetActive( true);

                if(counter == 6)
                    StartCoroutine(Trigger6());
                else if(counter == 9)
                    StartCoroutine(Trigger9());
                else if(counter == 12)
                    StartCoroutine(Trigger12());
            }
            else
            {
                counter--;

                text.Find(      counter.ToString()).gameObject.SetActive(false);
                text.Find((counter + 1).ToString()).gameObject.SetActive( true);

                counter++;
            }

            if(counter == 13)
                DontDestroyOnLoad(GameObject.Find("Player"));
        }
    }

    private IEnumerator Trigger6()
    {
        hasThrown = false;

        while(!hasThrown)
            yield return null;

        text.Find("6").Find("Throw"            ).gameObject.SetActive(false);
        //text.Find("6").Find("Teleport / Recall").gameObject.SetActive( true);
        text.Find("6").GetChild(1).gameObject.SetActive(true);
    }

    private IEnumerator Trigger9()
    {
        while(_bat.activeSelf)
            yield return null;

        _heart.SetActive(true);

        text.Find("9").Find("Attack").gameObject.SetActive(false);
        text.Find("9").Find("Heart" ).gameObject.SetActive( true);
    }

    private IEnumerator Trigger12()
    {
        hasThrown = false;

        while(!hasThrown)
            yield return null;

        text.Find("12").Find("Grates"  ).gameObject.SetActive(false);
        text.Find("12").Find("Teleport").gameObject.SetActive( true);
    }
}