using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommonLibrary.CommonMethods;

public class IntroCutscene : MonoBehaviour
{/*
    public Vector3 stopPosition; // -7.260018, 11
    public Camera  camera; // x: -6.822518

    private Animator         anim;
    private PlayerThrow      sword;
    private PlayerController controller;
    private GameObject       ui;

    private float moveSpeed;

    void Start()
    {
        anim       = GetComponent<Animator>();
        sword      = GetComponent<PlayerThrow>();
        controller = GetComponent<PlayerController>();
        ui         = GameObject.Find("UI");

        moveSpeed = controller.speed - 2;

        if(transform.position.x < -20f) // quick fix to stop cutscene from playing after loading from menu
        {
            SetObjectStates(false);
            StartCoroutine(Cutscene());
        }
    }

    private void SetObjectStates(bool state)
    {
        camera.GetComponent<CameraPlayerFollow>().enabled = state;
        ui.SetActive(state);
        controller.TEMP_INPUT_DISABLE_FLAG = !state;

        // playerController.SetInputChecking(state); -> refactor code so that PC receives inputs from some input script
    }

    private IEnumerator Cutscene()
    {
        // static cam -> watch player walk into frame and stop
        while(transform.position != stopPosition)
        {
            anim.SetBool("Running", true);
            Move(transform, stopPosition, moveSpeed);

            controller.MoveUpdate(1, true); // for sound effects

            yield return null;
        }

        controller.MoveUpdate(0, true);
        anim.SetBool("Running", false);
        RotateTowardsPosition(transform, Vector3.zero); // x is less than 0 -> rotate right

        yield return new WaitForSeconds(.5f);

        // player throws sword down hole

        controller.SetHasSword(false);
        controller.SetThrowingState(Vector2.down);
        sword.ThrowSword(Vector2.down);

        // cam follows sword

        camera.GetComponent<CameraSwordFollow>().enabled = true;

        // hits ground -> short pause -> player teleports -> game starts
        while(!sword.HasHit())
            yield return null;

        yield return new WaitForSeconds(.4f);

        sword.TeleportToSword();
        controller.SetThrowingState(Vector2.zero);

        camera.GetComponent<CameraSwordFollow>().enabled = false;
        camera.ResetAspect();

        yield return new WaitForSeconds(.25f);

        SetObjectStates(true);

        //controller.SetMostRecentCheckpoint(GameObject.Find("Invisible Start"));

        yield break;
    }*/
}